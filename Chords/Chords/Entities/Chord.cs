namespace Chords.Entities
{
    public class Chord
    {
        public float[] Samples { get; }
        public int SampleRate { get; }
        public string Name { get; }

        public Chord() { }
        public Chord(float[] samples, int sampleRate, string name)
        {
            Name = name;
            Samples = samples;
            SampleRate = sampleRate;
        }
    }
}
