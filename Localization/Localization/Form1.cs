using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            LocalizationData.InitializeIDs();

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
            LocalizationData.s_Ids.Remove((int)e.Row.Cells[0].Value);
        }

        private void OnAddRow(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int newID = LocalizationData.GetNextUniqueID();
            LocalizationData.s_Ids.Add(newID);
            MainDataGridView.Rows[e.RowIndex].SetValues(new object[] { newID });
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
