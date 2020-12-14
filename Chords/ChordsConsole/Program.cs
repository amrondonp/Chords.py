using Chords.Predictors;
using System;

namespace ChordsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args.Length >= 1 ? args[0] : "./storedChords/big-in-japan.mp3";
            IPredictor predictor = new AutoMlPredictor();
            bool hasFinished = false;

            var chordProcessingProgress = new Progress<int>(v =>
            {
                Console.WriteLine($@"Computing chords... {v} %");
                if(v >= 100)
                {
                    hasFinished = true;
                }
            });

            var predictions = predictor.GetPredictionForFile(file, chordProcessingProgress, 500);

            while(!hasFinished) { }
            
            foreach(string prediction in predictions)
            {
                Console.Write(prediction + " ");
            }
        }
    }
}
