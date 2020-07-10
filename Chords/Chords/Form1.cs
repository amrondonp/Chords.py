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
        
        private Button[] chordButtons;
        private readonly IProgress<int> chordProcessingProgress;
        private readonly IProgress<double> audioPlayProgress;
        private AudioPlayer.AudioPlayer audioPlayer;
        private int windowInMs = 500;
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

            numericUpDown1.Minimum = 100;
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Value = windowInMs;

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private void FocusChordPlayedAtTime(double milliseconds)
        {
            label1.Text = "Audio played up to " + milliseconds + " ms";
            int playedChord = (int)Math.Floor(milliseconds / windowInMs);
            
            if (playedChord < this.chordButtons.Length)
            {
                this.chordButtons[playedChord].BackColor = HIGHLIGHT_COLOR;

                if(this.doAutoScroll.Checked)
                {
                    this.flowLayoutPanel1.ScrollControlIntoView(chordButtons[playedChord]);
                }
            }

            for(int i = 0; i < chordButtons.Length; i++)
            {
                if(i != playedChord && chordButtons[i].BackColor == HIGHLIGHT_COLOR)
                {
                    chordButtons[i].BackColor = progressLabel.BackColor;
                }
            }
        }

        private async void OpenFileClicked(object sender, EventArgs e)
        {
            filePath = GetOpenedFilePath();
            if (filePath == null)
            {
                return;
            }

            await CalculateChords();
        }

        private async Task CalculateChords()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var chordsPredicted = await Task.Run(() =>
                LongAudioProfiling.GetPredictionWithProgressReport(filePath, this.chordProcessingProgress, windowInMs)
            );
            sw.Stop();

            progressLabel.Text = "Chords computed successfully Elapsed=" + sw.Elapsed.TotalMilliseconds;
            ShowChordButtons(chordsPredicted);
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

        private void ShowChordButtons(string[] chordsPredicted)
        {
            this.flowLayoutPanel1.Controls.Clear();
            chordButtons = new Button[chordsPredicted.Length];
            int biggestButtonWidth = 0;
            int biggestButtonHeight = 0;

            for (int i = 0; i < chordsPredicted.Length; i++)
            {
                var chord = chordsPredicted[i];
                var button = new Button
                {
                    Text = chord,
                    Font = new Font(this.Font.FontFamily, 15),
                };

                int buttonIndex = i;
                button.Click += new EventHandler((obj, args) => {
                    audioPlayer.Stop();
                    audioPlayer.SetPositionInMs(buttonIndex * windowInMs);
                });

                chordButtons[i] = button;
                biggestButtonWidth = Math.Max(biggestButtonWidth, button.PreferredSize.Width);
                biggestButtonHeight = Math.Max(biggestButtonHeight, button.PreferredSize.Height);
            }

            foreach(Button button in chordButtons)
            {
                button.Width = biggestButtonWidth;
                button.Height = biggestButtonHeight;
            }

            this.flowLayoutPanel1.Controls.AddRange(chordButtons);
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
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Play();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Pause();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Stop();
            audioPlayer.SetPositionInMs(0);
        }

        private async void RecalculateButton_Click(object sender, EventArgs e)
        {   
            if(audioPlayer == null)
            {
                return;
            }

            windowInMs = (int)numericUpDown1.Value;
            await CalculateChords();
        }
    }
}
