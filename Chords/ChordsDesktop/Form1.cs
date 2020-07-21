using Chords.Predictors;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChordsDesktop
{
    public partial class Form1 : Form
    {
        private static readonly Color HighlightColor =
            Color.FromArgb(0, 204, 102);

        private Button[] chordButtons;
        private readonly IProgress<int> chordProcessingProgress;
        private readonly IProgress<double> audioPlayProgress;
        private AudioPlayer.AudioPlayer audioPlayer;
        private int windowInMs = 500;
        private string filePath;
        private IPredictor predictor;

        public Form1(IPredictor predictor)
        {
            InitializeComponent();

            chordProcessingProgress = new Progress<int>(v =>
            {
                progressBar1.Value = v;
                progressLabel.Text = $@"Computing chords... {v} %";
            });

            audioPlayProgress = new Progress<double>(milliseconds =>
            {
                FocusChordPlayedAtTime(milliseconds);
            });

            numericUpDown1.Minimum = 100;
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Value = windowInMs;

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.predictor = predictor;
        }

        public sealed override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        private void FocusChordPlayedAtTime(double milliseconds)
        {
            label1.Text = $@"Audio played up to {milliseconds} ms";
            var playedChord = (int)Math.Floor(milliseconds / windowInMs);

            if (playedChord < chordButtons.Length)
            {
                chordButtons[playedChord].BackColor = HighlightColor;

                if (doAutoScroll.Checked)
                {
                    flowLayoutPanel1.ScrollControlIntoView(
                        chordButtons[playedChord]);
                }

                bigChordLabel.Text = chordButtons[playedChord].Text;
            }

            for (var i = 0; i < chordButtons.Length; i++)
            {
                if (i != playedChord &&
                    chordButtons[i].BackColor == HighlightColor)
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
            var sw = new Stopwatch();

            sw.Start();
            var chordsPredicted = await Task.Run(() =>
                predictor.GetPredictionForFile(filePath,
                    chordProcessingProgress, windowInMs)
            );
            sw.Stop();

            progressLabel.Text =
                $@"Chords computed successfully Elapsed={sw.Elapsed.TotalMilliseconds}";

            ShowChordButtons(chordsPredicted);
            PlayNewAudioFile(filePath);
        }

        private async void PlayNewAudioFile(string filePathToLoad)
        {
            audioPlayer?.Dispose();

            audioPlayer = await Task.Run(() =>
                new AudioPlayer.AudioPlayer(filePathToLoad, audioPlayProgress));
            audioPlayer.Play();
        }

        private void ShowChordButtons(string[] chordsPredicted)
        {
            flowLayoutPanel1.Controls.Clear();
            chordButtons = new Button[chordsPredicted.Length];
            var biggestButtonWidth = 0;
            var biggestButtonHeight = 0;

            for (var i = 0; i < chordsPredicted.Length; i++)
            {
                var chord = chordsPredicted[i];
                var button = new Button
                {
                    Text = chord,
                    Font = new Font(Font.FontFamily, 15),
                };

                var buttonIndex = i;
                button.Click += (obj, args) =>
                {
                    if (audioPlayer == null)
                    {
                        return;
                    }

                    audioPlayer.Stop();
                    audioPlayer.SetPositionInMs(buttonIndex * windowInMs);
                };

                chordButtons[i] = button;
                biggestButtonWidth = Math.Max(biggestButtonWidth,
                    button.PreferredSize.Width);
                biggestButtonHeight = Math.Max(biggestButtonHeight,
                    button.PreferredSize.Height);
            }

            foreach (var button in chordButtons)
            {
                button.Width = biggestButtonWidth;
                button.Height = biggestButtonHeight;
            }

            flowLayoutPanel1.Controls.AddRange(chordButtons.ToArray<Control>());
        }

        private string GetOpenedFilePath()
        {
            using var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = @"Audio (*.wav;*.mp3)|*.wav;*.mp3",
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
            audioPlayer?.Play();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            audioPlayer?.Pause();
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
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Stop();

            windowInMs = (int)numericUpDown1.Value;
            await CalculateChords();
        }
    }
}
