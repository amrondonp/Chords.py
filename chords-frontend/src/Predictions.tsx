import React from "react";
import { Link } from "react-router-dom";
import styles from "./Predictions.module.css";

export interface Prediction {
  id: number;
  progress: number;
  filePath: string;
  fileName: string;
  windowInMs: number;
  autoBorder: boolean;
  modelName: string;
  chords?: any[];
}

export function Predictions() {
  const [predictions, setPredictions] = React.useState<
    Prediction[] | undefined
  >(undefined);
  const [error, setError] = React.useState(undefined);

  React.useEffect(() => {
    fetch("http://localhost:25026/api/predictions")
      .then(async (response) => {
        const data = await response.json();
        setPredictions(data);
      })
      .catch((error) => {
        setError(error);
      });
  }, []);

  if (error) {
    return <div>{error}</div>;
  }

  if (!predictions) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <div className={styles.list}>
        <h3>id</h3>
        <h3>Name</h3>
        <h3>Progress</h3>
        {predictions.map((prediction) => {
          return (
            <>
              <div className={styles.listRow}>
                {
                  <Link to={`/predictions/${prediction.id}`}>
                    {prediction.id}
                  </Link>
                }
              </div>
              <div className={styles.listRow}>{prediction.fileName}</div>
              <div className={styles.listRow}>{prediction.progress}%</div>
            </>
          );
        })}
      </div>
    </>
  );
}
