import { PredictionView } from "./Prediction";
import { Predictions } from "./Predictions";
import { NewPrediction } from "./NewPrediction";
import styles from "./App.module.css";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { ThemeProvider } from "@fluentui/react";
import { guitarTheme } from "./Themes";

function App() {
  return (
    <ThemeProvider applyTo="body" theme={guitarTheme}>
      <Router>
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
