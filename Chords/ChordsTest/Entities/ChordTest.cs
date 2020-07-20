using Chords.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChordsTest.Entities
{
    [TestClass]
    public class ChordTest
    {
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
