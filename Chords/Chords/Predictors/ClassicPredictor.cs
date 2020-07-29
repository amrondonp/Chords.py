using Chords.Entities;
using System;

namespace Chords.Predictors
{
    public class ClassicPredictor : IPredictor
    {
        public string GetPrediction(float[] sample, int sampleRate)
        {
            return Profiling.Profiling.GetPrediction(sampleRate, sample);
        }

        public string[] GetPredictions(float[] samples, int sampleRate,
            int windowInMs, IProgress<int> progress)
        {
            return Profiling.LongAudioProfiling
                .PredictionWithProgressReport(sampleRate, samples, windowInMs,
                    progress);
        }

        public string[] GetPredictionForFile(
            string filePath,
            IProgress<int> progress, int windowInMs)
        {
            return Profiling.LongAudioProfiling.GetPredictionWithProgressReport(
                filePath, progress, windowInMs);
        }

        public Chord GetPredictionWithChord(float[] sample, int sampleRate)
        {
            return Profiling.Profiling.GetPredictionWithChord(sampleRate, sample);
        }

        public Chord[] GetPredictionsWithChords(float[] samples, int sampleRate, int windowInMs, IProgress<int> progress)
        {
            return Profiling.LongAudioProfiling.PredictionWithProgressReportAndCustomPredictionWithChords
                (sampleRate, samples, windowInMs, progress, (sampleRateLambda, samplesLambda) =>
                        GetPredictionWithChord(samplesLambda, sampleRateLambda));
        }
    }
}
