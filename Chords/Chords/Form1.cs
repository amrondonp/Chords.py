using Chords.Profiling;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Chords
{
    public partial class Form1 : Form
    {
        private static readonly Color HIGHLIGHT_COLOR = Color.FromArgb(0, 204, 102);
        
        private Label[] chordLabels;
        private readonly IProgress<int> chordProcessingProgress;
        private readonly IProgress<double> audioPlayProgress;
        private AudioPlayer.AudioPlayer audioPlayer;
        private readonly int windowInMs = 500;
        private string filePath;

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
            int playedChord = (int)Math.Floor(milliseconds / windowInMs);
            
            if (playedChord < this.chordLabels.Length)
            {
                this.chordLabels[playedChord].BackColor = HIGHLIGHT_COLOR;
            }

            if(playedChord - 1 >= 0 && playedChord - 1 < this.chordLabels.Length)
            {
                this.chordLabels[playedChord - 1].BackColor = progressLabel.BackColor;
            }

            for(int i = playedChord + 1; i < chordLabels.Length; i++)
            {
                if(chordLabels[i].BackColor == HIGHLIGHT_COLOR)
                {
                    chordLabels[i].BackColor = progressLabel.BackColor;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void OpenFileClicked(object sender, EventArgs e)
        {
            filePath = GetOpenedFilePath();
            if (filePath == null)
            {
                return;
            }

            Stopwatch sw = new Stopwatch();

            sw.Start();
            var chordsPredicted = await Task.Run(() =>
                LongAudioProfiling.GetPredictionWithProgressReport(filePath, this.chordProcessingProgress, windowInMs)
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

        private void PlayButton_Click(object sender, EventArgs e)
        {
            audioPlayer.Play();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            audioPlayer.Pause();
        }

        private async void StopButton_Click(object sender, EventArgs e)
        {
            if(audioPlayer != null)
            {
                audioPlayer.Dispose();
            }
            FocusChordPlayedAtTime(0);

            audioPlayer = await Task.Run(() => new AudioPlayer.AudioPlayer(filePath, audioPlayProgress));
        }
    }
}
