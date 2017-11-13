import sys
from preprocessing.pitch_class_profiling import PitchClassProfiler

CHORDS_FOLDER = "C:/Users/amron/Downloads/jim2012Chords/Guitar_Only/"

with open("c.csv", "w") as f: 
    for i in range(1,6):
        name = "c/c" + str(i) + ".wav"
        print(name)
        pitch_class_profiler = PitchClassProfiler(CHORDS_FOLDER + name)
        profile = pitch_class_profiler.get_profile()
        for j in range(12):
            f.write(str(profile[j]))
            if( j < 11):
                f.write(", ")
            else:
                f.write("\n")


