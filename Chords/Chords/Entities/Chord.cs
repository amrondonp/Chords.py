namespace Chords.Entities
{
    public class Chord
    {
        public float[] Samples { get; set;  }
        public int SampleRate { get; set; }
        public string Name { get; set; }

        public Chord() { }

        public Chord(float[] samples, int sampleRate, string name)
        {
            Name = name;
            Samples = samples;
            SampleRate = sampleRate;
        }
    }
}
