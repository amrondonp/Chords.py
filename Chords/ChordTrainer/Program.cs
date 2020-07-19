using Chords.MachineLearning;
using System;

namespace ChordTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            uint secondsToRun = 60;
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Console.WriteLine(@"Training Prediction model with AutoML");

            var (trainData, modelWithLabelMapping, _) =
                AutoMlModelCreation.CreateDataViewAndTransformer("./Resources/trainData.csv", secondsToRun);

            AutoMlModelCreation.MlContextInstance.Model.Save(
                modelWithLabelMapping, trainData.Schema,
                "./generatedModels/model" + currentTime + ".model");
        }
    }
}
