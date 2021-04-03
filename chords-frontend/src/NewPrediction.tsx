import styles from "./NewPrediction.module.css";
import { PrimaryButton } from "@fluentui/react";

export function NewPrediction() {
  return (
    <div className={styles.container}>
      <span>Upload a song to get the chords</span>
      <PrimaryButton text="Primary" onClick={() => {}} />
    </div>
  );
}
