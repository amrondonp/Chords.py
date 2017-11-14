from neural_network.train import Trainer

class Classifier():
    def __init__(self, train=False):
        self.train = train
        self.trainer = Trainer()

        if not self.train:
            self.trainer.load()
        else:
            self.trainer.train()

    def classify(self, audio_file_path):
        #prediction = self.trainer.predict(audio_file_path)
        self.trainer.plot_prediction(audio_file_path)