import React from "react";
import { useState } from "react";
import { useParams } from "react-router-dom";
import { Prediction } from "./Predictions";
import styles from "./Prediction.module.css";

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

  return (
    <div className={styles.chordList}>
      {prediction.chords?.map((chord) => (
        <div className={styles.chord}>{chord.name}</div>
      ))}
    </div>
  );
}
