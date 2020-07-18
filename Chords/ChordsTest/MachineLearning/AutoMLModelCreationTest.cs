using Chords.MachineLearning;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChordsTest.MachineLearning
{
    [TestClass]
    public class AutoMlModelCreationTest
    {
        [TestMethod]
        public void GetModel_TrainsTheModelCorrectly()
        {
            var (experimentResult, predictionEngine) =
                AutoMlModelCreation.CreateModel("./Resources/trainData.csv", 1);
            Assert.IsNotNull(experimentResult);

            var metrics =
                AutoMlModelCreation.EvaluateModel(experimentResult, "./Resources/testData.csv");
            Assert.IsTrue(metrics.LogLoss < 0.5);

            double[] emPcp =
            {
                0.001714135727507671, 0.003264961867620893,
                0.004673452009535607, 0.004337914586398048,
                0.10095131218447927, 0.003493948587298034,
                0.007436551511743249, 0.4809614087324316,
                0.010889062598783337, 0.0016608709514508658,
                0.006601373019547074, 0.3740150082232044
            };

            var prediction =
                predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(emPcp));
            Assert.IsTrue(prediction.ChordPrediction.ToLower().Equals("em"));

            double[] dPcp =
            {
                0.003255314087377314,
                0.01810165496076202, 0.4737221312697051,
                0.006197408724571781, 0.044656804098667124,
                0.008874184476456886, 0.044524344194969326,
                0.012706518669088441, 0.03246405033858645,
                0.34075234557146383, 0.009979990771760218,
                0.00476525283659171
            };

            var dPrediction =
                predictionEngine.Predict(AutoMlModelCreation.GetChordDataFromPcp(dPcp));
            Assert.IsTrue(dPrediction.ChordPrediction.ToLower().Equals("d"));
        }
    }
}
