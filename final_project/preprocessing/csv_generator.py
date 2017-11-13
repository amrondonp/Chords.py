import os

from io import StringIO
from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config

CHORDS_FOLDER = config()["chords_path"] 

class CsvGenerator():
    def __init__(self):
        pass

    def generate_validation_dataset(self):
        for validation_folder in config()["validation_data_folders"]:
            dst_folder_name = "dataset/validation/" + validation_folder.split("/")[-1]

            if not os.path.exists(dst_folder_name):
                os.makedirs(dst_folder_name)

            for pitch in config()["pitches"]:
                self.generate_csv(validation_folder, dst_folder_name, pitch, 10)

    def profile_to_csv_string(self, profile):
        output = ""
        for j in range(12):
            output += str(profile[j])
            if( j < 11):
                output += (", ")
            else:
                output += ("\n")
        return output

    def generate_csv(self, origin_folder, dest_folder, pitch, how_many):
        with open(dest_folder + "/" + pitch + ".csv", "w") as f: 
            for i in range(1, how_many + 1):
                name = "/" + pitch + "/" + pitch + str(i) + ".wav"
                print(name)
                pitch_class_profiler = PitchClassProfiler(origin_folder + name)
                profile = pitch_class_profiler.get_profile()
                f.write(self.profile_to_csv_string(profile))
                
    def generate_training_dataset(self):
        print("Generating data... This may take several minutes")
        for pitch in config()["pitches"]:
            print("Generating data for pitch " + pitch)
            self.generate_csv(CHORDS_FOLDER, "dataset/train", pitch)

