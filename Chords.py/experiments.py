import os
import json
from neural_network.spliter import Spliter

class SplitSongExperiment():
    def __init__(self, results_file="split_song_results.json"):
        self.results_file = results_file

    def longest_common_subsequence(self, a, b):
        lengths = [[0 for j in range(len(b)+1)] for i in range(len(a)+1)]
        # row 0 and column 0 are initialized to 0 already
        
        for i, x in enumerate(a):
            for j, y in enumerate(b):
                if x == y:
                    lengths[i+1][j+1] = lengths[i][j] + 1
                else:
                    lengths[i+1][j+1] = max(lengths[i+1][j], lengths[i][j+1])
        return lengths[len(a)][len(b)]

    def accuracy(self, chord_list, expected_file):
        with open(expected_file, "r") as f:
            expected_list = f.readline().split(" ")

        coincidencies = 0.0
        number_of_elements = min( len(chord_list), len(expected_list) )

        for i in range( number_of_elements ):
            if chord_list[i] == expected_list[i]:
                coincidencies += 1.0

        accuracy = { 
            "absolute": coincidencies / number_of_elements,
            "lcs": self.longest_common_subsequence(chord_list, expected_list) / number_of_elements
        }

        return accuracy


    def results(self):
        song_names = [ name.split(".")[0] for name in os.listdir("songs/guitar") ]
        results = {
            "voice": [],
            "guitar": [],
            "karaoke": []
        }

        songs = {
            "voice": [ "songs/voice/" + name + ".wav" for name in song_names ],
            "karaoke": [ "songs/karaoke/" + name + ".wav" for name in song_names ],
            "guitar": [ "songs/guitar/" + name + ".wav" for name in song_names ]
        }

        expected_splits = [ "songs_chords/expected/" + name + ".txt" for name in song_names ]

        for key in songs.keys():
            for i in range( len( songs[key] ) ):
                song =  songs[key][i]
                print("Processing " + song)
                spliter = Spliter(song)
                chords = spliter.split_song()
                accuracy = self.accuracy(chords, expected_splits[i])

                results[key].append({
                    "song": song,
                    "accuracy": accuracy
                })
            
        return results

    def save_results(self):
        results = self.results()
        with open(self.results_file, "w") as f:
            f.write( json.dumps(results) )

splitSongExperiment = SplitSongExperiment()
splitSongExperiment.save_results()
#print ( splitSongExperiment.longest_common_subsequence("XMJYAUZ", "MZJAWXU") )