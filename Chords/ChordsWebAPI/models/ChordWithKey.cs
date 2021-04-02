using Chords.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebAPI.Models
{
    public class ChordWithKey
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SampleLength { get; set; }
        public int SampleRate { get; set; }
    }
}
