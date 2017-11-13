import os

from io import StringIO
from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config

CHORDS_FOLDER = config()["chords_path"] 

class CsvGenerator():
    def __init__(self):
        pass

    def profile_to_csv_string(self, profile):
        output = ""
        for j in range(12):
            output += str(profile[j])
            if( j < 11):
                output += (", ")
            else:
                output += ("\n")
        return output

    def generate_csv(self, pitch):
        with open("dataset/train/" + pitch + ".csv", "w") as f: 
            for i in range(1,201):
                name = "/" + pitch + "/" + pitch + str(i) + ".wav"
                print(name)
                pitch_class_profiler = PitchClassProfiler(CHORDS_FOLDER + name)
                profile = pitch_class_profiler.get_profile()
                f.write(self.profile_to_csv_string(profile))
                
    def generate_training_dataset(self):
        print("Generating data... This may take several minutes")
        for pitch in config()["pitches"]:
            print("Generating data for pitch " + pitch)
            self.generate_csv(pitch)

