using Chords.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebAPI.Entities
{
    public class Prediction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Progress { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int WindowInMs { get; set; }
        public bool AutoBorder { get; set; }
        public string ModelName { get; set; }
        public List<ChordWithKey> Chords { get; set; }
    }
}
