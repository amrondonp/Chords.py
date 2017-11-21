import sys
from neural_network.train import Trainer

dataset = sys.argv[1] if len( sys.argv ) > 1 else "training"

trainer = Trainer()
trainer.load()
trainer.plot_confusion_matrix(dataset)

