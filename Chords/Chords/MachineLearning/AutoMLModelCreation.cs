using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace Chords.MachineLearning
{
    public class ChordData
    {
        [LoadColumn(0)]
        public float C { get; set; }

        [LoadColumn(1)]
        public float CSharp { get; set; }

        [LoadColumn(2)]
        public float D { get; set; }

        [LoadColumn(3)]
        public float DSharp { get; set; }

        [LoadColumn(4)]
        public float E { get; set; }

        [LoadColumn(5)]
        public float F { get; set; }

        [LoadColumn(6)]
        public float FSharp { get; set; }

        [LoadColumn(7)]
        public float G { get; set; }

        [LoadColumn(8)]
        public float GSharp { get; set; }

        [LoadColumn(9)]
        public float A { get; set; }

        [LoadColumn(10)]
        public float ASharp { get; set; }

        [LoadColumn(11)]
        public float B { get; set; }

        [LoadColumn(12), ColumnName("Chord")]
        public string Chord { get; set; }
    }

    public class AutoMLModelCreation
    {
        public static ExperimentResult<MulticlassClassificationMetrics> CreateModel(string trainDataFile, uint timeoutInSeconds)
        {
            var mLContext = new MLContext();
            var trainData = mLContext.Data.LoadFromTextFile<ChordData>(
                trainDataFile,
                hasHeader: true,
                separatorChar: ','
            );

            var experiment = mLContext.Auto().CreateMulticlassClassificationExperiment(timeoutInSeconds);
            var result = experiment.Execute(trainData, "Chord");
            return result;
        }

        public static MulticlassClassificationMetrics EvaluateModel(ExperimentResult<MulticlassClassificationMetrics> experimentResult, string trainDataFile)
        {
            var bestRun = experimentResult.BestRun;
            var trainedModel = bestRun.Model;

            var mLContext = new MLContext();
            var testData = mLContext.Data.LoadFromTextFile<ChordData>(
                trainDataFile,
                hasHeader: true,
                separatorChar: ','
            );

            var predictions = trainedModel.Transform(testData);
            return mLContext.MulticlassClassification.Evaluate(data: predictions, labelColumnName: "Chord", scoreColumnName: "Score");
        }
    }
}
