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

        public Chord LoadChord(string id)
        {
            var fileName = directory + id + ".json";
            var jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<Chord>(jsonString);
        }

        public string SaveChord(Chord chord)
        {
            var id = System.Guid.NewGuid();
            var jsonString = JsonSerializer.Serialize(chord);
            File.WriteAllText(directory + id + ".json", jsonString);

            return id.ToString();
        }
    }
}
