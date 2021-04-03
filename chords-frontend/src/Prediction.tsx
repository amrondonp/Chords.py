import { useParams } from "react-router-dom";

export function Prediction() {
  const { id } = useParams<{ id: string }>();
  return <div>Prediction with id {id}</div>;
}
