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
            var experimentResult = AutoMLModelCreation.CreateModel("./Resources/trainData.csv", 1);
            Assert.IsNotNull(experimentResult);

            var metrics = AutoMLModelCreation.EvaluateModel(experimentResult, "./Resources/testData.csv");
            Assert.IsTrue(metrics.LogLoss < 0.5);
        }
    }
}
