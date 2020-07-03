using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;


namespace ChordsTest.Profiling
{
    [TestClass]
    public class ProfilingTest
    {
        static bool CompareFloat(float a, float b)
        {
            return Math.Abs(a - b) < 1e-5;
        }

        static bool CompareComplex(Complex complex, float x, float y)
        {
            return CompareFloat((float)complex.Real, x) && CompareFloat((float)complex.Imaginary, y);
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

        [TestMethod]
        public void GetFFT_ComputesTheFFTCorreclty()
        {
            var (_, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var fft = Chords.Profiling.Profiling.GetFFT(samples);
            Assert.IsNotNull(fft);
            Assert.AreEqual(fft.Length, 262144);
            Assert.IsTrue(CompareComplex(fft[0], -12.335663f, 0.0f));
            Assert.IsTrue(CompareComplex(fft[1], -12.377926f, 2.8002753f));
            Assert.IsTrue(CompareComplex(fft[1000], -43.04428f, 24.787685f));
            Assert.IsTrue(CompareComplex(fft[14000], 3.0144866f, -1.557055f));
        }
    }
}
