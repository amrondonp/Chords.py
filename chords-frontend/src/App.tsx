import { Prediction } from "./Prediction";
import { Predictions } from "./Predictions";
import styles from "./App.module.css";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

function App() {
  return (
    <Router>
      <div className={styles.appContainer}>
        <Switch>
          <Route path="/predictions/:id">
            <Prediction />
          </Route>
          <Route path="/">
            <Predictions />
          </Route>
        </Switch>
      </div>
    </Router>
  );
}

export default App;
