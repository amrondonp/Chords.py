import styles from "./NewPrediction.module.css";
import { PrimaryButton, DefaultButton } from "@fluentui/react";
import React, { ChangeEvent } from "react";

export function NewPrediction() {
  const [file, setFile] = React.useState<File | null>(null);
  const inputFile = React.useRef<HTMLInputElement | null>(null);
  const onClick = () => {
    inputFile.current?.click();
  };

  const onChangeFile = (event: ChangeEvent<HTMLInputElement>) => {
    setFile(event.target?.files && event.target?.files[0]);
  };

  const onFileCommitted = () => {};

  return (
    <div className={styles.container}>
      <input
        type="file"
        id="file"
        ref={inputFile}
        style={{ display: "none" }}
        onChange={onChangeFile}
      />
      <span>{file ? file.name : "No file selected"}</span>
      <DefaultButton text="Select File" onClick={onClick} />
      <PrimaryButton text="Get the chords" onClick={onFileCommitted} />
    </div>
  );
}
