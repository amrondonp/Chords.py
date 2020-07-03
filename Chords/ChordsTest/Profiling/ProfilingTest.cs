using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Numerics;


namespace ChordsTest.Profiling
{
    [TestClass]
    public class ProfilingTest
    {
        static bool CompareFloat(double a, double b)
        {
            return CompareFloatWithPrecision(a, b, 1e-5);
        }

        static bool CompareFloatWithPrecision(double a, double b, double precision)
        {
            return Math.Abs(a - b) < precision;
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

        [TestMethod]
        public void GetPCP_ComputesThePCPCorreclty()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var fft = Chords.Profiling.Profiling.GetFFT(samples);

            double[] expected = { 0.003255314087377314,
                0.01810165496076202, 0.4737221312697051,
                0.006197408724571781, 0.044656804098667124,
                0.008874184476456886, 0.044524344194969326,
                0.012706518669088441, 0.03246405033858645,
                0.34075234557146383, 0.009979990771760218,
                0.00476525283659171 };

            double [] actual = Chords.Profiling.Profiling.PitchClassProfile(fft, sampleRate);

            Assert.AreEqual(expected.Length, actual.Length);
            for(int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(CompareFloatWithPrecision(expected[i], actual[i], 0.07));
            }
        }

        [TestMethod]
        public void GetPrediction_PredictsCorrectly()
        {
            double[] expected = {5.8298269e-11, 7.8153551e-01, 2.2488731e-01, 1.6687775e-10, 4.1404841e-11,
                                 1.5814350e-10, 3.1175637e-07, 2.3691897e-09, 2.9101182e-07, 4.8335897e-07};

            float[] actual = Chords.Profiling.Profiling.GetRawPrediction("./Resources/d.wav");

            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(CompareFloatWithPrecision(expected[i], actual[i], 0.07));
            }
        }
    }
}
