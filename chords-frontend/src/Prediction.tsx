import React from "react";
import { useState } from "react";
import { useParams } from "react-router-dom";
import { Prediction } from "./Predictions";
import styles from "./Prediction.module.css";
import { ChangeEvent } from "react";

export function PredictionView() {
  const { id } = useParams<{ id: string }>();
  const [error, setError] = React.useState(undefined);
  const [currentTime, setCurrentTime] = React.useState(0);
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
  }, [id]);

  const audioPlayerRef = React.useRef<HTMLAudioElement>(null);
  // const inputFile = React.useRef<HTMLInputElement | null>(null);

  const onChangeFile = (event: ChangeEvent<HTMLInputElement>) => {
    const audioPlayer = audioPlayerRef.current;
    if (audioPlayer) {
      const fileURL = URL.createObjectURL(
        event.target.files && event.target.files[0]
      );
      audioPlayer.src = fileURL;
      audioPlayer.play();
    }
  };

  React.useEffect(() => {
    const timer = setInterval(() => {
      audioPlayerRef.current &&
        setCurrentTime(audioPlayerRef.current?.currentTime);
    }, 100);

    return () => {
      clearTimeout(timer);
    };
  }, []);

  if (error) {
    return <div>{error}</div>;
  }

  if (!prediction) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <input
        id="audio_file"
        type="file"
        accept="audio/*"
        onChange={onChangeFile}
      />
      <audio ref={audioPlayerRef} />
      <p>{currentTime}</p>
      <div className={styles.chordList}>
        {prediction.chords?.map((chord, i) => (
          <div className={styles.chordContainer}>
            <div className={styles.time}>
              {(i * chord.sampleLength) / chord.sampleRate}s
            </div>
            <div className={styles.chord}>{chord.name}</div>
          </div>
        ))}
      </div>
    </div>
  );
}
