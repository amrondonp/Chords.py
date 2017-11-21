import pandas
import numpy as np
import keras
import itertools
import matplotlib.pyplot as plt
from keras.models import Sequential, load_model
from keras.layers import Dense
from preprocessing.pitch_class_profiling import PitchClassProfiler
from util import config
from keras import metrics
import sklearn.metrics

class Trainer():
    def __init__(self, file_name="my_model.h5", loss_function="binary_crossentropy"):
        self.pitches = ["c", "d", "dm", "e", "em", "f", "g", "a", "am", "bm"]
        self.trained = False
        self.file_name = file_name
        self.loss_function = loss_function

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
        validation = {}
        for instrument in config()["instruments"]:
            X = self.validation_input_data(instrument).values
            Y = self.validation_output_data().values

            scores = self.model().evaluate(X, Y)
            for i in range(1, len(self._model.metrics_names)):
                print("\nResults validating with training data: %s: %.2f%%" % (self._model.metrics_names[i], scores[i]*100))

            print(self._model.metrics_names)
            validation[instrument] = (self._model.metrics_names, scores)
        return validation

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

        self._model.compile(loss=self.loss_function, optimizer='adam',  metrics=[metrics.categorical_accuracy, metrics.top_k_categorical_accuracy])
        self._model.fit(X, Y, epochs=100, batch_size=10, verbose=0)

        scores = self._model.evaluate(X, Y)

        for i in range(1, len(self._model.metrics_names)):
            print("\nResults validating with training data: %s: %.2f%%" % (self._model.metrics_names[i], scores[i]*100))
        
        self.trained = True
        return self._model.metrics_names, scores

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

    def plot_confusion_matrix(self, dataset="training",
                            normalize=True,
                            title='Confusion matrix',
                            cmap=plt.cm.Blues):
        """
        This function prints and plots the confusion matrix.
        Normalization can be applied by setting `normalize=True`.
        """
        cm = self.confusion_matrix(dataset)
        classes = config()["pitches"]

        if normalize:
            cm = cm.astype('float') / cm.sum(axis=1)[:, np.newaxis]

        plt.imshow(cm, interpolation='nearest', cmap=cmap)
        plt.title(title)
        plt.colorbar()
        tick_marks = np.arange(len(classes))
        plt.xticks(tick_marks, classes, rotation=45)
        plt.yticks(tick_marks, classes)

        fmt = '.2f' if normalize else 'd'
        thresh = cm.max() / 2.
        for i, j in itertools.product(range(cm.shape[0]), range(cm.shape[1])):
            plt.text(j, i, format(cm[i, j], fmt),
                    horizontalalignment="center",
                    color="white" if cm[i, j] > thresh else "black")

        plt.tight_layout()
        plt.ylabel('True label')
        plt.xlabel('Predicted label')
        plt.show()

    def confusion_matrix(self, dataset="training"):
        if dataset == "training":
            input_data = self.input_data().values
            output_data = self.output_data().values
        else:
            input_data = self.validation_input_data(dataset).values
            output_data = self.validation_output_data().values
        
        predictions =  self.model().predict(input_data)

        expected_categories = []
        prediction_categories = []
        
        for i, prediction in enumerate( predictions ):
            expected_categories.append( np.argmax(prediction)  )
            prediction_categories.append( np.argmax(output_data[i]) )

        return sklearn.metrics.confusion_matrix(prediction_categories, expected_categories)

