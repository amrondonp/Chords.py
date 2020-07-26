using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chords.Repositories;
using System.IO;
using Chords.Entities;
using ChordsTest.Entities;
using System.Threading.Tasks;

namespace ChordsTest.Repository
{
    [TestClass]
    public class FileSystemTrainDataGeneratorTest
    {
        [TestMethod]
        public async Task EmptyFolder_GeneratesCsvTrainDataEmpty()
        {
            const string inputDirectory = "./Resources/trainDataGeneratorFolderEmpty/";
            const string outputCsvFile = inputDirectory + "trainDataGenerated.csv";
            
            try
            {
                Directory.CreateDirectory(inputDirectory);
                Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 0);
                Assert.IsFalse(File.Exists(outputCsvFile));

                var trainDataGenerator = new FileSystemTrainDataGenerator(inputDirectory, outputCsvFile);

                await trainDataGenerator.GenerateTrainData();

                Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 0);
                Assert.IsTrue(File.Exists(outputCsvFile));

                using StreamReader sr = File.OpenText(outputCsvFile);
                string s = sr.ReadLine();

                Assert.AreEqual(s, "c,c#,d,d#,e,f,f#,g,g#,a,a#,b,chord");
            }
            finally
            {
                File.Delete(outputCsvFile);
                Directory.Delete(inputDirectory);
            }
        }

        [TestMethod]
        public async Task WithChordsInFolder_GeneratesCsvTrainData()
        {
            const string inputDirectory = "./Resources/trainDataGeneratorFolder10Chords/";
            const string outputCsvFile = inputDirectory + "trainDataGenerated.csv";
           
            try
            {
                Directory.CreateDirectory(inputDirectory);
                Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 0);
                Assert.IsFalse(File.Exists(outputCsvFile));

                var respository = new FileSystemChordRepository(inputDirectory);

                var chords = new Chord[10];
                for (var i = 0; i < 10; i++)
                {
                    chords[i] = ChordTest.ChordExample();
                }

                foreach (Chord chord in chords)
                {
                    respository.SaveChord(chord);
                }

                Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 10);
                Assert.IsFalse(File.Exists(outputCsvFile));

                var trainDataGenerator = new FileSystemTrainDataGenerator(inputDirectory, outputCsvFile);
                await trainDataGenerator.GenerateTrainData();

                Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 10);
                Assert.IsTrue(File.Exists(outputCsvFile));

                string[] fileContent = File.ReadAllText(outputCsvFile).Split("\n");

                Assert.AreEqual(fileContent.Length, 12);

                var contentsRegistry = fileContent[4].Split(",");
                Assert.AreEqual(contentsRegistry.Length, 13);

                for(var i = 0; i < contentsRegistry.Length - 1; i++)
                {
                    var isDouble = double.TryParse(contentsRegistry[i], out double _);
                    Assert.IsTrue(isDouble);
                }

                Assert.AreEqual(contentsRegistry[12], chords[4].Name.ToLower());
                Assert.AreEqual(fileContent[0], "c,c#,d,d#,e,f,f#,g,g#,a,a#,b,chord");
            }
            finally
            {
                File.Delete(outputCsvFile);
                var filesToDelete = Directory.GetFiles(inputDirectory, "*.json");
                foreach (string fileToDelete in filesToDelete)
                {
                    File.Delete(fileToDelete);
                }
                Directory.Delete(inputDirectory);
            }
        }
    }
}
