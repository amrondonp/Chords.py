using Chords.Entities;
using Chords.Predictors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

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

        [TestMethod]
        public void ClassicPredictor_GetPredictionWithBorderDetection()
        {
            var predictor = new ClassicPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/wind_of_change.wav");
            var chords = predictor.GetPredictionWithBorderDetection(samples, sampleRate, 500, 100, new Progress<int>());
            var totalSampleLength = chords.Select(chord => chord.Samples.Length).Aggregate((acc, val) => acc + val);
            Assert.AreEqual(samples.Length, totalSampleLength);
            Assert.AreEqual(chords[0].Name, "F");
            Assert.AreEqual(chords[1].Name, "Dm");
            Assert.AreEqual(chords[2].Name, "F");
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionWithBorderDetection()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/wind_of_change.wav");
            var chords = predictor.GetPredictionWithBorderDetection(samples, sampleRate, 500, 100, new Progress<int>());
            var totalSampleLength = chords.Select(chord => chord.Samples.Length).Aggregate((acc, val) => acc + val);
            Assert.AreEqual(samples.Length, totalSampleLength);
            Assert.AreEqual(chords.Length, 9);
            Assert.AreEqual(chords[0].Name, "F");
            Assert.AreEqual(chords[1].Name, "Dm");
            Assert.AreEqual(chords[2].Name, "F");
            Assert.AreEqual(chords[3].Name, "Dm");
            Assert.AreEqual(chords[4].Name, "Am");
            Assert.AreEqual(chords[5].Name, "Dm");
            Assert.AreEqual(chords[6].Name, "Am");
            Assert.AreEqual(chords[7].Name, "G");
            Assert.AreEqual(chords[8].Name, "Em");
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionWithBorderDetection_Combined()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/combined.wav");
            var chords = predictor.GetPredictionWithBorderDetection(samples, sampleRate, 500, 100, new Progress<int>());
            var totalSampleLength = chords.Select(chord => chord.Samples.Length).Aggregate((acc, val) => acc + val);
            Assert.AreEqual(samples.Length, totalSampleLength);
            Assert.AreEqual(chords.Length, 2);
            Assert.AreEqual(chords[0].Name, "C");
            Assert.AreEqual(chords[1].Name, "G");
        }

        [Ignore]
        [TestMethod]
        public void AutoMlPredictor_GetPredictionWithBorderDetection_Sweet()
        {
            var predictor = new AutoMlPredictor("./Resources/20200730032502S400L0.175406C0.043894.model");
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/sweet improved.mp3");
            var chords = predictor.GetPredictionWithBorderDetection(samples, sampleRate, 500, 250, new Progress<int>());
            var a = String.Join(" ", chords.Select(chord => chord.Name));
            Assert.IsTrue(false); // Test not fully implemented yet
        }
    }
}
