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
    }
}
