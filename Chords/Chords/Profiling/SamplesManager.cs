namespace Chords.Profiling
{
    public class SamplesManager
    {
        private readonly int sampleRate;
        private readonly float[] samples;

        public SamplesManager(int sampleRate, float[] samples)
        {
            this.sampleRate = sampleRate;
            this.samples = samples;
        }

        public (int, float[]) GetSamplesAtPositionGivenWindowInMs(int position, int windowInMs)
        {
            var newSampleSize = LongAudioProfiling.GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);
            var samplesReturned = new float[newSampleSize];

            for (var i = 0; i < newSampleSize; i++)
            {
                samplesReturned[i] = samples[position * newSampleSize + i];
            }

            return (sampleRate, samplesReturned);
        }
    }
}
