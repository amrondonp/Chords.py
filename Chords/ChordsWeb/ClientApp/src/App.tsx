import { PredictionView } from "./Prediction";
import { Predictions } from "./Predictions";
import { NewPrediction } from "./NewPrediction";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { ThemeProvider, Toggle } from "@fluentui/react";
import { blueTheme, guitarTheme } from "./Themes";

import styles from "./App.module.css";
import "./Theme.css";
import React from "react";

function App() {
  const [isGuitarTheme, setIsGuitarTheme] = React.useState(true);

  React.useEffect(() => {
    document.documentElement.setAttribute(
      "data-theme",
      isGuitarTheme ? "guitar" : "blue"
    );
  }, [isGuitarTheme]);

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
              <Predictions />
              <NewPrediction />
            </Route>
          </Switch>
        </div>
      </Router>
    </ThemeProvider>
  );
}

export default App;
