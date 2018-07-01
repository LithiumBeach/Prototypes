using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization
{
    public class LocalizationData
    {
        public LocalizationData()
        { m_ID = -1; m_Text = LocalizationDataManager.s_InvalidID; }

        public LocalizationData(int id, string text)
        { m_ID = id; m_Text = text; }

        public int m_ID;
        public string m_Text;
    }
}
