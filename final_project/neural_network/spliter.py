import numpy as np

from preprocessing.pitch_class_profiling import LongFileProfiler, PitchClassProfiler
from neural_network.train import Trainer
from util import config

class Spliter():
    def __init__(self, song_file, out_file="spliter_result.txt"):
        self.song_file = song_file
        self.out_file=out_file

    def split_song(self):
        trainer = Trainer()
        trainer.load()

        longFileProfiler = LongFileProfiler(self.song_file)
        profiles = longFileProfiler.get_profile()
        
        chords = []

        for profile in profiles:
            X = np.array( [profile] )
            prediction = trainer.model().predict(X)
            chord_index = np.argmax(prediction)
            
            chords.append( config()["pitches"][chord_index] )
        return chords

    def save_split(self):
        chords = self.split_song()
        chords_string = " ".join(chords)
        with open(self.out_file, "w") as f:
            f.write( chords_string )
        print( "Split result saved in " + self.out_file )