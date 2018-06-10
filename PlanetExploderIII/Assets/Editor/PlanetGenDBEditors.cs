using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    using UnityEditor;

    [CustomEditor(typeof(CoreDatabase))]
    public class CoreDatabaseEditor : Editor
    {
        CoreDatabase m_Target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            m_Target = target as CoreDatabase;
            if (m_Target == null)
            {
                return;
            }
            if(GUILayout.Button("Refresh Database"))
            {
                //finally, populate the database
                m_Target.PopulateDB(Resources.LoadAll<CoreData>("PlanetCompositions/CoreToSurface"));
            }
        }
    }

    [CustomEditor(typeof(SurfaceDatabase))]
    public class SurfaceDatabaseEditor : Editor
    {
        SurfaceDatabase m_Target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            m_Target = target as SurfaceDatabase;
            if (m_Target == null)
            {
                return;
            }
            if (GUILayout.Button("Refresh Database"))
            {
                //finally, populate the database
                m_Target.PopulateDB(Resources.LoadAll<SurfaceData>("PlanetCompositions/SurfaceToAtmosphere"));
            }
        }
    }

    [CustomEditor(typeof(AtmosphereDatabase))]
    public class AtmosphereDatabaseEditor : Editor
    {
        AtmosphereDatabase m_Target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            m_Target = target as AtmosphereDatabase;
            if (m_Target == null)
            {
                return;
            }
            if (GUILayout.Button("Refresh Database"))
            {
                //finally, populate the database
                m_Target.PopulateDB(Resources.LoadAll<AtmosphereData>("PlanetCompositions/Atmospheres"));
            }
            //if (GUILayout.Button("Clear Database"))
            //{
            //    //finally, populate the database
            //    m_Target.Clear();
            //}

        }
    }
}
