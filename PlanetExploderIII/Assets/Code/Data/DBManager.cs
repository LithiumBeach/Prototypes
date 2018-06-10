using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using patterns;
using Sirenix.OdinInspector;

namespace pe
{
    public class DBManager : SingletonBehavior<DBManager>
    {
        [Required]
        public CoreDatabase CoreDB;
        [Required]
        public SurfaceDatabase SurfaceDB;
        [Required]
        public AtmosphereDatabase AtmosphereDB;
        [Required]
        public MatterDatabase MatterDB;

        protected override void OnAwake()
        {
            CoreDB.Initialize();
            SurfaceDB.Initialize();
            AtmosphereDB.Initialize();
        }

#if UNITY_EDITOR
        // Add a menu item named "Do Something" to MyMenu in the menu bar.
        [UnityEditor.MenuItem("PlanetExploder/Refresh Databases")]
        static void DoSomething()
        {
            Instance.CoreDB.PopulateDB(Resources.LoadAll<CoreData>("PlanetCompositions/CoreToSurface"));
            Instance.SurfaceDB.PopulateDB(Resources.LoadAll<SurfaceData>("PlanetCompositions/SurfaceToAtmosphere"));
            Instance.AtmosphereDB.PopulateDB(Resources.LoadAll<AtmosphereData>("PlanetCompositions/Atmospheres"));
        }
#endif
    }
}
