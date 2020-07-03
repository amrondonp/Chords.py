using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chords.NewFolder
{
    class Profiling
    {
        public static float [] GetPitchClassProfiling(string pathToAudioFile)
        {
            using AudioFileReader reader = new AudioFileReader(pathToAudioFile);
            float[] samples = new float[reader.Length];
            reader.Read(samples, 0, (int)reader.Length);
            return samples;
        }
    }
}
