import matplotlib.pyplot as plt
from scipy.io import wavfile
from scipy.fftpack import rfft

class PitchClassProfiler():
    def __init__(self, file_name):
        self.file_name = file_name
        self.read = False

    def _read_file(self):
        self._frecuency, self._samples = wavfile.read(self.file_name)
        self.read = True

    def frecuency(self):
        if not self.read:
            self._read_file()
        return self._frecuency

    def samples(self):
        if not self.read:
            self._read_file()
        return self._samples

    def fourier(self):
        return rfft(self.samples())

    def plot_signal(self):
        plt.plot(self.samples())
        plt.show()

    def plot_fourier(self):
        plt.plot(self.fourier())
        plt.show()

    def get_profile(self):
        pass
