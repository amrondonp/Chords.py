import React from "react";
import { useState } from "react";
import { useParams } from "react-router-dom";
import { Prediction } from "./Predictions";

export function PredictionView() {
  const { id } = useParams<{ id: string }>();
  const [error, setError] = React.useState(undefined);
  const [prediction, setPrediction] = useState<Prediction | undefined>(
    undefined
  );

  React.useEffect(() => {
    fetch(`http://localhost:25026/api/predictions/${id}`)
      .then(async (response) => {
        const data = await response.json();
        setPrediction(data);
      })
      .catch((error) => {
        setError(error);
      });
  }, []);

  if (error) {
    return <div>{error}</div>;
  }

  if (!prediction) {
    return <div>Loading...</div>;
  }

  return <div>Prediction with id {JSON.stringify(prediction.chords)}</div>;
}
