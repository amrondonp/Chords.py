using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public void GenerateTrainData()
        {
            using StreamWriter sw = File.CreateText(outputCsvFile);
            sw.WriteLine(fileHeader());
        }

        private string fileHeader()
        {
            return "c,c#,d,d#,e,f,f#,g,g#,a,a#,b,chord";
        }
    }
}
