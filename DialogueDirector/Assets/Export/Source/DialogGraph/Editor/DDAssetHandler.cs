using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace dd
{
    public class DDAssetHandler : MonoBehaviour
    {
        //TODO: PUT THIS INTO A NEW CLASS: MyAssetHandler
        //TODO: do the same thing for DialogGraphDatas (not the editor type)
        [OnOpenAsset(1)]
        public static bool step1(int instanceID, int line)
        {
            UnityEngine.Object o = EditorUtility.InstanceIDToObject(instanceID);
            string name = o.name;
            System.Type t = o.GetType();
            if (t == typeof(DialogGraphData))
            {
                DialogSequenceWindow.OpenWindow();
                EditorWindow.GetWindow<DialogSequenceWindow>().Initialize((o as DialogGraphData));
            }
            return false; // we did not handle the open
        }
    }
}