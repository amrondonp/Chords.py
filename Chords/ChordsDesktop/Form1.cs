using Chords.Entities;
using Chords.Predictors;
using Chords.Profiling;
using Chords.Repositories;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private readonly Panel[] containerPanels;
        private readonly PictureBox[] powers;
        private Chord[] chordsPredicted;
        private int[] chordsIntervals;
        private float[] samples;
        private int sampleRate;
        private readonly IProgress<int> chordProcessingProgress;
        private readonly IProgress<(int, string)> trainProgress;
        private readonly IProgress<double> audioPlayProgress;
        private AudioPlayer.AudioPlayer audioPlayer;
        private int playedChord;
        private int windowInMs = 500;
        private string filePath;
        private IPredictor predictor;
        private readonly IChordRepository repository;
        private readonly string generatedModelsDirectory;

        public Form1(IPredictor predictor, IChordRepository repository)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = new Point(100, 100);

            chordProcessingProgress = new Progress<int>(v =>
            {
                progressBar1.Value = v;
                progressLabel.Text = $@"Computing chords... {v} %";
            });

            trainProgress = new Progress<(int, string)>(v =>
            {
                progressBar1.Value = v.Item1;
                progressLabel.Text = $@"{v.Item2} {v.Item1} %";
            });

            audioPlayProgress = new Progress<double>(milliseconds =>
            {
                FocusChordPlayedAtTime(milliseconds);
            });

            numericUpDown1.Minimum = 100;
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Value = windowInMs;

            trainSeconds.Minimum = 1;
            trainSeconds.Maximum = int.MaxValue;
            trainSeconds.Value = 180;

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            toolStripMenuItem3.Click += new EventHandler(ChangeModel);

            containerPanels = new Panel[12];
            powers = new PictureBox[12];
            for(int i = 0; i < 12; i++)
            {
                var panel = new Panel
                {
                    Dock = DockStyle.Fill,
                };
                powers[i] = new PictureBox
                {
                    Height = 0,
                    BackColor = System.Drawing.SystemColors.ActiveCaption,
                    Dock = DockStyle.Bottom
                };

                panel.Controls.Add(powers[i]);
                chartTable.Controls.Add(panel, i + 1, 1);
                containerPanels[i] = panel;
            }
            
            this.predictor = predictor;
            this.repository = repository;
            generatedModelsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "models");
        }

        public sealed override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        private void FocusChordPlayedAtTime(double milliseconds)
        {
            label1.Text = $@"Audio played up to {milliseconds} ms";
            milliseconds = Math.Round(milliseconds);

            playedChord = Array.BinarySearch(chordsIntervals, (int)milliseconds);
            if(playedChord < 0)
            {
                playedChord = ~playedChord;
            }

            if (playedChord < chordButtons.Length)
            {
                chordButtons[playedChord].BackColor = HighlightColor;

                if (doAutoScroll.Checked)
                {
                    flowLayoutPanel1.ScrollControlIntoView(
                        chordButtons[playedChord]);
                }

                bigChordLabel.Text = chordButtons[playedChord].Text;
                UpdatePcpChart(chordsPredicted[playedChord]);
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
            (sampleRate, samples) = await Task.Run(() => Profiling.GetSamples(filePath));

            chordsPredicted = await Task.Run(() =>
                predictor.GetPredictionWithBorderDetection(samples, sampleRate,
                    windowInMs, 100, chordProcessingProgress)
            );

            chordsIntervals = new int[chordsPredicted.Length];
            chordsIntervals[0] = chordsPredicted[0].DurationInMs();
            for(int i = 1; i < chordsIntervals.Length; i++)
            {
                chordsIntervals[i] = chordsIntervals[i - 1] + chordsPredicted[i].DurationInMs();
            }


            sw.Stop();

            progressLabel.Text =
                $@"Chords computed successfully Elapsed={sw.Elapsed.TotalMilliseconds}";

            ShowChordButtons();
            PlayNewAudioFile(filePath);
        }

        private async void PlayNewAudioFile(string filePathToLoad)
        {
            audioPlayer?.Dispose();

            audioPlayer = await Task.Run(() =>
                new AudioPlayer.AudioPlayer(filePathToLoad, audioPlayProgress));
            audioPlayer.Play();
        }

        private void ShowChordButtons()
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
                    Text = chord.Name,
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
                    audioPlayer.SetPositionInMs(buttonIndex == 0 ? 0 : chordsIntervals[buttonIndex - 1] + 1);
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

        private void CorrectChordButton_Click(object sender, EventArgs e)
        {
            chordButtons[playedChord].Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(correctedChordTextBox.Text);
            chordButtons[playedChord].Font = new Font(chordButtons[playedChord].Font, FontStyle.Italic);

            bigChordLabel.Text = chordButtons[playedChord].Text;

            var biggestButtonWidth = 0;
            var biggestButtonHeight = 0;

            for (var i = 0; i < chordButtons.Length; i++)
            {
                var button = chordButtons[i];
                biggestButtonWidth = Math.Max(biggestButtonWidth, button.PreferredSize.Width);
                biggestButtonHeight = Math.Max(biggestButtonHeight, button.PreferredSize.Height);
            }

            foreach (var button in chordButtons)
            {
                button.Width = biggestButtonWidth;
                button.Height = biggestButtonHeight;
            }


            var newChord = new Chord(chordsPredicted[playedChord].Samples, chordsPredicted[playedChord].SampleRate,
                chordButtons[playedChord].Text, chordsPredicted[playedChord].Pcp);

            Task.Run(() =>
            {
                repository.SaveChord(newChord);
            });
        }

        private async void ChangeModel(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = generatedModelsDirectory,
                Filter = @"Chord Model (*.model;*.onnx)|*.model;*.onnx",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newModelName = openFileDialog.FileName;

            if(newModelName.Split(".")[^1].Equals("onnx"))
            {
                // TODO add propper change of onnx model
                predictor = new ClassicPredictor();
            } else
            {
                predictor = new AutoMlPredictor(newModelName);
            }


            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Stop();
            await CalculateChords();
        }

        private async void RetrainButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() => Chords.MachineLearning.AutoMlModelCreation.CreateModelGivenInitialDataAndStoredChordsFolder(
                "./Resources/trainData.csv", "./Resources/testData.csv", "./storedChords/", (uint)trainSeconds.Value, "./models/",
               trainProgress
            ));
        }

        private void UpdatePcpChart(Chord chord) {
            for(int i = 0; i < chord.Pcp.Length; i++)
            {
                var computedHeight = chord.Pcp[i] * containerPanels[i].Height;
                powers[i].Height = (int)Math.Round(computedHeight);
            }
        }
    }
}
