import React from "react";

interface Prediction {
  id: number;
  progress: number;
  filePath: string;
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
    <div>
      {predictions.map((prediction) => {
        return (
          <div>
            <div>{prediction.filePath}</div>
            <div>{prediction.progress}</div>
          </div>
        );
      })}
    </div>
  );
}
