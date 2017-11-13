from preprocessing.pitch_class_profiling import PitchClassProfiler

CHORDS_FOLDER = "C:/Users/amron/Downloads/jim2012Chords/Guitar_Only/"

pitch_class_profiler = PitchClassProfiler(CHORDS_FOLDER + "e/e1.wav")

pitch_class_profiler.plot_profile()


