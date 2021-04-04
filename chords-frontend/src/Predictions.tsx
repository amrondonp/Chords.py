import {
  DetailsList,
  SelectionMode,
  Spinner,
  SpinnerSize,
} from "@fluentui/react";
import React from "react";
import { useHistory } from "react-router-dom";

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
    key: "name",
    name: "Name",
    fieldName: "name",
    minWidth: 100,
    maxWidth: 200,
    isResizable: true,
  },
  {
    key: "progress",
    name: "Progress",
    fieldName: "progress",
    minWidth: 100,
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
    <DetailsList
      columns={columns}
      items={listItems}
      selectionMode={SelectionMode.none}
      onItemInvoked={onItemClicked}
    ></DetailsList>
  );
}
