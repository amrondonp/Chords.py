using Chords.Repositories;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chords.MachineLearning
{
    public class ChordData
    {
        [LoadColumn(0)] public float C { get; set; }

        [LoadColumn(1)] public float CSharp { get; set; }

        [LoadColumn(2)] public float D { get; set; }

        [LoadColumn(3)] public float DSharp { get; set; }

        [LoadColumn(4)] public float E { get; set; }

        [LoadColumn(5)] public float F { get; set; }

        [LoadColumn(6)] public float FSharp { get; set; }

        [LoadColumn(7)] public float G { get; set; }

        [LoadColumn(8)] public float GSharp { get; set; }

        [LoadColumn(9)] public float A { get; set; }

        [LoadColumn(10)] public float ASharp { get; set; }

        [LoadColumn(11)] public float B { get; set; }

        [LoadColumn(12), ColumnName("Chord")] public string Chord { get; set; }
    }

    public class ChordPredictionResult
    {
        public string ChordPrediction { get; set; }
    }

    public class AutoMlModelCreation
    {
        public static readonly MLContext MlContextInstance = new MLContext();

        public static (IDataView, ITransformer,
            ExperimentResult<MulticlassClassificationMetrics>)
            CreateDataViewAndTransformer(
                string trainDataFile, uint timeoutInSeconds)
        {
            var trainData = MlContextInstance.Data.LoadFromTextFile<ChordData>(
                trainDataFile,
                hasHeader: true,
                separatorChar: ','
            );

            return CreateTransformerGivenDataView(trainData, timeoutInSeconds);
        }

        public static (IDataView, ITransformer, ExperimentResult<MulticlassClassificationMetrics>) CreateTransformerGivenDataView(IDataView trainData, uint timeoutInSeconds)
        {
            var experiment = MlContextInstance.Auto()
                .CreateMulticlassClassificationExperiment(timeoutInSeconds);
            var result = experiment.Execute(
                trainData,
                "Chord",
                preFeaturizer: MlContextInstance.Transforms.Conversion
                    .MapValueToKey("Chord")
            );

            var pipeline = result.BestRun.Estimator.Append(
                MlContextInstance.Transforms.Conversion.MapKeyToValue(
                    "ChordPrediction", "PredictedLabel"
                ));

            var modelWithLabelMapping = pipeline.Fit(trainData);

            return (trainData, modelWithLabelMapping, result);
        }

        public static (ExperimentResult<MulticlassClassificationMetrics>,
            PredictionEngine<ChordData, ChordPredictionResult>) CreateModel(
                string trainDataFile, uint timeoutInSeconds)
        {
            var (_, modelWithLabelMapping, result) =
                CreateDataViewAndTransformer(trainDataFile, timeoutInSeconds);

            var engine =
                MlContextInstance.Model
                    .CreatePredictionEngine<ChordData, ChordPredictionResult>(
                        modelWithLabelMapping);

            return (result, engine);
        }

        public static (ExperimentResult<MulticlassClassificationMetrics>,
            PredictionEngine<ChordData, ChordPredictionResult>) CreateModelGivenDataView(IDataView trainData, uint timeoutInSeconds)
        {
            var (_, modelWithLabelMapping, result) =
               CreateTransformerGivenDataView(trainData, timeoutInSeconds);

            var engine =
                MlContextInstance.Model
                    .CreatePredictionEngine<ChordData, ChordPredictionResult>(
                        modelWithLabelMapping);

            return (result, engine);
        }

        public static async Task<(ExperimentResult<MulticlassClassificationMetrics>,
            PredictionEngine<ChordData, ChordPredictionResult>)> CreateModelGivenInitialDataAndStoredChordsFolder(
                string originalTrainingDataFile,
                string originalTestFile,
                string storedChordsFolder,
                uint timeoutInSeconds,
                string outputFolder)
        {
            var currentTime = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");

            var trainDataGenerator = new FileSystemTrainDataGenerator(storedChordsFolder, Path.Combine(storedChordsFolder, "trainData.csv"));
            await trainDataGenerator.GenerateTrainData();
            var textLoader = AutoMlModelCreation.MlContextInstance
                .Data.CreateTextLoader<ChordData>(separatorChar: ',', hasHeader: true);

            var trainDataFiles = Directory.GetFiles(storedChordsFolder, "*.csv");
            var trainData = textLoader.Load(trainDataFiles.Append(originalTrainingDataFile).ToArray());

            var (_, modelWithLabelMapping, result) =
                CreateTransformerGivenDataView(trainData, timeoutInSeconds);

            var engine =
                MlContextInstance.Model
                    .CreatePredictionEngine<ChordData, ChordPredictionResult>(
                        modelWithLabelMapping);

            var validationMetrics =
                AutoMlModelCreation.EvaluateModel(result,
                    originalTestFile);

            MlContextInstance.Model.Save(
                modelWithLabelMapping, trainData.Schema,
                $@"{outputFolder}{currentTime}S{timeoutInSeconds}L{validationMetrics.LogLoss}.model");

            return (result, engine);
        }

        public static MulticlassClassificationMetrics EvaluateModel(
            ExperimentResult<MulticlassClassificationMetrics> experimentResult,
            string trainDataFile)
        {
            var bestRun = experimentResult.BestRun;
            var trainedModel = bestRun.Model;

            var testData = MlContextInstance.Data.LoadFromTextFile<ChordData>(
                trainDataFile,
                hasHeader: true,
                separatorChar: ','
            );

            var predictions = trainedModel.Transform(testData);
            return MlContextInstance.MulticlassClassification.Evaluate(
                data: predictions, labelColumnName: "Chord");
        }

        public static ChordData GetChordDataFromPcp(double[] pcp)
        {
            return new ChordData
            {
                C = (float)pcp[0],
                CSharp = (float)pcp[1],
                D = (float)pcp[2],
                DSharp = (float)pcp[3],
                E = (float)pcp[4],
                F = (float)pcp[5],
                FSharp = (float)pcp[6],
                G = (float)pcp[7],
                GSharp = (float)pcp[8],
                A = (float)pcp[9],
                ASharp = (float)pcp[10],
                B = (float)pcp[11],
                Chord = ""
            };
        }
    }
}
