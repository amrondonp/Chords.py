using ChordsWeb.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWeb.Entities
{
    public class PredictionContext: DbContext
    {
        public PredictionContext(DbContextOptions<PredictionContext> options) : base(options) { }

        public DbSet<Prediction> Predictions { get; set; }
    }
}
