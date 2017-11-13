import matplotlib.pyplot as plt
import numpy as np
from scipy.io import wavfile
from scipy.fftpack import fft
from math import log2

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
        return fft(self.samples())

    def plot_signal(self):
        plt.plot(self.samples())
        plt.show()

    def plot_fourier(self):
        plt.plot(self.fourier())
        plt.show()

    def get_profile(self):
        #The algorithm here is implemented using
        #the names of the math formula as shown in the paper

        fs = self.frecuency()
        X = self.fourier()
        fref = [16.35, 17.32, 18.35, 19.45, 20.60, 21.83, 23.12, 24.50, 25.96, 27.50, 29.14, 30.87]

        N = len(X)
        assert(N % 2 == 0)

        def M(l, p):
            if l == 0:
                return -1
            return round(12 * log2( (fs * l)/(N * fref[p] )  ) ) % 12

        pcp = [0 for p in range(12)]
        
        for p in range(12):
            for l in range(N//2):
                if p == M(l, p):
                    pcp[p] += abs(X[l])**2
        
        #Normalize pcp
        pcp_norm = [0 for p in range(12)]
        for p in range(12):
            pcp_norm[p] = (pcp[p] / sum(pcp))

        return pcp_norm

    def plot_profile(self):
        objects = ('C', 'C#', 'D', 'D#', 'E', 'F', 'F#', 'G', 'G#', 'A', 'A#', 'B')
        y_pos = np.arange(len(objects))
        performance = self.get_profile()
        
        plt.bar(y_pos, performance, align='center', alpha=0.5)
        plt.xticks(y_pos, objects)
        plt.ylabel('Energy')
        plt.title('PCP results')
        
        plt.show()

