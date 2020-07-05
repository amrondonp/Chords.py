using Chords.Profiling;
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

namespace Chords
{
    public partial class Form1 : Form
    {
        private Label[] chordLabels;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //for (int i = 0; i < 30; i++)
            //{
            //    var label = new Label
            //    {
            //        Text = "Em",
            //        Width = 100,
            //            Height = 50,
            //            //AutoSize = true,
            //            Font = new Font(this.Font.FontFamily, 15),
            //            BorderStyle = BorderStyle.FixedSingle,

            //    };

            //    this.flowLayoutPanel1.Controls.Add(label);
            //}
        }

        private async void openFileClicked(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Audio files (*.wav)|*.wav";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            progressBar1.Maximum = 100;
            progressBar1.Step = 1;

            var progress = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                progressBar1.Value = v;
                progressLabel.Text = "Computing chords... " + v + " %";
            });

            var playProgress = new Progress<double>(milliseconds =>
            {
                label1.Text = "Audio played up to " + milliseconds + " ms";
                double window = 500;
                int playedChord = (int)Math.Floor(milliseconds / window);
                foreach(Label label in chordLabels)
                {
                    label.BackColor = progressLabel.BackColor;
                }
                if(playedChord < this.chordLabels.Length)
                {
                    this.chordLabels[playedChord].BackColor = Color.Aqua;
                }
            });

            var fileContent = await Task.Run(() => LongAudioProfiling.GetPredictionWithProgressReport(openFileDialog.FileName, progress));            
            progressLabel.Text = "Chords computed successfully";

            
            this.flowLayoutPanel1.Controls.Clear();
            chordLabels = new Label[fileContent.Length];
            for(int i = 0; i < fileContent.Length; i++)
            {
                var chord = fileContent[i];
                var label = new Label
                {
                    Text = chord,
                    Width = 100,
                    Height = 50,
                    Font = new Font(this.Font.FontFamily, 15),
                    BorderStyle = BorderStyle.FixedSingle,
                };

                this.flowLayoutPanel1.Controls.Add(label);
                chordLabels[i] = label;
            }

            AudioPlayer.AudioPlayer audioPlayer = new AudioPlayer.AudioPlayer(openFileDialog.FileName, playProgress);
            audioPlayer.Play();
        }

        
    }
}
