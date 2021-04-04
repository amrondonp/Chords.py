import React from "react";
import { useState } from "react";
import { useParams } from "react-router-dom";
import { Prediction } from "./Predictions";
import styles from "./Prediction.module.css";
import { ChangeEvent } from "react";
import classNames from "classnames/bind";
import { times } from "lodash-es";

const cx = classNames.bind(styles);
const secondInPixels = 160;

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

  const intervals = React.useMemo(
    () =>
      prediction?.chords ? getChordsTimeIntervalInS(prediction.chords) : [],
    [prediction?.chords]
  );

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
      <div className={styles.timeLineScrollContainer}>
        <div className={styles.timeLineContainer}>
          <div
            className={styles.timeRuler}
            style={{
              width: rulerWidth(intervals),
            }}
          >
            {times(2 * numberOfSeconds(intervals), (i) => {
              return (
                <div className={styles.secondBox}>
                  <span>{i > 0 && i % 2 === 0 ? (i / 2).toFixed(2) : ""}</span>
                  <div
                    className={
                      i % 2 === 1 ? styles.secondMark : styles.halfSecondMark
                    }
                  ></div>
                </div>
              );
            })}
          </div>
          <div className={styles.chordList}>
            {prediction.chords?.map((chord, i) => (
              <div
                className={styles.chordContainer}
                style={{ width: getChordWidth(chord) }}
                key={chord.id}
              >
                <div
                  className={
                    isInInterval(intervals[i], currentTime)
                      ? cx("chord", "playedChord")
                      : styles.chord
                  }
                >
                  {chord.name}
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

function numberOfSeconds(intervals: { start: number; end: number }[]) {
  return Math.ceil(intervals[intervals.length - 1].end);
}

function rulerWidth(intervals: { start: number; end: number }[]) {
  return secondInPixels * numberOfSeconds(intervals);
}

function getChordWidth(chord: any) {
  return secondInPixels * getChordDuration(chord);
}

function getChordDuration(chord: any) {
  return chord.sampleLength / chord.sampleRate;
}

function getChordsTimeIntervalInS(
  chords: any[]
): { start: number; end: number }[] {
  return chords.reduce((intervals, chord, i) => {
    const start = i === 0 ? 0 : intervals[i - 1].end;
    return intervals.concat({
      start,
      end: start + getChordDuration(chord),
    });
  }, []);
}

function isInInterval(
  { start, end }: { start: number; end: number },
  value: number
): boolean {
  return value >= start && value <= end;
}
