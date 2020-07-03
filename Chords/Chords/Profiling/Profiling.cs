using NAudio.Wave;
using System;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace Chords.Profiling
{
    public class Profiling
    {
        public static (int sampleRate, float[] samples) GetSamples(string pathToAudioFile)
        {
            using AudioFileReader reader = new AudioFileReader(pathToAudioFile);

            var sampleProvider = reader.ToSampleProvider();
            float[] samples = new float[reader.Length / sizeof(float)];
            var samplesRead = sampleProvider.Read(samples, 0, samples.Length);

            if (samples.Length != samplesRead)
            {
                throw new Exception("Error when reading the samples, samples.Length=" + samples.Length + " samplesRead=" + samplesRead);
            }

            return (reader.WaveFormat.SampleRate, samples);
        }

        public static Complex[] GetFFT(float[] samples)
        {
            Complex[] complexData = new Complex[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                complexData[i] = new Complex(samples[i], 0);
            }

            Fourier.Forward(complexData, FourierOptions.Matlab);
            return complexData;
        }
    }
}
