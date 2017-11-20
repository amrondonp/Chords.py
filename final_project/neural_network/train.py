import pandas
import numpy as np
import keras
import matplotlib.pyplot as plt
from keras.models import Sequential, load_model
from keras.layers import Dense
from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config

class Trainer():
    def __init__(self, file_name="my_model.h5"):
        self.pitches = ["c", "d", "dm", "e", "em", "f", "g", "a", "am", "bm"]
        self.trained = False
        self.file_name = file_name

    def read_pitch_csv(self, folder_name):
        data = pandas.DataFrame()
        list_ = []

        for pitch in self.pitches:
            file_data = pandas.read_csv(folder_name + pitch + ".csv", header=None)
            list_.append(file_data)
        data = pandas.concat(list_)
        return data

    def out_data_generator(self, how_many):
        list_ = []

        for i in range(len(self.pitches)):
            for _ in range(how_many):
                out = [0.0 for _ in range(len(self.pitches))]
                out[i] = 1.0
                list_.append(out)

        data = pandas.DataFrame(list_)
        return data

    def validation_input_data(self, instrument):
        return self.read_pitch_csv("dataset/validation/" + instrument + "/")

    def validation_output_data(self):
        return self.out_data_generator(10)

    def input_data(self):
        return self.read_pitch_csv("dataset/train/")

    def output_data(self):
        return self.out_data_generator(200)

    def validate(self):
        for instrument in config()["instruments"]:
            X = self.validation_input_data(instrument).values
            Y = self.validation_output_data().values

            scores = self.model().evaluate(X, Y)
            print("Results validating " + instrument + " : %s: %.2f%%" % (self._model.metrics_names[1], scores[1]*100) + "\n" )

    def model(self):
        if not self.trained:
            self.train()
        return self._model

    def save(self):
        self.model().save(self.file_name)

    def save_architecture(self):
        json_string = self.model_architecture()
        json_file_name = self.file_name.split(".")[0] + ".json"
        with open(json_file_name, "w") as f:
            f.write(json_string)

    def model_architecture(self):
        return self.model().to_json()

    def load(self):
        self._model = load_model(self.file_name)
        self.trained = True

    def train(self):
        self._model = Sequential()
        self._model.add(Dense(30, input_dim=12, activation='relu'))
        self._model.add(Dense(10, activation='sigmoid'))

        X = self.input_data().values
        Y = self.output_data().values

        self._model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])
        self._model.fit(X, Y, epochs=100, batch_size=10)

        scores = self._model.evaluate(X, Y)
        print("\nResults validating with training data: %s: %.2f%%" % (self._model.metrics_names[1], scores[1]*100))
        self.trained = True

    def predict(self, audio_file):
        profiler = PitchClassProfiler(audio_file)
        X = np.array( [profiler.get_profile()] )
        return self.model().predict(X)

    def plot_prediction(self, audio_file):
        objects = ["C", "D", "Dm", "E", "Em", "F", "G", "A", "Am", "Bm"]
        y_pos = np.arange(len(objects))
        performance = self.predict(audio_file)[0]

        plt.bar(y_pos, performance, align='center', alpha=0.5)
        plt.xticks(y_pos, objects)
        plt.ylabel('Probability')
        plt.title('Classification results')

        plt.show()
