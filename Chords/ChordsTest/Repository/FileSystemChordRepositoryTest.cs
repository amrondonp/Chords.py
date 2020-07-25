using Chords.Repositories;
using ChordsTest.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ChordsTest.Repository
{
    [TestClass]
    public class FileSystemChordRepositoryTest
    {
        [TestMethod]
        public void SaveChord_LoadChord_Correctly()
        {
            const string directory = "./Resources/storedTestChordsSaveChord/";
            
            try
            {
                Directory.CreateDirectory(directory);
                IChordRepository repository = new FileSystemChordRepository(directory);

                var chord = ChordTest.ChordExample();

                var existingJsonFilesCount = Directory
                    .EnumerateFiles(directory, "*.json").Count();

                repository.SaveChord(chord);

                var jsonFiles =
                    Directory.EnumerateFiles(directory, "*.json").ToList();

                Assert.AreEqual(jsonFiles.Count(), existingJsonFilesCount + 1);

                var content = File.ReadAllText(jsonFiles.FirstOrDefault());

                Assert.AreEqual(content, JsonSerializer.Serialize(chord));

            } finally {
                var jsonFiles =
                    Directory.EnumerateFiles(directory, "*.json").ToList();

                File.Delete(jsonFiles.FirstOrDefault());
                Directory.Delete(directory);
            }
        }
    }
}
