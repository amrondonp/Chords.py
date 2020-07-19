using Chords.MachineLearning;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System;

namespace ChordTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            uint secondsToRun = 1;
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Console.WriteLine(
                $@"Training Prediction model with AutoML for {secondsToRun} seconds");

            var (trainData, modelWithLabelMapping, experimentResult) =
                AutoMlModelCreation.CreateDataViewAndTransformer(
                    "./Resources/trainData.csv", secondsToRun);

            Console.WriteLine();
            GetAndPrintValidationMetricsForData(experimentResult, "./Resources/testData.csv");
            Console.WriteLine();
            GetAndPrintValidationMetricsForData(experimentResult, "./Resources/trainData.csv");
            Console.WriteLine();

            AutoMlModelCreation.MlContextInstance.Model.Save(
                modelWithLabelMapping, trainData.Schema,
                $@"./generatedModels/model{currentTime}.model");
        }

        private static void GetAndPrintValidationMetricsForData(
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
        }
    }
}
