using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace dd
{
    public class DDAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            DialogSequenceWindow dsw = EditorWindow.GetWindow<DialogSequenceWindow>();
            if (dsw != null && dsw.m_Data != null)
            {
                dsw.OnUnitySave();
                EditorUtility.SetDirty(dsw.m_Data);
            }

            return paths;
        }

    }
}