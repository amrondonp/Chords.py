import styles from "./NewPrediction.module.css";
import { PrimaryButton, DefaultButton, Spinner } from "@fluentui/react";
import { url } from "./urls";
import React, { ChangeEvent } from "react";

interface NewPredictionProps {
  onPredictionCreated?: () => void;
}

export function NewPrediction({ onPredictionCreated }: NewPredictionProps) {
  const [loadingState, setLoadingState] = React.useState<"loading" | "loaded">(
    "loaded"
  );

  const [file, setFile] = React.useState<File | null>(null);
  const inputFile = React.useRef<HTMLInputElement | null>(null);
  const onClick = () => {
    inputFile.current?.click();
  };

  const onChangeFile = (event: ChangeEvent<HTMLInputElement>) => {
    setFile(event.target?.files && event.target?.files[0]);
  };

  const onFileCommitted = () => {
    if (file) {
      setLoadingState("loading");
      const formData = new FormData();
      formData.append("file", file);
      fetch(url("/api/predictions"), {
        body: formData,
        method: "post",
      }).then(() => {
        setLoadingState("loaded");
        if (onPredictionCreated) {
          onPredictionCreated();
        }
      });
    }
  };

  const isLoading = loadingState === "loading";

  return (
    <div className={styles.container}>
      <h3>Create new prediction</h3>
      <input
        type="file"
        id="file"
        ref={inputFile}
        style={{ display: "none" }}
        onChange={onChangeFile}
      />
      <span>{file ? file.name : "No file selected"}</span>
      <DefaultButton
        text="Select File"
        onClick={onClick}
        disabled={isLoading}
      />
      <PrimaryButton
        text="Get the chords"
        onClick={onFileCommitted}
        disabled={isLoading}
      />
      {isLoading && <Spinner />}
    </div>
  );
}
