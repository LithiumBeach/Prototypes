using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Localization
{
    public static class LocalizationDataManager
    {
        internal static string s_InvalidID = "[INVALID]";
        internal static List<int> s_Ids;
        internal static CultureInfo[] s_Cultures =
        {
            CultureInfo.GetCultureInfo("en"),
            CultureInfo.GetCultureInfo("fr"),
            CultureInfo.GetCultureInfo("es")
        };

        internal static void InitializeIDs()
        {
            //stub
            s_Ids = new List<int>();
        }

        //find the next invalid id.
        internal static int GetNextUniqueID()
        {
            //nevative values are reserved for invalid, start at 0.
            int iterID = 0;

            while (iterID < int.MaxValue)
            {
                //if the key doesn't exist
                if (!s_Ids.Contains(iterID))
                {
                    //this is the next key
                    return iterID;
                }
                iterID++;
            }
            MessageBox.Show("uh oh.. all the keys seems to be taken. invalid key -1 will be returned.");
            return -1;
        }

        internal static List<List<LocalizationData>> ConvertToData(DataGridView mainDataGridView)
        {
            List<List<LocalizationData>> l = new List<List<LocalizationData>>();
            //first column is ids, start with i=1 (first language col)
            for (int languageIndex = 1; languageIndex < mainDataGridView.Columns.Count; languageIndex++)
            {
                l.Add(new List<LocalizationData>());
                for (int entryIndex = 0; entryIndex < mainDataGridView.Rows.Count; entryIndex++)
                {
                    l[languageIndex-1].Add(
                        new LocalizationData
                        {
                            m_ID = (int)mainDataGridView.Rows[entryIndex].Cells[0].Value,
                            m_Text = (string)mainDataGridView.Rows[entryIndex].Cells[languageIndex].Value
                        } );
                }
            }
            return l;
        }
    }
}
