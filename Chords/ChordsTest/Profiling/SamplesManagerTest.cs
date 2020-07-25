using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChordsTest.Profiling
{
    [TestClass]
    public class SamplesManagerTest
    {
        [TestMethod]
        public void GetsTheChordsCorrectly_HappyPath()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var (sampleRateReturned, samplesReturned) = samplesManager.GetSamplesAtPositionGivenWindowInMs(1, 500);
            
            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, sampleRate / 2);
            
            for(var i = 0 ; i < samplesReturned.Length; i++)
            {
                Assert.AreEqual(samplesReturned[i], samples[sampleRate / 2 + i]);
            }
        }

        [TestMethod]
        public void GetsTheChordsCorrectly_NonDivisible()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var (sampleRateReturned, samplesReturned) = samplesManager.GetSamplesAtPositionGivenWindowInMs(4, 437);
            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, sampleRate * 437 / 1000 );
            for (var i = 0; i < samplesReturned.Length; i++)
            {
                Assert.AreEqual(samplesReturned[i], samples[4 * (sampleRate * 437 / 1000) + i]);
            }
        }

        [TestMethod]
        public void GetsTheChordsCorrectly_LastCompleteChunk()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var windowInMs = 617;
            var newWindowLength = Chords.Profiling.LongAudioProfiling.GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);

            var numberOfCompleteChunks = samples.Length / newWindowLength;

            var (sampleRateReturned, samplesReturned) = 
                samplesManager.GetSamplesAtPositionGivenWindowInMs(numberOfCompleteChunks - 1, windowInMs);

            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, sampleRate * windowInMs / 1000);

            for (var i = 0; i < samplesReturned.Length; i++)
            {
                Assert.AreEqual(samplesReturned[i], samples[(numberOfCompleteChunks - 1) * (sampleRate * windowInMs / 1000) + i]);
            }
        }

        [TestMethod]
        public void GetTheChordsCorrectly_LastIncompleteChunk()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var windowInMs = 617;
            var newWindowLength = Chords.Profiling.LongAudioProfiling.GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);

            var numberOfCompleteChunks = samples.Length / newWindowLength;
            var isThereAnIncompleteChunk = samples.Length % newWindowLength != 0;

            Assert.IsTrue(isThereAnIncompleteChunk);

            var (sampleRateReturned, samplesReturned) =
                samplesManager.GetSamplesAtPositionGivenWindowInMs(numberOfCompleteChunks, windowInMs);

            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, samples.Length % newWindowLength);
        }
    }
}
