import { ONNXPredictor } from "./Prediction";

test("ONNXPredictor", () => {
  const prediction = ONNXPredictor({
    sampleRate: 0,
    samples: new Float32Array(1),
  });

  expect(prediction).toBeTruthy();
});
