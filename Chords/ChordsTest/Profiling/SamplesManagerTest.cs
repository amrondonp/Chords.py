using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChordsTest.Profiling
{
    [TestClass]
    public class SamplesManagerTest
    {
        [TestMethod]
        public void GetsTheChordsCorrectly()
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
    }
}
