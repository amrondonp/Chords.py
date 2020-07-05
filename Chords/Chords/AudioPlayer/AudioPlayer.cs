using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Chords.AudioPlayer
{
    public class AudioPlayer
    {
        private AudioFileReader reader;
        private WaveOutEvent waveOut;
        private IProgress<double> progress;
        private Timer timer;

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            progress.Report(waveOut.GetPositionTimeSpan().TotalMilliseconds);
        }

        public AudioPlayer(string fileName, IProgress<double> progress)
        {
            this.progress = progress;
            
            timer = new System.Timers.Timer();
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
    }
}
