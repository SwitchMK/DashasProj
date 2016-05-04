using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace DashaProj
{
    public partial class ProjectForm : Form
    {
        public ProjectForm()
        {
            InitializeComponent();
            TotalTime = new TimeSpan(0, 0, 0);
        }

        public List<DataProvider> loadedData { get; set; }
        DataProvider dataProvider = new DataProvider();
        public static List<Form> mainProject = new List<Form>();

        private TimeSpan TotalTime { get; set; }
        string FileName { get; set; } = string.Empty;

        private void startButton_Click(object sender, EventArgs e)
        {
            ActivateFields();
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            dataProvider.Input = inputTextBox.Text.Split(' ').ToList();
            dataProvider.Output = ReturnNewSequence(dataProvider.Input);
            outputTextBox.Text = String.Join(" ", dataProvider.Output);
        }

        List<string> ReturnNewSequence(List<string> inputSequence)
        {
            List<string> outputSequence = new List<string>();

            for (int i = 0; i < inputSequence.Count; i++)
            {
                if (i % 5 == 0)
                {
                    outputSequence.Add(inputSequence[i]);
                }
            }

            return outputSequence;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            DeactivateFields();
        }

        void ResetAllFields()
        {
            inputTextBox.Text = "";
            outputTextBox.Text = "";
        }

        void ActivateFields()
        {
            inputTextBox.Enabled = true;
            actionButton.Enabled = true;
            resetButton.Enabled = true;
            waitInputTimer.Start();
        }

        void DeactivateFields()
        {
            ResetAllFields();
            FileName = string.Empty;
            inputTextBox.Enabled = false;
            actionButton.Enabled = false;
            resetButton.Enabled = false;
            waitInputTimer.Stop();
        }

        private void duration_Tick(object sender, EventArgs e)
        {
            TotalTime += new TimeSpan(0, 0, 1);
            durationLabel.Text = TotalTime.ToString();
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            duration.Start();
            mainProject.Add(this);
        }

        private void waitInputTimer_Tick(object sender, EventArgs e)
        {
            waitInputTimer.Stop();
            DialogResult result =
            MessageBox.Show("Enter some data to input field.", 
                "Warning", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                waitInputTimer.Start();
            }
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (inputTextBox.Text != string.Empty)
            {
                waitInputTimer.Stop();
            }
            else
            {
                waitInputTimer.Start();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeactivateFields();

            if (dataProvider.Input != null)
            {
                dataProvider.Input.Clear();
            }

            if (dataProvider.Output != null)
            {
                dataProvider.Output.Clear();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataProvider.Input != null && dataProvider.Output != null)
            {
                var saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDataToFile(saveFileDialog.FileName);
                    FileName = saveFileDialog.FileName;
                }
            }
            else
            {
                MessageBox.Show("You don't have needed data to save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SaveDataToFile(string fileName)
        {
            if (File.Exists(fileName) &&
                (new FileInfo(fileName).Length != 0))
            {
                var loadedJson = File.ReadAllText(fileName);
                loadedData = JsonConvert.DeserializeObject<List<DataProvider>>(loadedJson);
                dataProvider.Key = loadedData.Count;
                loadedData.Add(dataProvider);
            }
            else
            {
                loadedData = new List<DataProvider>();
                loadedData.Add(dataProvider);
            }

            var json = JsonConvert.SerializeObject(loadedData);
            File.WriteAllText(fileName, json);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileName != string.Empty)
            {
                SaveDataToFile(FileName);
            }
            else
            {
                saveAsToolStripMenuItem.PerformClick();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var loadData = new LoadData();

                string json = File.ReadAllText(openFileDialog.FileName);
                loadedData = JsonConvert.DeserializeObject<List<DataProvider>>(json);
                loadData.loadedData = loadedData;

                loadData.Show();
            }
        }
    }
}
