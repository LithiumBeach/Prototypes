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
            LoadFromJson();
            LoadFromJson();
        }

        private void InitializeColumns()
        {
            MainDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //populate columns with languages defined in LocalizationDataManager.s_Cultures
            //TODO: user-inputted s_Cultures.
            //TODO: user-deletable columns (updating s_Cultures)
            for (int i = 0; i < LocalizationDataManager.s_Cultures.Length; i++)
            {
                MainDataGridView.ColumnCount++;
                MainDataGridView.Columns[i + 1].HeaderCell.Value = LocalizationDataManager.GetCultureNameAtIndex(i);
            }

            //Listen for new row
            MainDataGridView.RowsAdded += OnAddRow;
            //Listen for right before a row is deleted (while we still have that row's data)
            MainDataGridView.UserDeletingRow += OnBeforeDeleteRow;

            //set first row's id to 1
            //OnAddRow(null, new DataGridViewRowsAddedEventArgs(0, 0));
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

            for (int languageIndex = 0; languageIndex < LocalizationDataManager.s_Cultures.Length; languageIndex++)
            {
                string path = "..\\" +
                              LocalizationDataManager.s_FilenamePrefix +
                              LocalizationDataManager.GetCultureNameAtIndex(languageIndex) +
                              LocalizationDataManager.s_FilenameExtension;
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
            LoadFromJson();
        }

        private void LoadFromJson()
        {
            List<List<LocalizationData>> datas = LocalizationDataManager.ConvertFromData(MainDataGridView);
            for (int i = 0; i < datas.Count; i++)
            {
                for (int j = 0; j < datas[i].Count; j++)
                {
                    if (MainDataGridView.Rows.Count < j+1)
                    {
                        MainDataGridView.RowCount++;
                    }
                    MainDataGridView.Rows[j].Cells[i + 1].Value = datas[i][j].m_Text;
                    MainDataGridView.Rows[j].Cells[0].Value = datas[i][j].m_ID;

                    if (!LocalizationDataManager.s_Ids.Contains(datas[i][j].m_ID))
                    {
                        LocalizationDataManager.s_Ids.Add(datas[i][j].m_ID);
                    }
                }
            }
        }
    }
}
