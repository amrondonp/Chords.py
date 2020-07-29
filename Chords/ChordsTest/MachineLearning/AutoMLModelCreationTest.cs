using Chords.Entities;
using Chords.MachineLearning;
using Chords.Repositories;
using ChordsTest.Entities;
using Microsoft.ML;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace ChordsTest.MachineLearning
{
    [TestClass]
    public class AutoMlModelCreationTest
    {
        static readonly double[] emPcp =
        {
            0.001714135727507671, 0.003264961867620893,
            0.004673452009535607, 0.004337914586398048,
            0.10095131218447927, 0.003493948587298034,
            0.007436551511743249, 0.4809614087324316,
            0.010889062598783337, 0.0016608709514508658,
            0.006601373019547074, 0.3740150082232044
        };

        static readonly double[] dPcp =
        {
            0.003255314087377314,
            0.01810165496076202, 0.4737221312697051,
            0.006197408724571781, 0.044656804098667124,
            0.008874184476456886, 0.044524344194969326,
            0.012706518669088441, 0.03246405033858645,
            0.34075234557146383, 0.009979990771760218,
            0.00476525283659171
        };

        [TestMethod]
        public void GetModel_TrainsTheModelCorrectly()
        {
            var (experimentResult, predictionEngine) =
                AutoMlModelCreation.CreateModel("./Resources/trainData.csv", 1);

            Assert.IsNotNull(experimentResult);

            var metrics =
                AutoMlModelCreation.EvaluateModel(experimentResult, "./Resources/testData.csv");

            Assert.IsTrue(metrics.LogLoss < 0.5);

            var prediction =
                predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(emPcp));

            Assert.IsTrue(prediction.ChordPrediction.ToLower().Equals("em"));

            var dPrediction =
                predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(dPcp));

            Assert.IsTrue(dPrediction.ChordPrediction.ToLower().Equals("d"));
        }

        [TestMethod]
        public void GetModelWithDataView()
        {
            var textLoader = AutoMlModelCreation.MlContextInstance
                .Data.CreateTextLoader<ChordData>(separatorChar: ',', hasHeader: true);

            var trainData = textLoader.Load("./Resources/trainData.csv", "./Resources/testData.csv");

            var (experimentResult, predictionEngine) =
                AutoMlModelCreation.CreateModelGivenDataView(trainData, 1);

            Assert.IsNotNull(experimentResult);

            var metrics =
                AutoMlModelCreation.EvaluateModel(experimentResult, "./Resources/testData.csv");

            Assert.IsTrue(metrics.LogLoss < 0.5);

            var prediction =
                predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(emPcp));

            Assert.IsTrue(prediction.ChordPrediction.ToLower().Equals("em"));

            var dPrediction =
                predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(dPcp));

            Assert.IsTrue(dPrediction.ChordPrediction.ToLower().Equals("d"));
        }

        [TestMethod]
        public async Task CreateModelGivenInitialDataAndStoredChordsFolder()
        {
            const string trainDataFile = "./Resources/trainData.csv";
            const string inputDirectory = "./Resources/trainDataGeneratorFolderTraining/";
            const uint timeoutInSeconds = 1;
            const string outputDirectory = "./Resources/generatedModels/";

            try
            {
                Directory.CreateDirectory(inputDirectory);
                Directory.CreateDirectory(outputDirectory);
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

                var (experimentResult, predictionEngine) =
                await AutoMlModelCreation.CreateModelGivenInitialDataAndStoredChordsFolder(trainDataFile, inputDirectory, timeoutInSeconds, outputDirectory);

                Assert.IsNotNull(experimentResult);

                var metrics =
                    AutoMlModelCreation.EvaluateModel(experimentResult, "./Resources/testData.csv");

                Assert.IsTrue(metrics.LogLoss < 0.5);

                var prediction =
                    predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(emPcp));

                Assert.IsTrue(prediction.ChordPrediction.ToLower().Equals("em"));

                var dPrediction =
                    predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(dPcp));

                Assert.IsTrue(dPrediction.ChordPrediction.ToLower().Equals("d"));
            } finally
            {
                File.Delete(Path.Combine(inputDirectory, "trainData.csv"));
                var filesToDelete = Directory.GetFiles(inputDirectory, "*.json");
                foreach (string fileToDelete in filesToDelete)
                {
                    File.Delete(fileToDelete);
                }
                Directory.Delete(inputDirectory);

                var modelsToDelete = Directory.GetFiles(outputDirectory, "*.model");
                foreach (string modelToDelete in modelsToDelete)
                {
                    File.Delete(modelToDelete);
                }
                Directory.Delete(outputDirectory);
            }
        }
    }
}
