using Chords.Entities;
using MathNet.Numerics.IntegralTransforms;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Chords.Profiling
{
    public class Profiling
    {
        public static (int sampleRate, float[] samples) GetSamples(
            string pathToAudioFile)
        {
            using var reader = new AudioFileReader(pathToAudioFile);

            var sampleProvider = reader.ToMono();
            var samples =
                new float[reader.Length / sizeof(float) /
                          reader.WaveFormat.Channels];
            var samplesRead = sampleProvider.Read(samples, 0, samples.Length);
            Array.Resize(ref samples, samplesRead);

            if (samples.Length != samplesRead)
            {
                throw new Exception(
                    "Error when reading the samples, samples.Length=" +
                    samples.Length + " samplesRead=" + samplesRead);
            }

            return (reader.WaveFormat.SampleRate, samples);
        }

        public static Complex[] GetFft(float[] samples)
        {
            var complexData = new Complex[samples.Length];
            for (var i = 0; i < samples.Length; i++)
            {
                complexData[i] = new Complex(samples[i], 0);
            }

            Fourier.Forward(complexData, FourierOptions.Matlab);
            return complexData;
        }

        public static double[] PitchClassProfile(Complex[] x, int frecuency)
        {
            double fs = frecuency;
            var fref = 130.81;

            double n = x.Length;

            int M(int l)
            {
                // Computing Math.Round(12 * Math.Log2(   (fs * l)/(N * fref)  ) ) % 12; step by step
                var aux = (fs * l) / (n * fref);
                var aux2 = Math.Log(aux) / Math.Log(2);
                var aux3 = 12 * aux2;
                var axu4 = (int)Math.Round(aux3);
                var aux5 = axu4 % 12;

                if (aux5 < 0)
                {
                    aux5 += 12;
                }

                return aux5;
            }

            var pcp = new double[12];
            var size = (int)(n / 2);

            for (var l = 1; l < size; l++)
            {
                var bin = M(l);
                var mag = x[l].Magnitude;
                var sq = mag * mag;
                pcp[bin] += sq;
            }

            // Normalize pcp
            var pcpNorm = new double[12];
            var pcpSum = pcp.Sum();
            for (var p = 0; p < 12; p++)
            {
                pcpNorm[p] = (pcp[p] / pcpSum);
            }

            return pcpNorm;
        }

        public static float[] GetRawPredictionForFile(string pathToAudioFile)
        {
            var (sampleRate, samples) = GetSamples(pathToAudioFile);
            return GetRawPrediction(sampleRate, samples).Item1;
        }

        public static (float[], double[]) GetRawPrediction(int sampleRate, float[] samples)
        {
            var fft = GetFft(samples);
            var pcp = PitchClassProfile(fft, sampleRate);

            var inputTensor = new DenseTensor<float>(new[] { 1, 12 });
            for (var i = 0; i < 12; i++)
            {
                inputTensor[0, i] = (float)pcp[i];
            }

            var input = new List<NamedOnnxValue>
                {NamedOnnxValue.CreateFromTensor("dense_1_input", inputTensor)};
            var session =
                new InferenceSession("models/binary_crossentropy.onnx");

            using var results = session.Run(input);

            return (results.First().AsEnumerable<float>().ToArray(), pcp);
        }

        public static string GetPredictionForFile(string pathToAudioFile)
        {
            var rawPrediction = GetRawPredictionForFile(pathToAudioFile);
            return GetPredictionFormRawPrediction(rawPrediction);
        }

        public static string GetPrediction(int sampleRate, float[] samples)
        {
            var rawPrediction = GetRawPrediction(sampleRate, samples);
            return GetPredictionFormRawPrediction(rawPrediction.Item1);
        }

        public static Chord GetPredictionWithChord(int sampleRate, float[] samples)
        {
            var (rawPrediction, pcp) = GetRawPrediction(sampleRate, samples);
            return new Chord(samples, sampleRate, GetPredictionFormRawPrediction(rawPrediction), pcp);
        }

        private static string GetPredictionFormRawPrediction(
            float[] rawPrediction)
        {
            var maxProbabilityIndex = 0;
            float maxProbabilty = 0;

            for (var i = 0; i < rawPrediction.Length; i++)
            {
                if (maxProbabilty < rawPrediction[i])
                {
                    maxProbabilty = rawPrediction[i];
                    maxProbabilityIndex = i;
                }
            }

            string[] chordsTable =
                {"C", "D", "Dm", "E", "Em", "F", "G", "A", "Am", "Bm"};
            return chordsTable[maxProbabilityIndex];
        }

        public static double[] PitchClassProfileForSamples(float[] samples, in int sampleRate)
        {
            return PitchClassProfile(GetFft(samples), sampleRate);
        }
    }
}
