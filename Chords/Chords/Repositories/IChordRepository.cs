using Chords.Entities;

namespace Chords.Repositories
{
    public interface IChordRepository
    {
        void SaveChord(Chord chord);
        void LoadChord(string fileName);
    }
}
