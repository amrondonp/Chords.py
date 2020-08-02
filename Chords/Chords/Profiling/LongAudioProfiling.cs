using Chords.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chords.Profiling
{
    public class LongAudioProfiling
    {
        public static int GetNumberOfSamplesGivenWindowInMs(int sampleRate, int windowInMs)
        {
            return (windowInMs * sampleRate) / 1000;
        }

        public static string[] GetPrediction(string filePath, int windowInMs = 500)
        {
            var dummyProgress = new Progress<int>(_ => { });
            return GetPredictionWithProgressReport(filePath, dummyProgress, windowInMs);
        }

        public static string[] GetPredictionWithProgressReport(string filePath,
            IProgress<int> progress, int windowInMs = 500)
        {
            var (sampleRate, samples) = Profiling.GetSamples(filePath);
            return PredictionWithProgressReport(sampleRate, samples, windowInMs, progress);
        }

        public static string[] PredictionWithProgressReport(int sampleRate, float[] samples, int windowInMs, IProgress<int> progress)
        {
            return PredictionWithProgressReportAndCustomPrediction(sampleRate, samples,
                windowInMs, progress, Profiling.GetPrediction);
        }

        public static string[] PredictionWithProgressReportAndCustomPrediction(
            int sampleRate, float[] samples, int windowInMs,
            IProgress<int> progress,
            Func<int, float[], string> predictionFunction)
        {
            var samplesPerWindow =
                GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);


            var predictions = new string[samples.Length / samplesPerWindow];
            var predictionIndex = 0;

            var samplesWindow = new float[samplesPerWindow];

            for (var i = 0;
                i + samplesPerWindow < samples.Length;
                i += samplesPerWindow)
            {
                for (var j = 0; j < samplesPerWindow; j++)
                {
                    samplesWindow[j] = samples[i + j];
                }

                progress.Report(
                    (int)(((i + samplesPerWindow) / (samples.Length + 0.0)) * 100));
                predictions[predictionIndex] =
                    predictionFunction(sampleRate, samplesWindow);
                predictionIndex++;
            }

            progress.Report(100);

            return predictions;
        }

        public static Chord[] PredictionWithProgressReportAndCustomPredictionWithChords(
            int sampleRate, float[] samples, int windowInMs,
            IProgress<int> progress,
            Func<int, float[], Chord> predictionFunction)
        {
            var samplesPerWindow =
                GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);


            var predictions = new Chord[samples.Length / samplesPerWindow];
            var predictionIndex = 0;


            for (var i = 0;
                i + samplesPerWindow < samples.Length;
                i += samplesPerWindow)
            {
                var samplesWindow = new float[samplesPerWindow];
                for (var j = 0; j < samplesPerWindow; j++)
                {
                    samplesWindow[j] = samples[i + j];
                }

                progress.Report(
                    (int)(((i + samplesPerWindow) / (samples.Length + 0.0)) * 100));
                predictions[predictionIndex] =
                    predictionFunction(sampleRate, samplesWindow);
                predictionIndex++;
            }

            progress.Report(100);

            return predictions;
        }
        
        class Interval
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

        public static Chord[] PredictionWithBorderDetection(
            int sampleRate, float[] samples, int windowSizeInMs, int offsetInMs,
            IProgress<int> progress,
            Func<int, float[], Chord> predictionFunction)
        {
            int windowSize = (int)Math.Floor((0.0 + windowSizeInMs * sampleRate) / 1000);
            int offsetSize = (int)Math.Floor((0.0 + offsetInMs * sampleRate) / 1000);

            int intervalStart = 0, intervalEnd;
            float[] window = new float[windowSize];
            var intervals = new List<Interval>();


            do
            {
                progress.Report( (intervalStart*50) / samples.Length);
                intervalEnd = Math.Min(intervalStart + windowSize, samples.Length);

                Array.Copy(samples, intervalStart, window, 0, intervalEnd - intervalStart);
                var chord = predictionFunction(sampleRate, window);

                Interval interval = new Interval(intervalStart, intervalEnd, chord.Name);
                int intervalExtension = 0;

                while (chord.Name == interval.name && intervalStart + intervalExtension + windowSize + offsetSize < samples.Length)
                {
                    intervalExtension += offsetSize;
                    Array.Copy(samples, intervalStart + intervalExtension, window, 0, windowSize);
                    chord = predictionFunction(sampleRate, window);
                }

                interval.to += intervalExtension;
                intervals.Add(interval);
                intervalStart = interval.to;
            } while (intervalStart < samples.Length);

            List<Chord> actual = new List<Chord>();

            int a = 0;
            int b = 0;

            while (a < intervals.Count())
            {
                if (b == intervals.Count() || intervals[b].name != intervals[a].name)
                {
                    int rightEnd = b < intervals.Count() ? intervals[b].from : samples.Length;
                    progress.Report(50 + (rightEnd * 50) / samples.Length);
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

            progress.Report(100);

            return actual.ToArray();
        }
    }
}
