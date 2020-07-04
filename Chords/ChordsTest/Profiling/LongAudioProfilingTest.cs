using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ChordsTest.Profiling
{
    [TestClass]
    public class LongAudioProfilingTest
    {
        [TestMethod]
        public void GetNumberOfSamplesGivenWindowInMs_GetsTheSampleSizeCorrectly()
        {
            Assert.AreEqual(
                Chords.Profiling.LongAudioProfiling
                    .GetNumberOfSamplesGivenWindowInMs(44100, 1000),
                44100
            );

            Assert.AreEqual(
                Chords.Profiling.LongAudioProfiling
                    .GetNumberOfSamplesGivenWindowInMs(44100, 500),
                22050
            );

            Assert.AreEqual(
                Chords.Profiling.LongAudioProfiling
                    .GetNumberOfSamplesGivenWindowInMs(44100, 200),
                8820
            );

            Assert.AreEqual(
               Chords.Profiling.LongAudioProfiling
                   .GetNumberOfSamplesGivenWindowInMs(44100, 333),
               14685
           );
        }
    }
}
