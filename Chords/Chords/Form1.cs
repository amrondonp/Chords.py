using Chords.Profiling;
using Chords.AudioPlayer;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Chords
{
    public partial class Form1 : Form
    {
        private Label[] chordLabels;
        private readonly IProgress<int> chordProcessingProgress;
        private readonly IProgress<double> audioPlayProgress;
        private AudioPlayer.AudioPlayer audioPlayer;

        public Form1()
        {
            InitializeComponent();

            chordProcessingProgress = new Progress<int>(v =>
            {
                progressBar1.Value = v;
                progressLabel.Text = "Computing chords... " + v + " %";
            });

            audioPlayProgress = new Progress<double>(milliseconds =>
            {
                FocusChordPlayedAtTime(milliseconds);
            });
        }

        private void FocusChordPlayedAtTime(double milliseconds)
        {
            label1.Text = "Audio played up to " + milliseconds + " ms";
            double window = 500;
            int playedChord = (int)Math.Floor(milliseconds / window);
            
            if (playedChord < this.chordLabels.Length)
            {
                this.chordLabels[playedChord].BackColor = Color.FromArgb(0, 204, 102);
            }

            if(playedChord - 1 >= 0 && playedChord - 1 < this.chordLabels.Length)
            {
                this.chordLabels[playedChord - 1].BackColor = progressLabel.BackColor;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void OpenFileClicked(object sender, EventArgs e)
        {
            var filePath = GetOpenedFilePath();
            if (filePath == null)
            {
                return;
            }

            Stopwatch sw = new Stopwatch();

            sw.Start();

            var chordsPredicted = await Task.Run(() =>
                LongAudioProfiling.GetPredictionWithProgressReport(filePath, this.chordProcessingProgress)
            );

            sw.Stop();

 
            progressLabel.Text = "Chords computed successfully Elapsed=" + sw.Elapsed.TotalMilliseconds;
            ShowChordLabels(chordsPredicted);
            PlayNewAudioFile(filePath);
        }

        private async void PlayNewAudioFile(string filePath)
        {
            if(audioPlayer != null)
            {
                audioPlayer.Dispose();
            }

            audioPlayer = await Task.Run(() => new AudioPlayer.AudioPlayer(filePath, audioPlayProgress));
            audioPlayer.Play();
        }

        private void ShowChordLabels(string[] chordsPredicted)
        {
            this.flowLayoutPanel1.Controls.Clear();
            chordLabels = new Label[chordsPredicted.Length];
            int biggestLabelx = 0;
            int biggestLabely = 0;

            for (int i = 0; i < chordsPredicted.Length; i++)
            {
                var chord = chordsPredicted[i];
                var label = new Label
                {
                    Text = chord,
                    Font = new Font(this.Font.FontFamily, 15),
                    BorderStyle = BorderStyle.FixedSingle,
                };

                chordLabels[i] = label;
                biggestLabelx = Math.Max(biggestLabelx, label.PreferredSize.Width);
                biggestLabely = Math.Max(biggestLabely, label.PreferredSize.Height);
            }

            foreach(Label label in chordLabels)
            {
                label.Width = biggestLabelx;
                label.Height = biggestLabely;
            }

            this.flowLayoutPanel1.Controls.AddRange(chordLabels);
        }

        private string GetOpenedFilePath()
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Audio (*.wav;*.mp3)|*.wav;*.mp3",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            return openFileDialog.FileName;
        }   
    }
}
