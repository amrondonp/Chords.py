using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChordsTest.Profiling
{
    [TestClass]
    public class SamplesManagerTest
    {
        private void AssertArrayEqualOnOffset(float [] arr1, float [] arr2, int offsetOnArr2)
        {
            for (var i = 0; i < arr1.Length; i++)
            {
                Assert.AreEqual(arr1[i], arr2[offsetOnArr2 + i]);
            }
        }

        [TestMethod]
        public void MakesSampleChunks_HappyPath()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var windowInMs = 500;
            var expectedNewSize = sampleRate / 2;
            var position = 1;

            var (sampleRateReturned, samplesReturned) = samplesManager.GetSamplesAtPositionGivenWindowInMs(position, windowInMs);

            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, expectedNewSize);
            AssertArrayEqualOnOffset(samplesReturned, samples, position * expectedNewSize);
        }

        [TestMethod]
        public void MakesSampleChunks_NonDivisible()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            
            var windowInMs = 437;
            var expectedNewSize = sampleRate * windowInMs / 1000;
            var position = 4;
            
            var (sampleRateReturned, samplesReturned) = samplesManager.GetSamplesAtPositionGivenWindowInMs(position, windowInMs);

            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, expectedNewSize);
            AssertArrayEqualOnOffset(samplesReturned, samples, position * expectedNewSize);
        }

        [TestMethod]
        public void MakesSampleChunks_LastCompleteChunk()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var windowInMs = 617;
            var expectedNewSize = Chords.Profiling.LongAudioProfiling.GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);
            var numberOfCompleteChunks = samples.Length / expectedNewSize;
            var position = numberOfCompleteChunks - 1;

            var (sampleRateReturned, samplesReturned) = 
                samplesManager.GetSamplesAtPositionGivenWindowInMs(position, windowInMs);

            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, expectedNewSize);
            AssertArrayEqualOnOffset(samplesReturned, samples, position * expectedNewSize);
        }

        [TestMethod]
        public void MakesSampleChunks_LastIncompleteChunk()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var windowInMs = 617;
            var expectedNewSize = Chords.Profiling.LongAudioProfiling.GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);
            var numberOfCompleteChunks = samples.Length / expectedNewSize;
            var isThereAnIncompleteChunk = samples.Length % expectedNewSize != 0;
            var position = numberOfCompleteChunks;


            var (sampleRateReturned, samplesReturned) =
                samplesManager.GetSamplesAtPositionGivenWindowInMs(position, windowInMs);

            Assert.IsTrue(isThereAnIncompleteChunk);
            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, samples.Length % expectedNewSize);
            AssertArrayEqualOnOffset(samplesReturned, samples, position * expectedNewSize);
        }

        [TestMethod]
        public void MakesSampleChunks_WindowBiggerThanFile()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var windowInMs = 617000000;
            var expectedNewSize = Chords.Profiling.LongAudioProfiling.GetNumberOfSamplesGivenWindowInMs(sampleRate, windowInMs);

            var numberOfCompleteChunks = samples.Length / expectedNewSize;
            var isThereAnIncompleteChunk = samples.Length % expectedNewSize != 0;

            var (sampleRateReturned, samplesReturned) =
                samplesManager.GetSamplesAtPositionGivenWindowInMs(numberOfCompleteChunks, windowInMs);

            Assert.AreEqual(numberOfCompleteChunks, 0);
            Assert.IsTrue(isThereAnIncompleteChunk);
            Assert.AreEqual(sampleRateReturned, sampleRate);
            Assert.AreEqual(samplesReturned.Length, samples.Length);
            AssertArrayEqualOnOffset(samplesReturned, samples, 0);
        }

        [TestMethod]
        public void MakesSampleChunks_OutOfBoundsChunkThrowsAnError()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var exceptionThrown = false;

            try
            {
                samplesManager.GetSamplesAtPositionGivenWindowInMs(1000, 500);
            } catch(Exception)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        public void MakesSampleChunks_NegativeIndexChunkThrowsAnError()
        {
            var (sampleRate, samples) = Chords.Profiling.Profiling.GetSamples("./Resources/d.wav");
            var samplesManager = new Chords.Profiling.SamplesManager(sampleRate, samples);
            Assert.IsNotNull(samplesManager);

            var exceptionThrown = false;

            try
            {
                samplesManager.GetSamplesAtPositionGivenWindowInMs(-1, 500);
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
