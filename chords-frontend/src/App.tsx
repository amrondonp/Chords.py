import { PredictionView } from "./Prediction";
import { Predictions } from "./Predictions";
import { NewPrediction } from "./NewPrediction";
import styles from "./App.module.css";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

function App() {
  return (
    <Router>
      <div className={styles.appContainer}>
        <Switch>
          <Route path="/predictions/:id">
            <PredictionView />
          </Route>
          <Route path="/">
            <h2>Predictions</h2>
            <NewPrediction />
            <Predictions />
          </Route>
        </Switch>
      </div>
    </Router>
  );
}

export default App;
