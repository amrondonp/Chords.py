using ChordsWebAPI.Models;
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

        public PredictionsController(PredictionContext predictionContext)
        {
            _predictionContext = predictionContext;
        }

        [HttpGet]
        public async Task<List<Prediction>> GetAll()
        {
            return await _predictionContext.Predictions.ToListAsync();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<int> Create()
        {
            string fileName = "./tmp/example.wav";
            var prediction = new Prediction
            {
                AutoBorder = false,
                FilePath = fileName,
                WindowInMs = 500
            };

            var dbResult = await _predictionContext.Predictions.AddAsync(prediction);
            await _predictionContext.SaveChangesAsync();
            await dbResult.ReloadAsync();
            return dbResult.Entity.Id;
        }
    }
}
