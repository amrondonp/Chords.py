using Chords.Entities;
using Chords.Predictors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace ChordsTest.Predictors
{
    public class Interval
    {
        public int from, to;
        public string name;

        public Interval(int from, int to, string name)
        {
            this.from = from;
            this.to = to;
            this.name = name;
        }
    }

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

        private Chord[] GetPredictionWithBorderDetection(float[] samples, int sampleRate, IPredictor predictor)
        {
            int windowSizeInMs = 500;
            int offsetInMs = 100;

            int windowSize = (int)Math.Floor((0.0 + windowSizeInMs * sampleRate) / 1000);
            int offsetSize = (int)Math.Floor((0.0 + offsetInMs * sampleRate) / 1000);

            int intervalStart = 0, intervalEnd;
            float[] window = new float[windowSize];
            var intervals = new List<Interval>();

            do
            {
                intervalEnd = Math.Min(intervalStart + windowSize, samples.Length);

                Array.Copy(samples, intervalStart, window, 0, intervalEnd - intervalStart);
                var chord = predictor.GetPredictionWithChord(window, sampleRate);

                Interval interval = new Interval(intervalStart, intervalEnd, chord.Name);
                int intervalExtension = 0;

                while (chord.Name == interval.name && intervalStart + intervalExtension + windowSize < samples.Length)
                {
                    intervalExtension += offsetSize;
                    Array.Copy(samples, intervalStart + intervalExtension, window, 0, windowSize);
                    chord = predictor.GetPredictionWithChord(window, sampleRate);
                }

                interval.to += intervalExtension;
                intervals.Add(interval);
                intervalStart = interval.to;
            } while (intervalStart < samples.Length);

            List <Chord> actual = new List<Chord>();

            int a = 0;
            int b = 0;

            while(a < intervals.Count())
            {
                if (b == intervals.Count() || intervals[b].name != intervals[a].name)
                {
                    int rightEnd = b < intervals.Count() ? intervals[b].from : samples.Length;
                    float[] intervalSamples = new float[rightEnd - intervals[a].from];
                    Array.Copy(samples, intervals[a].from, intervalSamples, 0, intervalSamples.Length);
                    var pcp = Chords.Profiling.Profiling.PitchClassProfileForSamples(intervalSamples, sampleRate);
                    actual.Add(new Chord(intervalSamples, sampleRate, intervals[a].name, pcp));
                    a = b;
                }
                else
                {
                    b++;
                }
            }

            return actual.ToArray();
        }

        [TestMethod]
        public void AutoMlPredictor_GetPredictionWithBorderDetection()
        {
            var predictor = new AutoMlPredictor();
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/wind_of_change.wav");
            var chords = GetPredictionWithBorderDetection(samples, sampleRate, predictor);
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
    }
}
