import sys

from preprocessing.pitch_class_profiling import PitchClassProfiler

file_name = sys.argv[1] if len( sys.argv ) > 1 else "song/d.wav"

pitchClassProfiler = PitchClassProfiler(file_name)
pitchClassProfiler.plot_profile()
