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
        public void GetFFT_ComputesTheFFTCorreclty_D()
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
        public void GetFFT_ComputesTheFFTCorreclty_Em()
        {
            var (_, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/em.wav");
            var fft = Chords.Profiling.Profiling.GetFFT(samples);
            Assert.IsNotNull(fft);
            Assert.AreEqual(fft.Length, 124928);
            Assert.IsTrue(CompareComplex(fft[0], 2.0939026f, 0.0f));
            Assert.IsTrue(CompareComplex(fft[1], -6.8944035f, -0.8140067f));
            Assert.IsTrue(CompareComplex(fft[1000], -2.2811902f, -2.0249183f));
            Assert.IsTrue(CompareComplex(fft[14000], 0.37053436f, -0.2087223f));
        }

        [TestMethod]
        public void GetPCP_ComputesThePCPCorreclty_D()
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
                Assert.IsTrue(CompareFloatWithPrecision(expected[i], actual[i], 1e-7));
            }
        }

        [TestMethod]
        public void GetPCP_ComputesThePCPCorreclty_Em()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/em.wav");
            var fft = Chords.Profiling.Profiling.GetFFT(samples);

            double[] expected = { 0.001714135727507671, 0.003264961867620893,
                0.004673452009535607, 0.004337914586398048,
                0.10095131218447927, 0.003493948587298034,
                0.007436551511743249, 0.4809614087324316,
                0.010889062598783337, 0.0016608709514508658,
                0.006601373019547074, 0.3740150082232044 };

            double[] actual = Chords.Profiling.Profiling.PitchClassProfile(fft, sampleRate);

            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(CompareFloatWithPrecision(expected[i], actual[i], 1e-7));
            }
        }

        [TestMethod]
        public void GetRawPrediction_PredictsCorrectly_D()
        {
            double[] expected = {5.8298269e-11, 7.8153551e-01, 2.2488731e-01, 1.6687775e-10, 4.1404841e-11,
                                 1.5814350e-10, 3.1175637e-07, 2.3691897e-09, 2.9101182e-07, 4.8335897e-07};

            float[] actual = Chords.Profiling.Profiling.GetRawPrediction("./Resources/d.wav");

            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(CompareFloatWithPrecision(expected[i], actual[i], 1e-1));
            }
        }

        [TestMethod]
        public void GetRawPrediction_PredictsCorrectly_Em()
        {
            double[] expected = {1.10043527e-06, 1.71935762e-14, 1.75733941e-11, 1.40256235e-11,
                                 9.49494004e-01, 9.13571330e-10, 2.07506955e-01, 2.02706462e-15,
                                 9.27643696e-16, 1.98795576e-08};

            float[] actual = Chords.Profiling.Profiling.GetRawPrediction("./Resources/em.wav");

            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.IsTrue(CompareFloatWithPrecision(expected[i], actual[i], 0.3));
            }
        }

        [TestMethod]
        public void GetPreditcion_PredictsCorrectly_D()
        {
            string prediction = Chords.Profiling.Profiling.GetPrediction("./Resources/d.wav");
            Assert.AreEqual(prediction, "D");
        }

        [TestMethod]
        public void GetPreditcion_PredictsCorrectly_Em()
        {
            string prediction = Chords.Profiling.Profiling.GetPrediction("./Resources/em.wav");
            Assert.AreEqual(prediction, "Em");
        }

        [TestMethod]
        public void GetPreditcion_PredictsCorrectly_C()
        {
            string prediction = Chords.Profiling.Profiling.GetPrediction("./Resources/c.wav");
            Assert.AreEqual(prediction, "C");
        }
    }
}
