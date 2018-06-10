using UnityEngine;

namespace pe
{
    public class CmdList : BaseCommand
    {
        public override void Initialize()
        {
            m_CommandStringsRef = CommandStrings.LS;
        }


        public override bool Evaluate(string[] _params)
        {
            if (InfoManager.Instance.FocusSolarSystem != null)
            {
#if UNITY_EDITOR
                //still deterministic, just needs to update if we regenerated seeds in editor.
                InfoManager.Instance.InitializeSolarSystems(); 
#endif
                LogPlanets();
            }
            else if (InfoManager.Instance.FocusGalaxy != null)
            {
                LogSolarSystems();
            }
            else
            {
                return false;
            }

            return true;
        }


        internal void LogSolarSystems()
        {
            string[] names = InfoManager.Instance.GetAllSolarSystemNamesInGalaxy(InfoManager.Instance.FocusGalaxy.m_GalaxyInfo);
            float[] sizes = InfoManager.Instance.GetAllSolarSystemSizesInGalaxy(InfoManager.Instance.FocusGalaxy.m_GalaxyInfo);
            for (int i = 0; i < names.Length; i++)
            {
                PlayerConsoleUI.Instance.PushToLog(new LogData(names[i] + " | size: " + sizes[i].ToString() + Utils.c_MassUnitSymbol, TextAnchor.MiddleRight));
            }
        }
        internal void LogPlanets()
        {
            string[] names = InfoManager.Instance.GetAllPlanetNamesInSolarSystem(InfoManager.Instance.FocusSolarSystem);
            float[] sizes = InfoManager.Instance.GetAllPlanetMassesInSolarSystem(InfoManager.Instance.FocusSolarSystem);
            for (int i = 0; i < names.Length; i++)
            {
                PlayerConsoleUI.Instance.PushToLog(new LogData(names[i] + " | size: " + sizes[i].ToString() + Utils.c_MassUnitSymbol, TextAnchor.MiddleRight));
            }
        }
    }
}
