export type AudioSample = {
  samples: Float32Array;
  sampleRate: number;
};

export type Predictor = (audioSample: AudioSample) => string;

export const ONNXPredictor: Predictor = (audioSample: AudioSample): string => {
  return "H";
};
