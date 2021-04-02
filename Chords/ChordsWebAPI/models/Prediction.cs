using Chords.Entities;
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
        public string FilePath { get; set; }
        public int WindowInMs { get; set; }
        public bool AutoBorder { get; set; }
        public string ModelName { get; set; }
        public ChordWithKey[] Chords { get; set; }
    }
}
