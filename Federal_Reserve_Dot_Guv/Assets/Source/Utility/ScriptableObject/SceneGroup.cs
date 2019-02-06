using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace util
{
    [CreateAssetMenu(fileName="Scene Group", menuName="Data/Scene Group")]
    public class SceneGroup : ScriptableObject //@TODO: how do they not have it templated? do the same thing for singletonbehavior
    {
        public List<SceneField> scenes;

        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonSizes.Gigantic)]
        public void OpenScenes()
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                //TODO: error: please put your scene files in the Assets/Scenes/ directory.
                //TODO: also make the directory, and select it in the inspector.
#if UNITY_EDITOR
                //open single scene first (closing all currently open). then open the rest additively
                EditorSceneManager.OpenScene("Assets/Scenes/SceneFiles/" + scenes[i].SceneName + ".unity", i==0 ? OpenSceneMode.Single : OpenSceneMode.Additive);
#endif
            }
        }

    }
}