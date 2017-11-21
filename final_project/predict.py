import sys
from neural_network.classifier import Classifier

classifier = Classifier()
file_path = sys.argv[1] if len(sys.argv) > 1 else "songs/d.wav"
classifier.classify(file_path)

