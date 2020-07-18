using Chords.Predictors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ChordsTest.Predictors
{
    [TestClass]
    public class PredictorTest
    {
        [TestMethod]
        public void ClassicPredictor_GetPredictionsForSample_CallsGetPredictionCorrectly()
        {
            var classicPredictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            Assert.AreEqual(classicPredictor.GetPrediction(samples, sampleRate), "D");
        }

        [TestMethod]
        public void ClassicPredictor_GetPredictionsForLongSample_CallsGetPredictionCorrectly()
        {
            var classicPredictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/about.wav");
            var predictions =
                classicPredictor.GetPredictions(samples, sampleRate, 500, new Progress<int>());

            Assert.IsTrue(predictions.Contains("G"));
            Assert.IsTrue(predictions.Contains("Em"));
        }

        [TestMethod]
        public void ClassicPredictor_GetPredictionsForFile_CallsGetPredictionCorrectly()
        {
            var classicPredictor = new ClassicPredictor();
            var predictions =
                classicPredictor.GetPredictionForFile("./Resources/about.wav", new Progress<int>(), 500);

            Assert.IsTrue(predictions.Contains("G"));
            Assert.IsTrue(predictions.Contains("Em"));
        }
    }
}
