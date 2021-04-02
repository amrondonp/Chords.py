using Chords.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebAPI.models
{
    public class Prediction
    {
        public int Id { get; set; }
        public int Progress { get; set; }
        public Chord[] Chords { get; set; }
    }
}
