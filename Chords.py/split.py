import sys
from neural_network.spliter import Spliter

song_file = sys.argv[1]
out_file = sys.argv[2] if len( sys.argv ) > 2 else "spliter_result.txt"

spliter = Spliter(song_file, out_file)
spliter.save_split()