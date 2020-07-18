using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chords.MachineLearning;
using Microsoft.ML;

namespace ChordsTest.MachineLearning
{
    [TestClass]
    public class AutoMLModelCreationTest
    {
        [TestMethod]
        public void GetModel_TrainsTheModelCorrecly()
        {
            var experimentResult = AutoMLModelCreation.CreateModel("./Resources/trainData.csv");
            Assert.IsNotNull(experimentResult);

            var bestRun = experimentResult.BestRun;
            var trainedModel = bestRun.Model;

            var mLContext = new MLContext();
            var testData = mLContext.Data.LoadFromTextFile<ChordData>(
                "./Resources/testData.csv",
                hasHeader: true,
                separatorChar: ','
            );

            var predictions = trainedModel.Transform(testData);
            var metrics = mLContext.MulticlassClassification.Evaluate(data: predictions, labelColumnName: "Chord", scoreColumnName: "Score");
            
            Assert.IsTrue(metrics.LogLoss < 0.5);
        }
    }
}
