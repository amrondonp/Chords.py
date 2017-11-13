import sys
from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config

CHORDS_FOLDER = config()["chords_path"] 

def generate_csv(pitch):
    with open("dataset/" + pitch + ".csv", "w") as f: 
        for i in range(1,201):
            name = "/" + pitch + "/" + pitch + str(i) + ".wav"
            print(name)
            pitch_class_profiler = PitchClassProfiler(CHORDS_FOLDER + name)
            profile = pitch_class_profiler.get_profile()
            for j in range(12):
                f.write(str(profile[j]))
                if( j < 11):
                    f.write(", ")
                else:
                    f.write("\n")

def plot_test(file):
    pitch_class_profiler = PitchClassProfiler(CHORDS_FOLDER + "/" + file)
    print(pitch_class_profiler.get_profile())
    pitch_class_profiler.plot_profile()

#plot_test("d/d200.wav")
generate_csv("em")
#https://ccrma.stanford.edu/~joshua79/220a/final-proj.html