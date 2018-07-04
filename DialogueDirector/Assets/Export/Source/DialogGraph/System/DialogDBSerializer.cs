using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace dd
{
    ///<summary> serializes/deserializes DialogDatas </summary>
    public static class DialogDBSerializer
    {
        private const string c_TextLinesFilepath = "DialogLines/LocalizedText_";
        public static TextAsset m_TextLinesAsset;

        private static Dictionary<int, string> m_DialogDB;
        public static string GetTextFromID(int id)
        {
            if (m_DialogDB.ContainsKey(id))
            {
                string text = m_DialogDB[id];

                //make sure string we did find is valid (not left blank)
                return (text != null && text != "") ?
                    text : DialogData.c_InvalidTextString;
            }
            return DialogData.c_InvalidTextString;
        }

        //loads json with all [ID, text] into the dictionary
        public static void LoadDialogLines(CultureInfo culture)
        {
            //construct filepath
            string str_lang = culture.DisplayName;
            string filepath = c_TextLinesFilepath + str_lang;

            //if we haven't loaded a DialogLines file before.
            if (m_TextLinesAsset != null)
            {
                if (m_TextLinesAsset.name != filepath)
                {
                    //we are switching languages, unload the old language file from memory
                    Resources.UnloadAsset(m_TextLinesAsset);
                }
                else
                {
                    //the file is already loaded.
                    return;
                }
            }

            //load new file
            m_TextLinesAsset = Resources.Load<TextAsset>(filepath);
            if (m_TextLinesAsset == null)
            {
                Debug.LogError(filepath + " can't be loaded! Doesn't exist!");
                return;
            }
            //set name to filepath
            m_TextLinesAsset.name = filepath;
            //make sure we successfully loaded the file.
            Debug.Assert(m_TextLinesAsset != null, "error loading DialogLines file! Most likely you have not supported this language");

            //initialize dictionary
            m_DialogDB = new Dictionary<int, string>();

            //parse json, output as DialogData[]
            DialogData[] datas = JsonHelper.FromJsonDialogData(m_TextLinesAsset.text);

            //if we got this far, and datas is null, there simply is no data. early exit.
            if (datas == null)
                return;

            //populate dictionary, omit DialogData struct. Simply [id, text]
            for (int i = 0; i < datas.Length; i++)
            {
                Debug.Assert(!m_DialogDB.ContainsKey(datas[i].m_ID), "ERROR! There is a duplicate id!! ID=" + datas[i].m_ID.ToString());

                m_DialogDB.Add(datas[i].m_ID, datas[i].m_Text);
            }
        }
    }
}