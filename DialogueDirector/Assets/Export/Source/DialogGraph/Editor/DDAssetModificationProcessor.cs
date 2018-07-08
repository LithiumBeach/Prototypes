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
            if (dsw != null)
            {
                dsw.OnUnitySave();
            }


            return paths;
        }
    }
}