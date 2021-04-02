using Chords.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebAPI.Models
{
    public class ChordWithKey: Chord
    {
        [Key]
        public int Id { get; set; }
    }
}
