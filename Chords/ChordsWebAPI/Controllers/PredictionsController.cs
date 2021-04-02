using Chords.Predictors;
using Chords.Profiling;
using ChordsWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionsController : Controller
    {
        private readonly PredictionContext _predictionContext;
        private readonly IPredictor predictor;

        public PredictionsController(PredictionContext predictionContext)
        {
            _predictionContext = predictionContext;
            predictor = new AutoMlPredictor("./models/model1595137632S120L0.004830031641869656.model");
        }

        [HttpGet]
        public async Task<List<Prediction>> GetAll()
        {
            return await _predictionContext.Predictions.ToListAsync();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<int> Create()
        {
            string fileName = "./tmp/en_algun_lugar.wav";
            var prediction = new Prediction
            {
                AutoBorder = false,
                FilePath = fileName,
                WindowInMs = 500
            };

            var dbResult = await _predictionContext.Predictions.AddAsync(prediction);
            await _predictionContext.SaveChangesAsync();
            await dbResult.ReloadAsync();
            _ = RunPrediction(prediction);
            return dbResult.Entity.Id;
        }

        private async Task RunPrediction(Prediction prediction)
        {
            var dbResult = await _predictionContext.Predictions.AddAsync(prediction);
            
            var chordProcessingProgress = new Progress<int>(async (v) =>
            {
                prediction.Progress = v;
                await _predictionContext.SaveChangesAsync();
            });

            var (sampleRate, samples) = await Task.Run(() => Profiling.GetSamples(prediction.FilePath));

            var chordsPredicted = await Task.Run(() =>
                predictor.GetPredictionsWithChords(samples, sampleRate,
                    prediction.WindowInMs, chordProcessingProgress)
            );

            prediction.Chords = chordsPredicted.Select(chord => new ChordWithKey
            {
                Name = chord.Name,
                SampleLength = chord.Samples.Length,
                SampleRate = chord.SampleRate
            }).ToArray();

            await _predictionContext.SaveChangesAsync();
        }
    }
}
