using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace dd
{
    ///<summary> serializes/deserializes DialogDatas </summary>
    public static class DialogDBSerializer
    {
        private const string c_TextLinesFilepath = "DialogLines/DialogLines_";
        public static TextAsset m_TextLinesAsset;

        private static Dictionary<int, string> m_DialogDB;
        public static string GetTextFromID(int id)
        {
            if (m_DialogDB.ContainsKey(id))
            {
                return m_DialogDB[id];
            }
            return DialogData.c_InvalidTextString;
        }

        //loads json with all [ID, text] into the dictionary
        public static void LoadDialogLines(SystemLanguage lang)
        {
            //construct filepath
            string str_lang = lang.ToString();
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
            //set name to filepath
            m_TextLinesAsset.name = filepath;
            //make sure we successfully loaded the file.
            Debug.Assert(m_TextLinesAsset != null, "error loading DialogLines file! Most likely you have not supported this language");

            //initialize dictionary
            m_DialogDB = new Dictionary<int, string>();

            //parse json, output as DialogData[]
            DialogData[] datas = JsonHelper.FromJson<DialogData>(m_TextLinesAsset.text);

            //if we got this far, and datas is null, there simply is no data. early exit.
            if (datas == null)
                return;

            //populate dictionary, omit DialogData struct. Simply [id, text]
            for (int i = 0; i < datas.Length; i++)
            {
                Debug.Assert(!m_DialogDB.ContainsKey(datas[i].m_LocalizationID), "ERROR! There is a duplicate id!! ID=" + datas[i].m_LocalizationID.ToString());

                m_DialogDB.Add(datas[i].m_LocalizationID, datas[i].m_Text);
            }
        }

        //export dictionary to json
        public static void SaveDialogLines(SystemLanguage lang)
        {
            //construct filepath
            string str_lang = lang.ToString();
            string filepath = c_TextLinesFilepath + str_lang;

            //the file is already loaded, use it.
            if(m_TextLinesAsset == null || m_TextLinesAsset.name != filepath)
            {
                //load new file
                m_TextLinesAsset = Resources.Load<TextAsset>(filepath);
                //set name to filepath
                m_TextLinesAsset.name = filepath;
                //make sure we successfully loaded the file.
                Debug.Assert(m_TextLinesAsset != null, "error loading DialogLines file! Most likely you have not supported this language");
            }

            //make new DialogData array to fill with dictionary values.
            DialogData[] ddArray = new DialogData[m_DialogDB.Count];

            int index = 0;
            //iterate through dictionary, making new DialogDatas.
            foreach(KeyValuePair<int, string> dialogEntry in m_DialogDB)
            {
                ddArray[index++] = new DialogData(dialogEntry.Key, dialogEntry.Value);
            }

            //convert array to json string
            string jsonString = JsonHelper.ToJson<DialogData>(ddArray);

            //finally, write to json file
            File.WriteAllText(AssetDatabase.GetAssetPath(m_TextLinesAsset), jsonString);
            EditorUtility.SetDirty(m_TextLinesAsset);
        }

        public static int AddDialogLine(string line)
        {
            int newID = GetNextUniqueID();
            m_DialogDB.Add(newID, line);
            return newID;
        }

        public static void RemoveDialogLine(int id)
        {
            m_DialogDB.Remove(id);
        }

        //find the next invalid id.
        public static int GetNextUniqueID()
        {
            //nevative values are reserved for invalid, start at 0.
            int iterID = 0;

            while (iterID < int.MaxValue)
            {
                //if the key doesn't exist
                if (!m_DialogDB.ContainsKey(iterID))
                {
                    //this is the next key
                    return iterID;
                }
                iterID++;
            }
            Debug.LogAssertion("uh oh.. all the keys seems to be taken. invalid key -1 will be returned.");
            return -1;
        }
    }
}