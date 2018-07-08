using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    /// <summary>
    /// data link [ID, text]
    /// all IDs must be UNIQUE and HUMAN-READABLE/WRITABLE
    /// no hash functions allowed
    /// ALL NEGATIVE IDs are RESERVED as INVALID
    /// for serialization with json.
    /// </summary>
    [System.Serializable]
    public class DialogData
    {
        public static readonly string c_InvalidTextString = "NOT LOCALIZED!";

        public DialogData() { m_ID = -1; m_Text = c_InvalidTextString; }
        public DialogData(int id, string text){ m_ID = id;  m_Text = text; }

        [OnValueChanged("HandleLocalizationIDChange")]
        public int m_ID;
        public string m_Text;

        private void HandleLocalizationIDChange()
        {
            m_Text = DialogDBSerializer.GetTextFromID(m_ID);
        }
    }
}