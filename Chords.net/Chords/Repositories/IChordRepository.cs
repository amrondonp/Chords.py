using Chords.Entities;

namespace Chords.Repositories
{
    public interface IChordRepository
    {
        string SaveChord(Chord chord);
        Chord LoadChord(string fileName);
    }
}
