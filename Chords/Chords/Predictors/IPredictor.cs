using System;

namespace Chords.Predictors
{
    public interface IPredictor
    {
        public string GetPrediction(float[] sample, int sampleRate);

        public string[] GetPredictions(float[] samples,
            int sampleRate, int windowInMs, IProgress<int> progress);
    }
}
