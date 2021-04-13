import {
  DetailsList,
  SelectionMode,
  Spinner,
  SpinnerSize,
} from "@fluentui/react";
import React from "react";
import { useHistory } from "react-router-dom";
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

const columns = [
  {
    key: "id",
    name: "ID",
    fieldName: "id",
    minWidth: 50,
    maxWidth: 200,
    isResizable: true,
  },
  {
    key: "name",
    name: "Name",
    fieldName: "name",
    minWidth: 150,
    maxWidth: 200,
    isResizable: true,
  },
  {
    key: "progress",
    name: "Progress",
    fieldName: "progress",
    minWidth: 50,
    maxWidth: 200,
    isResizable: true,
  },
];

export function Predictions() {
  const [predictions, setPredictions] = React.useState<
    Prediction[] | undefined
  >(undefined);
  const [error, setError] = React.useState(undefined);
  const history = useHistory();

  React.useEffect(() => {
    fetch("/api/predictions")
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
    return <Spinner size={SpinnerSize.large} />;
  }

  const listItems = predictions.map((prediction) => ({
    key: prediction.id,
    name: prediction.fileName,
    progress: prediction.progress,
    id: prediction.id,
  }));

  const onItemClicked = (item?: any, index?: number, ev?: Event) => {
    history.push(`predictions/${item.id}`);
  };

  return (
    <div className={styles.container}>
      <h3>Predictions</h3>
      <DetailsList
        columns={columns}
        items={listItems}
        selectionMode={SelectionMode.none}
        onItemInvoked={onItemClicked}
      ></DetailsList>
    </div>
  );
}
