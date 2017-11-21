import sys
from util import config
from neural_network.classifier import Classifier, Trainer
from preprocessing.pitch_class_profiling import LongFileProfiler, PitchClassProfiler

# classifier = Classifier()
# file_path = sys.argv[1] if len(sys.argv) > 1 else "songs/d.wav"
# classifier.classify(file_path)


# profiler = PitchClassProfiler(file_path)
# profiler.plot_profile()


import numpy as np

trainer = Trainer()
trainer.load()

song = "guitar/about_a_girl.wav"

longFileProfiler = LongFileProfiler("songs/" + song)
profiles = longFileProfiler.get_profile()

with open("songs_chords/" + song.split(".")[0] + ".txt", "w") as f:
    for profile in profiles:
        X = np.array( [profile] )
        prediction = trainer.model().predict(X)
        chord_index = np.argmax(prediction)
        
        f.write( config()["pitches"][chord_index] + " " )

