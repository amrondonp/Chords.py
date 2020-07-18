using Chords.MachineLearning;
using Microsoft.ML;
using System;

namespace Chords.Predictors
{
    public class AutoMlPredictor : IPredictor
    {
        private PredictionEngine<ChordData, ChordPredictionResult> engine;

        public AutoMlPredictor(string filePath = "./models/model.ml")
        {
            var predictionPipeline = AutoMlModelCreation
                .MlContextInstance.Model.Load(
                    filePath, out _);

            engine = AutoMlModelCreation.MlContextInstance.Model
                    .CreatePredictionEngine<ChordData, ChordPredictionResult>(
                        predictionPipeline);
        }

        public string GetPrediction(float[] sample, int sampleRate)
        {
            var pcp =
                Profiling.Profiling.PitchClassProfileForSamples(sample,
                    sampleRate);

            var chordData = AutoMlModelCreation.GetChordDataFromPcp(pcp);

            var prediction = engine.Predict(chordData).ChordPrediction;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(prediction.ToLower());
        }

        public string[] GetPredictions(float[] samples, int sampleRate,
            int windowInMs,
            IProgress<int> progress)
        {
            throw new NotImplementedException();
        }

        public string[] GetPredictionForFile(string filePath,
            IProgress<int> progress,
            int windowInMs)
        {
            throw new NotImplementedException();
        }
    }
}
