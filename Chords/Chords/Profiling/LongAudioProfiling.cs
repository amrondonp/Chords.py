using System;
using System.Collections.Generic;
using System.Text;

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
            var (sampleRate, samples) = Profiling.GetSamples(filePath);
            var samplesPerWindow = GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);
            
            
            string[] predictions = new string[samples.Length / samplesPerWindow];
            int predictionIndex = 0;

            float[] samplesWindow = new float[samplesPerWindow];

            for(int i = 0; i + samplesPerWindow < samples.Length; i+=samplesPerWindow )
            {
                for(int j = 0; j < samplesPerWindow; j++)
                {
                    samplesWindow[j] = samples[i + j];
                }
                predictions[predictionIndex] = Profiling.GetPrediction(sampleRate, samplesWindow);
                predictionIndex++;
            }

            return predictions;
        }
    }
}
