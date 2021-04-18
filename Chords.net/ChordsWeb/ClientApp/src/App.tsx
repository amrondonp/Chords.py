import { PredictionView } from "./Prediction";
import { Prediction, Predictions } from "./Predictions";
import { NewPrediction } from "./NewPrediction";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { ThemeProvider, Toggle } from "@fluentui/react";
import { blueTheme, guitarTheme } from "./Themes";

import styles from "./App.module.css";
import "./Theme.css";
import React from "react";
import { url } from "./urls";

function App() {
  const [isGuitarTheme, setIsGuitarTheme] = React.useState(true);
  const [predictions, setPredictions] = React.useState<
    Prediction[] | undefined
  >(undefined);
  const [error, setError] = React.useState(undefined);

  const fetchPredictions = React.useCallback(() => {
    fetch(url("/api/predictions"))
      .then(async (response) => {
        const data = await response.json();
        setPredictions(data);
      })
      .catch((error) => {
        setError(error);
      });
  }, []);

  React.useEffect(() => {
    document.documentElement.setAttribute(
      "data-theme",
      isGuitarTheme ? "guitar" : "blue"
    );
  }, [isGuitarTheme]);

  React.useEffect(() => {
    fetchPredictions();
  }, [fetchPredictions]);

  return (
    <ThemeProvider
      applyTo="body"
      theme={isGuitarTheme ? guitarTheme : blueTheme}
    >
      <Router>
        <div className={styles.navBar}>
          <h3>Chords</h3>
          <Toggle
            label="Theme"
            onText="guitar"
            offText="blue"
            inlineLabel
            onChange={(_: any, checked?: boolean) =>
              setIsGuitarTheme(checked ? true : false)
            }
            checked={isGuitarTheme}
          />
        </div>
        <div className={styles.appContainer}>
          <Switch>
            <Route path="/predictions/:id">
              <PredictionView />
            </Route>
            <Route path="/">
              <Predictions predictions={predictions} error={error} />
              <NewPrediction onPredictionCreated={fetchPredictions} />
            </Route>
          </Switch>
        </div>
      </Router>
    </ThemeProvider>
  );
}

export default App;
