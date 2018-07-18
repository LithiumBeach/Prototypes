using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace dd
{
    public class DDAssetHandler : MonoBehaviour
    {
        [OnOpenAsset(1)]
        public static bool step1(int instanceID, int line)
        {
            UnityEngine.Object o = EditorUtility.InstanceIDToObject(instanceID);
            string name = o.name;
            System.Type t = o.GetType();
            if (t == typeof(DialogGraphData))
            {
                DialogGraphWindow.OpenWindow();
                EditorWindow.GetWindow<DialogGraphWindow>().Initialize((o as DialogGraphData));
            }
            return false; // we did not handle the open
        }

        [MenuItem("Assets/Create/DialogDirector/Graph", false, priority = 281)]
        public static void CreateDialogGraphDataAsset()
        {
            DialogGraphData asset = ScriptableObject.CreateInstance<DialogGraphData>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/DialogGraphData" + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            asset.OpenDialogGraph();//this will call Initialize
        }
    }
}