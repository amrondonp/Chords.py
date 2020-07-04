using NAudio.Wave;
using System;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using System.Collections.Generic;
using Microsoft.ML.OnnxRuntime.Tensors;

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

        public static double[] PitchClassProfile(Complex[] X, int frecuency)
        {
            double fs = frecuency;
            double fref = 130.81;

            double N = X.Length;

            Func<int, double> M = (int l) =>
            {
                if (l == 0)
                {
                    return -1;
                }
                return Math.Round(12 * Math.Log2(   (fs * l)/(N * fref)  ) ) % 12;
            };

            double [] pcp = new double[12];
            for(int p = 0; p < 12; p++)
            {
                for(int l = 0; l < N/2; l++)
                {
                    if(p == M(l))
                    {
                        double mag = Math.Round(X[l].Magnitude, 6);
                        double sq = Math.Pow(mag, 2);
                        pcp[p] += sq;
                    }
                }
            }

            // Normalize pcp
            double [] pcp_norm = new double[12];
            double pcpSum = pcp.Sum();
            for(int p = 0; p < 12; p++)
            {
                pcp_norm[p] = (pcp[p] / pcpSum);
            }

            return pcp_norm;
        }

        public static float[] GetRawPrediction(string pathToAudioFile)
        {
            var (sampleRate, samples) = GetSamples(pathToAudioFile);
            var fft = GetFFT(samples);
            var pcp = PitchClassProfile(fft, sampleRate);

            var inputTensor = new DenseTensor<float>(new[] { 1, 12 });
            for(int i = 0; i < 12; i++)
            {
                inputTensor[0, i] = (float)pcp[i];
            }

            var input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("dense_1_input", inputTensor) };
            var session = new InferenceSession("models/binary_crossentropy.onnx");

            using var results = session.Run(input);
            return results.First().AsEnumerable<float>().ToArray();
        }

        public static string GetPrediction(string pathToAudioFile)
        {
            float[] rawPrediction = GetRawPrediction(pathToAudioFile);
            int maxProbabilityIndex = 0;
            float maxProbabilty = 0;

            for(int i = 0; i < rawPrediction.Length; i++)
            {
                if(maxProbabilty < rawPrediction[i])
                {
                    maxProbabilty = rawPrediction[i];
                    maxProbabilityIndex = i;
                }
            }

            string[] chordsTable = { "C", "D", "Dm", "E", "Em", "F", "G", "A", "Am", "Bm" };
            return chordsTable[maxProbabilityIndex];
        }
    }
}
