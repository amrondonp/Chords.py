using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChordsTest.Profiling
{
    [TestClass]
    public class ProfilingTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tuple = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            Assert.IsNotNull(tuple);
            Assert.AreEqual(tuple.sampleRate, 44100);
            Assert.AreEqual(tuple.samples.Length, 262144);
        }
    }
}
