import sys
from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config

CHORDS_FOLDER = config()["chords_path"] 

#plot_test("c/c50.wav")
#generate_csv("em")
#https://ccrma.stanford.edu/~joshua79/220a/final-proj.html

from neural_network.train import Trainer
trainer = Trainer()
#trainer.save()


#trainer.train()
#prediction = trainer.predict(CHORDS_FOLDER + "/a/a2.wav")
#print(prediction.tolist()[0])
#trainer.plot_prediction(CHORDS_FOLDER + "/g/gMauro.wav")

#trainer.plot_prediction("C:/Users/amron/Downloads/jim2012Chords/Other_Instruments/Piano/bm/bm2.wav")

from preprocessing.csv_generator import CsvGenerator
a = CsvGenerator()
a.generate_training_dataset()