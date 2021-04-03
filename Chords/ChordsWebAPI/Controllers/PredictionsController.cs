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
            predictor = new AutoMlPredictor();
        }

        [HttpGet]
        public async Task<List<Prediction>> GetAll()
        {
            return await _predictionContext.Predictions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Prediction> Get(int id)
        {
            return await _predictionContext.Predictions
                .Include(prediction => prediction.Chords)
                .SingleAsync(prediction => prediction.Id == id);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<int> Create()
        {
            string fileName = "C:\\Users\\anrondon\\Desktop\\Coding\\Chords.py\\final_project\\songs\\sweet improved.mp3";
            var prediction = new Prediction
            {
                AutoBorder = false,
                FilePath = fileName,
                WindowInMs = 500
            };


            var chordProcessingProgress = new Progress<int>((v) =>
            {
                prediction.Progress = v;
            });

            var (sampleRate, samples) = await Task.Run(() => Profiling.GetSamples(prediction.FilePath));

            var chordsPredicted = await Task.Run(() =>
                predictor.GetPredictionsWithChords(samples, sampleRate,
                    prediction.WindowInMs, chordProcessingProgress)
            );

            prediction.ModelName = "AutoMlPredictor";

            prediction.Chords = chordsPredicted.Select(chord => new ChordWithKey
            {
                Name = chord.Name,
                SampleLength = chord.Samples.Length,
                SampleRate = chord.SampleRate
            }).ToList();

            prediction.Chords.ToList().ForEach(chord =>
            {
                _predictionContext.Add(chord);
            });



            var dbResult = await _predictionContext.Predictions.AddAsync(prediction);
            await _predictionContext.SaveChangesAsync();
            await dbResult.ReloadAsync();

            return dbResult.Entity.Id;
        }

        private async Task RunPrediction(int predictionId)
        {
            var prediction = await _predictionContext.Predictions.FindAsync(predictionId);
            // var dbResult = await _predictionContext.Predictions.AddAsync(prediction);
            
            var chordProcessingProgress = new Progress<int>((v) =>
            {
                prediction.Progress = v;
                //await _predictionContext.SaveChangesAsync();
            });

            var (sampleRate, samples) = await Task.Run(() => Profiling.GetSamples(prediction.FilePath));

            var chordsPredicted = await Task.Run(() =>
                predictor.GetPredictionsWithChords(samples, sampleRate,
                    prediction.WindowInMs, chordProcessingProgress)
            );

            prediction.ModelName = "some-model";

            prediction.Chords = chordsPredicted.Select(chord => new ChordWithKey
            {
                Name = chord.Name,
                SampleLength = chord.Samples.Length,
                SampleRate = chord.SampleRate
            }).ToList();

            prediction.Chords.ToList().ForEach(chord =>
            {
                _predictionContext.Add(chord);
            });

            await _predictionContext.SaveChangesAsync();
        }
    }
}
