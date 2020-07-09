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
            progress.Report(reader.CurrentTime.TotalMilliseconds);
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

        public void Pause()
        {
            waveOut.Pause();
        }

        public void Stop()
        {
            waveOut.Stop();
        }

        public void SetPositionInMs(int milliseconds)
        {
            reader.CurrentTime = reader.CurrentTime.Subtract(reader.CurrentTime).Add(new TimeSpan(0, 0, 0, 0, milliseconds));
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
