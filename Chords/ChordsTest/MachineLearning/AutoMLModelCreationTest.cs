using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chords.MachineLearning;

namespace ChordsTest.MachineLearning
{
    [TestClass]
    public class AutoMLModelCreationTest
    {
        [TestMethod]
        public void GetModel_TrainsTheModelCorrecly()
        {
            var trainingResult = AutoMLModelCreation.CreateModel("./Resources/trainData.csv");
            Assert.IsNotNull(trainingResult);
        }
    }
}
