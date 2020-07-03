using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChordsTest.Profiling
{
    [TestClass]
    public class ProfilingTest
    {
        static bool CompareFloat(float a, float b)
        {
            return Math.Abs(a - b) < 1e-5;
        }

        [TestMethod]
        public void GetSamples_ReadsTheSamplesCorretly()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            Assert.IsNotNull(samples);
            Assert.AreEqual(sampleRate, 44100);
            Assert.AreEqual(samples.Length, 262144);
            Assert.IsTrue(CompareFloat(samples[0], -0.00088501f));
            Assert.IsTrue(CompareFloat(samples[1], -0.00082397f));
            Assert.IsTrue(CompareFloat(samples[^1], -0.00204468f));
        }
    }
}
