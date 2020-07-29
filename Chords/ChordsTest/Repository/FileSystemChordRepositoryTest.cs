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
        public void SaveChord_Correctly()
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

        [TestMethod]
        public void LoadChord_Correctly()
        {
            const string directory = "./Resources/storedTestChordsLoadChord/";

            try
            {
                Directory.CreateDirectory(directory);
                IChordRepository repository = new FileSystemChordRepository(directory);

                var chord = ChordTest.ChordExample();

                var existingJsonFilesCount = Directory
                    .EnumerateFiles(directory, "*.json").Count();

                var chordId = repository.SaveChord(chord);
                var loadedChord = repository.LoadChord(chordId);

                Assert.AreEqual(chord.Name, loadedChord.Name);
                Assert.AreEqual(chord.SampleRate, loadedChord.SampleRate);
                Assert.AreEqual(chord.Samples.Length, loadedChord.Samples.Length);
                Assert.AreEqual(chord.Pcp.Length, loadedChord.Pcp.Length);

                for (var i = 0; i < chord.Samples.Length; i++)
                {
                    Assert.AreEqual(chord.Samples[i], loadedChord.Samples[i]);
                }

                for (var i = 0; i < chord.Pcp.Length; i++)
                {
                    Assert.AreEqual(chord.Pcp[i], loadedChord.Pcp[i]);
                }
            }
            finally
            {
                var jsonFiles =
                    Directory.EnumerateFiles(directory, "*.json").ToList();

                foreach(var jsonFile in jsonFiles)
                {
                    File.Delete(jsonFile);
                }

                Directory.Delete(directory);
            }
        }
    }
}
