using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Timers;

namespace Chords.AudioPlayer
{
    public class AudioPlayer
    {
        private readonly AudioFileReader reader;
        private readonly WaveOutEvent waveOut;
        private readonly IProgress<double> progress;
        private readonly Timer timer;

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            progress.Report(waveOut.GetPositionTimeSpan().TotalMilliseconds);
        }

        public AudioPlayer(string fileName, IProgress<double> progress)
        {
            this.progress = progress;
            
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 100;


            reader = new AudioFileReader(fileName);
            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
        }

        public void Play()
        {
            timer.Enabled = true;
            waveOut.Play();
        }

        public void Stop()
        {
            waveOut.Stop();
        }

        public void Dispose()
        {
            Stop();

            timer.Stop();
            timer.Close();
            reader.Dispose();
            waveOut.Dispose();
        }
    }
}
