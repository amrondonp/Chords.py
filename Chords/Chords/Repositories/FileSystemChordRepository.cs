using Chords.Entities;
using System.IO;
using System.Text.Json;

namespace Chords.Repositories
{
    public class FileSystemChordRepository : IChordRepository
    {
        private readonly string directory;

        public FileSystemChordRepository(string directory)
        {
            this.directory = directory;
        }

        public void LoadChord(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public void SaveChord(Chord chord)
        {
            var jsonString = JsonSerializer.Serialize(chord);
            File.WriteAllText(directory + System.Guid.NewGuid() + ".json", jsonString);
        }
    }
}
