using Chords.Entities;
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
            var predictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            Assert.AreEqual(predictor.GetPrediction(samples, sampleRate), "D");
        }

        [TestMethod]
        public void ClassicPredictor_GetPredictionsForSample_CallsGetPredictionCorrectlyWithChord()
        {
            var predictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var chord = predictor.GetPredictionWithChord(samples, sampleRate);
            Assert.AreEqual(chord.Name, "D");

            var pcp = Chords.Profiling.Profiling.PitchClassProfileForSamples(samples, sampleRate);
            Assert.AreEqual(sampleRate, chord.SampleRate);
            
            Assert.AreEqual(pcp.Length, chord.Pcp.Length);
            for (var i = 0; i < pcp.Length; i++)
            {
                Assert.IsTrue(Math.Abs(pcp[i] - chord.Pcp[i]) < 1e-8);
            }

            Assert.AreEqual(samples.Length, chord.Samples.Length);
            for (var i = 0; i < samples.Length; i++)
            {
                Assert.IsTrue(Math.Abs(samples[i] - chord.Samples[i]) < 1e-8);
            }
        }

        [TestMethod]
        public void ClassicPredictor_GetPredictionsForLongSample_CallsGetPredictionCorrectly()
        {
            var predictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/about.wav");
            var predictions =
                predictor.GetPredictions(samples, sampleRate, 500, new Progress<int>());

            Assert.IsTrue(predictions.Contains("G"));
            Assert.IsTrue(predictions.Contains("Em"));
        }

        [TestMethod]
        public void ClassicPredictor_GetPredictionsWithChordsForLongSample_CallsGetPredictionCorrectly()
        {
            var predictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/about.wav");
            var chords =
                predictor.GetPredictionsWithChords(samples, sampleRate, 500, new Progress<int>());

            var gChord = chords.FirstOrDefault(chord => chord.Name.Equals("G"));
            var emChord = chords.FirstOrDefault(chord => chord.Name.Equals("Em"));

            Assert.AreEqual(gChord.Name, "G");
            Assert.AreEqual(emChord.Name, "Em");

            bool areArraysEqual = true;
            for (int i = 0; i < Math.Min(gChord.Samples.Length, emChord.Samples.Length); i++)
            {
                if (Math.Abs(gChord.Samples[i] - emChord.Samples[i]) > 1e-8)
                {
                    areArraysEqual = false;
                }
            }

            Assert.IsFalse(areArraysEqual);
        }

        [TestMethod]
        public void ClassicPredictor_GetPredictionsForFile_CallsGetPredictionCorrectly()
        {
            var predictor = new ClassicPredictor();
            var predictions =
                predictor.GetPredictionForFile("./Resources/about.wav", new Progress<int>(), 500);

            Assert.IsTrue(predictions.Contains("G"));
            Assert.IsTrue(predictions.Contains("Em"));
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionsForSample_CallsGetPredictionCorrectly()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            Assert.AreEqual(predictor.GetPrediction(samples, sampleRate), "D");
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionsForSample_CallsGetPredictionCorrectlyWithChord()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var chord = predictor.GetPredictionWithChord(samples, sampleRate);
            Assert.AreEqual(chord.Name, "D");

            var pcp = Chords.Profiling.Profiling.PitchClassProfileForSamples(samples, sampleRate);
            Assert.AreEqual(sampleRate, chord.SampleRate);

            Assert.AreEqual(pcp.Length, chord.Pcp.Length);
            for (var i = 0; i < pcp.Length; i++)
            {
                Assert.IsTrue(Math.Abs(pcp[i] - chord.Pcp[i]) < 1e-8);
            }

            Assert.AreEqual(samples.Length, chord.Samples.Length);
            for (var i = 0; i < samples.Length; i++)
            {
                Assert.IsTrue(Math.Abs(samples[i] - chord.Samples[i]) < 1e-8);
            }
        }


        [TestMethod]
        public void AutoMlPredictor_GetPredictionsForLongSample_CallsGetPredictionCorrectly()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/about.wav");
            var predictions =
                predictor.GetPredictions(samples, sampleRate, 500, new Progress<int>());

            Assert.IsTrue(predictions.Contains("G"));
            Assert.IsTrue(predictions.Contains("Em"));
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionsWithChordsForLongSample_CallsGetPredictionCorrectly()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/about.wav");
            var chords =
                predictor.GetPredictionsWithChords(samples, sampleRate, 500, new Progress<int>());

            var gChord = chords.FirstOrDefault(chord => chord.Name.Equals("G"));
            var emChord = chords.FirstOrDefault(chord => chord.Name.Equals("Em"));

            Assert.AreEqual(gChord.Name, "G");
            Assert.AreEqual(emChord.Name, "Em");

            bool areArraysEqual = true;
            for(int i = 0; i < Math.Min(gChord.Samples.Length, emChord.Samples.Length); i++)
            {
                if(Math.Abs(gChord.Samples[i] - emChord.Samples[i]) > 1e-8)
                {
                    areArraysEqual = false;
                }
            }

            Assert.IsFalse(areArraysEqual);
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionsForFile_CallsGetPredictionCorrectly()
        {
            var predictor = new AutoMlPredictor();
            var predictions =
                predictor.GetPredictionForFile("./Resources/about.wav", new Progress<int>(), 500);

            Assert.IsTrue(predictions.Contains("G"));
            Assert.IsTrue(predictions.Contains("Em"));
        }
    }
}
