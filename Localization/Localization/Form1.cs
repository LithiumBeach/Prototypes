using Microsoft.WindowsAPICodePack.Dialogs;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LocalizationDataManager.InitializeIDs();

            MainDataGridView.ClearSelection();

            InitializeColumns();

            if (!LoadFromJson(LocalizationDataManager.s_CurrentSaveDirectory))
            {
                LocalizationDataManager.InitializeIDs();
                //make one default row.
                int newID = LocalizationDataManager.GetNextUniqueID();
                LocalizationDataManager.s_Ids.Add(newID);
                MainDataGridView.Rows[0].SetValues(new object[] { newID });
            }

        }

        private void InitializeColumns()
        {
            MainDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            MainDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            MainDataGridView.AllowUserToAddRows = true;
            MainDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //populate columns with languages defined in LocalizationDataManager.s_Cultures
            //TODO: user-inputted s_Cultures.
            //TODO: user-deletable columns (updating s_Cultures)
            for (int i = 0; i < LocalizationDataManager.s_Cultures.Length; i++)
            {
                MainDataGridView.ColumnCount++;
                MainDataGridView.Columns[i + 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                MainDataGridView.Columns[i + 1].HeaderCell.Value = LocalizationDataManager.GetCultureNameAtIndex(i);
            }

            //Listen for new row
            MainDataGridView.RowsAdded += OnAddRow;
            //Listen for right before a row is deleted (while we still have that row's data)
            MainDataGridView.UserDeletingRow += OnBeforeDeleteRow;
        }

        //on a multi-row delete, this is called once for each row.
        private void OnBeforeDeleteRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            LocalizationDataManager.s_Ids.Remove((int)e.Row.Cells[0].Value);
        }

        private void OnAddRow(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!b_IsLoadingFromJson)
            {
                int newID = LocalizationDataManager.GetNextUniqueID();
                LocalizationDataManager.s_Ids.Add(newID);
                MainDataGridView.Rows[e.RowIndex].SetValues(new object[] { newID });
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAllLanguages(LocalizationDataManager.s_CurrentSaveDirectory, false);
        }

        private void SaveAllLanguages(string _pathPrefix, bool _confirmOverwritePopup)
        {
            //first list is per language, nested list is all the datas.
            List<List<LocalizationData>> datas = LocalizationDataManager.ConvertToData(MainDataGridView);

            for (int languageIndex = 0; languageIndex < LocalizationDataManager.s_Cultures.Length; languageIndex++)
            {
                string path = _pathPrefix +
                              LocalizationDataManager.s_FilenamePrefix +
                              LocalizationDataManager.GetCultureNameAtIndex(languageIndex) +
                              LocalizationDataManager.s_FilenameExtension;
                if (File.Exists(path))
                {
                    if (_confirmOverwritePopup)
                    {
                        DialogResult r = MessageBox.Show("Overwrite existing files?", "", MessageBoxButtons.OKCancel);
                        if (r == DialogResult.Yes)
                        {
                            File.Delete(path);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        File.Delete(path);
                    }
                }

                //https://stackoverflow.com/questions/16921652/how-to-write-a-json-file-in-c
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, datas[languageIndex]);
                }
            }

            //update current save directory to this if everything went well
            LocalizationDataManager.s_CurrentSaveDirectory = _pathPrefix;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = OpenCommonFileDialog();
            if (path != "")
            {
                LoadFromJson(path);
            }
        }

        private bool b_IsLoadingFromJson = false;
        private bool LoadFromJson(string _pathPrefix)
        {
            LocalizationDataManager.InitializeIDs();
            b_IsLoadingFromJson = true;
            List<List<LocalizationData>> datas = LocalizationDataManager.ConvertFromData(_pathPrefix, MainDataGridView);

            //determine the row count BEFORE adding any data.
            int numRows = 0;
            for (int i = 0; i < datas.Count; i++)
            {
                numRows = Math.Max(numRows, datas[i].Count);
            }
            MainDataGridView.RowCount = numRows;

            for (int i = 0; i < datas.Count; i++)
            {
                for (int j = 0; j < datas[i].Count; j++)
                {
                    MainDataGridView.Rows[j].Cells[i + 1].Value = datas[i][j].m_Text;
                    MainDataGridView.Rows[j].Cells[0].Value = datas[i][j].m_ID;

                    //update bank of used ids.
                    if (!LocalizationDataManager.s_Ids.Contains(datas[i][j].m_ID))
                    {
                        LocalizationDataManager.s_Ids.Add(datas[i][j].m_ID);
                    }

                }
            }
            b_IsLoadingFromJson = false;

            //no data if failed
            return datas.Count > 0;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedPath = OpenCommonFileDialog();
            if (selectedPath != "")
            {
                //Save Here!
                SaveAllLanguages(selectedPath + "\\", true);
            }
        }

        private string OpenCommonFileDialog()
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                var folderSelectorDialog = new CommonOpenFileDialog();
                folderSelectorDialog.EnsureReadOnly = true;
                folderSelectorDialog.IsFolderPicker = true;
                folderSelectorDialog.AllowNonFileSystemItems = false;
                folderSelectorDialog.Multiselect = false;
                folderSelectorDialog.EnsurePathExists = true;
                folderSelectorDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
                folderSelectorDialog.Title = "Project Location";
                //code hangs here
                CommonFileDialogResult result = folderSelectorDialog.ShowDialog();
                //resumes here
                if (result == CommonFileDialogResult.Ok)
                {
                    string selectedPath = folderSelectorDialog.FileName;
                    return selectedPath + "\\";
                }
            }
            return "";
        }
    }
}
