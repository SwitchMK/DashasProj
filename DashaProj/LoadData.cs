using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DashaProj
{
    public partial class LoadData : Form
    {
        public LoadData()
        {
            InitializeComponent();
        }

        public List<DataProvider> loadedData = new List<DataProvider>();
        public List<DataProvider> searchedData = new List<DataProvider>();


        private void LoadData_Load(object sender, EventArgs e)
        {
            Display(loadedData);
        }

        void Display(List<DataProvider> list)
        {
            data.Rows.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                data.Rows.Add();

                data.Rows[i].Cells[0].Value = list[i].Key.ToString();
                data.Rows[i].Cells[1].Value = String.Join(" ", list[i].Input);
                data.Rows[i].Cells[2].Value = String.Join(" ", list[i].Output);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            int key;
            if (searchTextBox.Text != string.Empty)
            {
                if (int.TryParse(searchTextBox.Text, out key))
                {
                    data.Rows.Clear();
                    searchedData.Clear();

                    foreach (var item in loadedData)
                    {
                        if (item.Key.Equals(key))
                        {
                            searchedData.Add(item);
                        }
                    }

                    Display(searchedData);
                }
                else
                {
                    MessageBox.Show("Incorrect input!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    searchTextBox.Text = string.Empty;
                }
            }
            else
            {
                Display(loadedData);
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            //ProjectForm.mainProject[0].input = String.Join(" ", loadedData.Where(d => d.Key.Equals(data.SelectedRows[0].Cells[0].Value.ToString())));
        }
    }
}
