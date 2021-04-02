﻿using Chords.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebAPI.Models
{
    public class Prediction
    {
        [Key]
        public int Id { get; set; }
        public int Progress { get; set; }
        public ChordWithKey[] Chords { get; set; }
    }
}
