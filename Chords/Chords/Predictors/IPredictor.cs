﻿using Chords.Entities;
using System;

namespace Chords.Predictors
{
    public interface IPredictor
    {
        public string GetPrediction(float[] sample, int sampleRate);

        public Chord GetPredictionWithChord(float[] sample, int sampleRate);

        public string[] GetPredictions(float[] samples,
            int sampleRate, int windowInMs, IProgress<int> progress);

        public string[] GetPredictionForFile(
            string filePath,
            IProgress<int> progress, int windowInMs);
    }
}
