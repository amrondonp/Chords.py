# Chord Recognition in Python and C

## Demo aplication

The demo application is written in C# and .NET core, as of July 9, 2020 the Windows 10 64 bits is available is the only version available. Versions for Linux is expected to come as a console application or as a GUI application whenever .NET core implements GUI :) for linux

Installing the application.

The demo app uses [ONNX runtime](https://github.com/microsoft/onnxruntime) to run the exported model for python and it needs [Visual C++](https://aka.ms/vs/16/release/vc_redist.x64.exe) to be installed on the machine that is going to run the app.

Steps.

1. Install [Visual C++](https://aka.ms/vs/16/release/vc_redist.x64.exe) from the Microsoft web site
2. Donwload the application from our [Releases](https://github.com/amrondonp/Chords.py/releases/)
3. Extact the folder and run `Chords.exe`

![title](final_project/images/demoapp.png)

### Current Features

- Load any .WAV or .MP3 file and it will try to predict the chrods for each half a second
- Play, Pause and Stop audio controls
- Seek to any chord in particular and resume the reproduction from there.

### Incoming features.

- Ability to change the length of the window for analizyng the chrods
- Ability to correct the model prection
- Retrain the model based on your corrections

# Training

NOTE this was tested using the following setup:

```
os: Ubuntu 18.04

python --version
Python 3.6.9
```

This program is a university project on the introductory course to artificial intelligence.

You'll need python 3, pip, and virtualenv(optional but recomended) to run the program

1. Clone the repository
2. Go inside the project folder `cd Chords.py/final_project`
3. Create a virtualenv `virtualenv -p python3 my_env`
4. Activate your environment `source my_env/bin/activate` (linux) `my_env/Scripts/activate.bat` (windows)
5. Install the dependencies `pip install -r requirements.txt`

At this point you have the environment ready to use several entry points.

- `python predict.py` or `python predict.py path/to/song.wav` will give you a prediction of the chord present in that sound file, by default it goes to `songs/d.wav` which is the D chord. It will show a window similar to this one.

![title](final_project/images/predict.png)

- `python split.py songs/guitar/about_a_girl.wav` is an example of the `split.py` entry point that takes a longer song, **splits** it and runs the prediction for each song piece. The results are saved in a filed called `spliter_result.txt`

  - Example, if you ran `python split.py songs/guitar/about_a_girl.wav`, then `spliter_result.txt` would have the following content: `em g em g em em em g em em g g em g g g em g g g em em g em em g g em em g g` which are the chords that it was able to identify on the song.

- In the file `paper.pdf` you'll find the final report of this university project with some references added. Currently it is only available in Spanish, you could go and try to use a translator, hope it helps
