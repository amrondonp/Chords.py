namespace Chords.Entities
{
    public class Chord
    {
        public float[] Samples { get; set;  }
        public int SampleRate { get; set; }
        public string Name { get; set; }
        public double[] Pcp { get; set; }

        public Chord() { }

        public Chord(float[] samples, int sampleRate, string name, double [] pcp)
        {
            Name = name;
            Samples = samples;
            SampleRate = sampleRate;
            Pcp = pcp;
        }

        public int DurationInMs()
        {
            return Samples.Length * 1000 / SampleRate;
        }
    }
}
