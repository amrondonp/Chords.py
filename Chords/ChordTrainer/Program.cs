using Chords.MachineLearning;
using Chords.Repositories;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChordTrainer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            args = new string[] { "180", "C:\\Users\\anrondon\\Desktop\\Coding\\Chords.py\\Chords\\ChordsDesktop\\bin\\Debug\\netcoreapp3.1\\storedChords" };
            uint secondsToRun = 1;
            string directory = null;

            if (args.Length >= 1)
            {
                secondsToRun = uint.Parse(args[0]);
            }

            if(args.Length >= 2)
            {
                directory = args[1];
            }

            if(directory != null)
            {
                var trainDataGenerator = new FileSystemTrainDataGenerator(directory, Path.Combine(directory, "trainData.csv"));
                await trainDataGenerator.GenerateTrainData();
            }

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Console.WriteLine(
                $@"Training Prediction model with AutoML for {secondsToRun} seconds");

            var textLoader = AutoMlModelCreation.MlContextInstance
                .Data.CreateTextLoader<ChordData>(separatorChar: ',', hasHeader: true);

            var trainDataFiles = Directory.GetFiles(directory, "*.csv");
            var trainData = textLoader.Load(trainDataFiles.Append("./Resources/trainData.csv").ToArray());

            var (_, modelWithLabelMapping, experimentResult) =
                AutoMlModelCreation.CreateTransformerGivenDataView(trainData, secondsToRun);


            Console.WriteLine();
            GetAndPrintValidationMetricsForData(experimentResult, "./Resources/testData.csv");
            Console.WriteLine();
            var validationMetrics = GetAndPrintValidationMetricsForData(experimentResult, "./Resources/trainData.csv");
            Console.WriteLine();

            AutoMlModelCreation.MlContextInstance.Model.Save(
                modelWithLabelMapping, trainData.Schema,
                $@"./generatedModels/model{currentTime}S{secondsToRun}L{validationMetrics.LogLoss}.model");
        }

        private static MulticlassClassificationMetrics GetAndPrintValidationMetricsForData(
            ExperimentResult<MulticlassClassificationMetrics> experimentResult, string fileName)
        {
            Console.WriteLine($@"Running experiment for dataset {fileName}");
            var validationMetrics =
                AutoMlModelCreation.EvaluateModel(experimentResult,
                    fileName);

            Console.WriteLine(@"Experiment ran with the following results");
            Console.WriteLine(
                $@"LogLoss={validationMetrics.LogLoss} the closer to 0 the better");
            Console.WriteLine($@"Confusion Matrix Actuals\Predicted");

            var confusionMatrix = validationMetrics.ConfusionMatrix;

            for (var i = 0; i < confusionMatrix.NumberOfClasses; i++)
            {
                for (var j = 0; j < confusionMatrix.NumberOfClasses; j++)
                {
                    Console.Write(confusionMatrix.Counts[i][j] + "\t");
                }

                Console.WriteLine();
            }

            return validationMetrics;
        }
    }
}
