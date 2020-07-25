using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chords.Repositories;
using System.IO;

namespace ChordsTest.Repository
{   
   [TestClass]
    public class FileSystemTrainDataGeneratorTest
    {
        [TestMethod]
        public void EmptyFolder_GeneratesCsvTrainDataEmpty()
        {
            const string inputDirectory = "./Resources/trainDataGeneratorFolder/";
            const string outputCsvFile = inputDirectory + "trainDataGenerated.csv";

            Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 0);
            Assert.IsFalse(File.Exists(outputCsvFile));

            var trainDataGenerator = new FileSystemTrainDataGenerator(inputDirectory, outputCsvFile);
            
            try
            {
                trainDataGenerator.GenerateTrainData();

                Assert.AreEqual(Directory.GetFiles(inputDirectory, "*.json").Length, 0);
                Assert.IsTrue(File.Exists(outputCsvFile));

                using StreamReader sr = File.OpenText(outputCsvFile);
                string s = sr.ReadLine();

                Assert.AreEqual(s, "c,c#,d,d#,e,f,f#,g,g#,a,a#,b,chord");
            } finally
            {
                File.Delete(outputCsvFile);
            }
        }
    }
}
