using Newtonsoft.Json;
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

namespace Localization
{
    public partial class Form1 : Form
    {
        public int NumRows { get { return MainDataGridView.Rows.Count; } }

        public Form1()
        {
            InitializeComponent();

            LocalizationDataManager.InitializeIDs();

            InitializeColumns();
        }

        private void InitializeColumns()
        {
            MainDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //Listen for new row
            MainDataGridView.RowsAdded += OnAddRow;
            //Listen for right before a row is deleted (while we still have that row's data)
            MainDataGridView.UserDeletingRow += OnBeforeDeleteRow;

            //set first row's id to 1
            OnAddRow(null, new DataGridViewRowsAddedEventArgs(0, 0));
        }

        //on a multi-row delete, this is called once for each row.
        private void OnBeforeDeleteRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            LocalizationDataManager.s_Ids.Remove((int)e.Row.Cells[0].Value);
        }

        private void OnAddRow(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int newID = LocalizationDataManager.GetNextUniqueID();
            LocalizationDataManager.s_Ids.Add(newID);
            MainDataGridView.Rows[e.RowIndex].SetValues(new object[] { newID });
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //first list is per language, nested list is all the datas.
            List<List<LocalizationData>> datas = LocalizationDataManager.ConvertToData(MainDataGridView);

            for (int languageIndex = 0; languageIndex < datas.Count; languageIndex++)
            {
                string path = "..\\TestLanguageFile" + languageIndex.ToString() + ".json";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                //https://stackoverflow.com/questions/16921652/how-to-write-a-json-file-in-c
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, datas[languageIndex]);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
