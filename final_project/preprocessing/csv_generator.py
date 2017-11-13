from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config

class CsvGenerator():
    def __init__(self):
        print(config()["dataset_path"])
