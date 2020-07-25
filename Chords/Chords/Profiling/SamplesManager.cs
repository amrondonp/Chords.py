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
            var expectedSampleSize = newSampleSize;

            var isLastChunkIncomplete = samples.Length % newSampleSize != 0;
            var isLastChunk = (position + 1) * newSampleSize > samples.Length;

            if(isLastChunk && isLastChunkIncomplete)
            {
                newSampleSize = samples.Length % newSampleSize;
            }

            var samplesReturned = new float[newSampleSize];
            for (var i = 0; i < newSampleSize; i++)
            {
                samplesReturned[i] = samples[position * expectedSampleSize + i];
            }

            return (sampleRate, samplesReturned);
        }
    }
}
