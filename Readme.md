Chord Recognition in Python

This program is a university project on the introductory course to artificial intelligence.

You'll need python 3, pip, and virtualenv(optional but recomended) to run the program

1. Clone the repository
2. Go inside the project folder `cd Chords.py/final_project`
3. Create a virtualenv ```virtualenv -p python3 my_env```
4. Activate your environment ```source my_env/bin/activate``` (linux) ```my_env/Scripts/activate.bat``` (windows)
5. Install the dependencies ```pip install -r requirements.txt```   

At this point you have the environment ready to use several entry points.

* `python predict.py` or `python predict.py path/to/song.wav` will give you a prediction of the chord present in that sound file, by default it goes to `songs/d.wav` which is the D chord. It will show a window similar to this one.

![title](final_project/images/predict.png)


