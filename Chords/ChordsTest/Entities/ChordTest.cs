using Chords.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChordsTest.Entities
{
    [TestClass]
    public class ChordTest
    {
        public static Chord ChordExample()
        {
            var sampleCount = 12340;
            var samples = new float[sampleCount];

            var random = new Random();

            for (var i = 0; i < sampleCount; i++) {
                samples[i] = (float)random.NextDouble();
            } 

            return new Chord(samples, 44100, "Em");
        }

        [TestMethod]
        public void Chord_IsCreated()
        {
            var samples = new[] { 1f, 2f, 3f };
            var chord = new Chord(samples, 44100, "Em");
            Assert.AreEqual(chord.Name, "Em");
            Assert.AreEqual(chord.SampleRate, 44100);
            Assert.AreEqual(chord.Samples, samples);
        }
    }
}
