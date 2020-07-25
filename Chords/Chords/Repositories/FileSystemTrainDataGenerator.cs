using Chords.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chords.Repositories
{
    public class FileSystemTrainDataGenerator
    {
        private readonly string inputDirectory;
        private readonly string outputCsvFile;

        public FileSystemTrainDataGenerator(string inputDirectory, string outputCsvFile)
        {
            this.inputDirectory = inputDirectory;
            this.outputCsvFile = outputCsvFile; 
        }

        public async Task GenerateTrainData()
        {
            var chordFiles = Directory.GetFiles(inputDirectory, "*.json");
            using StreamWriter sw = File.CreateText(outputCsvFile);
            sw.Write(fileHeader());
            sw.Write("\n");

            foreach(string chordFile in chordFiles)
            {
                var chordJson = File.ReadAllText(chordFile);
                var chord = JsonSerializer.Deserialize<Chord>(chordJson);
                var pcp = await Task.Run(() => Profiling.Profiling.PitchClassProfileForSamples(chord.Samples, chord.SampleRate));
                foreach(double component in pcp)
                {
                    sw.Write(component);
                    sw.Write(",");
                }
                sw.Write(chord.Name);
                sw.Write("\n");
            }

        }

        private string fileHeader()
        {
            return "c,c#,d,d#,e,f,f#,g,g#,a,a#,b,chord";
        }
    }
}
