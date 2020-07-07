using System;

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
            var dummyProgress = new Progress<int>(_=>{});
            return GetPredictionWithProgressReport(filePath, dummyProgress, windowInMs);
        }

        public static string[] GetPredictionWithProgressReport(string filePath, IProgress<int> progress, int windowInMs = 500)
        {
            var (sampleRate, samples) = Profiling.GetSamples(filePath);
            var samplesPerWindow = GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);


            string[] predictions = new string[samples.Length / samplesPerWindow];
            int predictionIndex = 0;

            float[] samplesWindow = new float[samplesPerWindow];

            for (int i = 0; i + samplesPerWindow < samples.Length; i += samplesPerWindow)
            {
                for (int j = 0; j < samplesPerWindow; j++)
                {
                    samplesWindow[j] = samples[i + j];
                }
                progress.Report((int)(((i + samplesPerWindow) / (samples.Length + 0.0)) * 100));
                predictions[predictionIndex] = Profiling.GetPrediction(sampleRate, samplesWindow);
                predictionIndex++;
            }
            progress.Report(100);

            return predictions;
        }
    }
}
