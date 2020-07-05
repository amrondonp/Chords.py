using Chords.Profiling;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void Button1_Click(object sender, EventArgs e)
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
            });

            var fileContent = await Task.Run(() => LongAudioProfiling.GetPredictionWithProgressReport(openFileDialog.FileName, progress));
            this.textBox1.Text = string.Join(" ", fileContent);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
