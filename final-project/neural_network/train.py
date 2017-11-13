import pandas
import numpy as np
from keras.models import Sequential
from keras.layers import Dense

class Trainer():
    def __init__(self):
        self.pitches = ["c", "d", "dm", "e", "em", "f", "g", "a", "am", "bm"]

    def input_data(self):
        data = pandas.DataFrame()
        list_ = []

        for pitch in self.pitches:
            file_data = pandas.read_csv("dataset/" + pitch + ".csv", header=None)
            list_.append(file_data)
        data = pandas.concat(list_)
        return data

    def output_data(self):
        list_ = []

        for i in range(len(self.pitches)):
            for _ in range(200):
                out = [0.0 for _ in range(len(self.pitches))]
                out[i] = 1.0
                list_.append(out)

        data = pandas.DataFrame(list_)
        return data

    def train(self):
        model = Sequential()
        model.add(Dense(30, input_dim=12, activation='relu'))
        model.add(Dense(10, activation='sigmoid'))

        X = self.input_data().values
        Y = self.output_data().values

        model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])
        model.fit(X, Y, epochs=10, batch_size=10)

        scores = model.evaluate(X, Y)
        print("\n%s: %.2f%%" % (model.metrics_names[1], scores[1]*100))