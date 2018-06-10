using System;
using patterns;
using UnityEngine;
using UnityEngine.UI;

namespace pe
{
    public class InfoUI : SingletonBehavior<InfoUI>
    {
        public RectTransform m_LogRoot;
        public Text m_InfoEntryPrefab;

        public RectTransform m_VisualRoot;

        protected override void OnAwake()
        {

        }

        protected override void OnUpdate(float dt)
        {

        }

        public void ClearLog()
        {
            int l = m_LogRoot.transform.childCount;
            for (int i = 0; i < l; i++)
            {
                Destroy(m_LogRoot.transform.GetChild(l - i - 1).gameObject);
            }
        }

        internal void Print(PlanetBehavior o)
        {
            if (o == null)
            {
                return;
            }

            //clear existing log
            ClearLog();

            bool isAlive = o.IsAlive;

            //name
            Text nameText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            nameText.text = o.name;
            nameText.fontSize += 8;

            //mass
            Text massText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            massText.text = "Σ mass: " + (!isAlive ? "-" : o.Mass.ToString() + PEConstants.c_MassUnitString);

            //energy
            Text epText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            epText.text = "Σ energy: " + (!isAlive ? "-" : o.Energy.ToString() + PEConstants.c_EnergyUnitString);

            //combustion energy threshold
            Text cetText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            cetText.text = "Σ combustion energy threshold: " + (!isAlive ? "-" : o.CombustionEnergyThreshold.ToString() + PEConstants.c_EnergyUnitString);

            //core
            Text coreText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            coreText.text = "core: " + (!isAlive ? "-" : o.m_Core.Composition.ToString());
            {
                //mass
                Text coreMassText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                coreMassText.text = "\tmass: " + (!isAlive ? "-" : o.m_Core.Mass.ToString() + PEConstants.c_MassUnitString);

                //energy
                Text coreEText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                coreEText.text = "\tΣ energy: " + (!isAlive ? "-" : o.m_Core.Energy.ToString() + PEConstants.c_EnergyUnitString);

                //combustion energy threshold
                Text coreETText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                coreETText.text = "\tΣ combustion energy threshold: " + (!isAlive ? "-" : o.m_Core.CombustionEnergyThreshold.ToString() + PEConstants.c_EnergyUnitString);
            }

            //surface
            Text surfaceText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            surfaceText.text = "surface: " + (!isAlive ? "-" : o.m_Surface.Composition.ToString());
            {
                //mass
                Text surfaceMassText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                surfaceMassText.text = "\tmass: " + (!isAlive ? "-" : o.m_Surface.Mass.ToString() + PEConstants.c_MassUnitString);

                //energy
                Text surfaceEText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                surfaceEText.text = "\tΣ energy: " + (!isAlive ? "-" : o.m_Surface.Energy.ToString() + PEConstants.c_EnergyUnitString);

                //combustion energy threshold
                Text surfaceETText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                surfaceETText.text = "\tΣ combustion energy threshold: " + (!isAlive ? "-" : o.m_Surface.CombustionEnergyThreshold.ToString() + PEConstants.c_EnergyUnitString);
            }

            //atmosphere
            Text atmosphereText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
            atmosphereText.text = "atmosphere: " + (!isAlive ? "-" : o.m_Atmosphere.Composition.ToString());
            {
                //mass
                Text atmosphereMassText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                atmosphereMassText.text = "\tmass: " + (!isAlive ? "-" : o.m_Atmosphere.Mass.ToString() + PEConstants.c_MassUnitString);

                //energy
                Text atmosphereEText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                atmosphereEText.text = "\tΣ energy: " + (!isAlive ? "-" : o.m_Atmosphere.Energy.ToString() + PEConstants.c_EnergyUnitString);

                //combustion energy threshold
                Text atmosphereETText = Instantiate(m_InfoEntryPrefab.gameObject, m_LogRoot).GetComponent<Text>();
                atmosphereETText.text = "\tΣ combustion energy threshold: " + (!isAlive ? "-" : o.m_Atmosphere.CombustionEnergyThreshold.ToString() + PEConstants.c_EnergyUnitString);
            }
        }

        public void ToggleConsole()
        {
            m_VisualRoot.gameObject.SetActive(!m_VisualRoot.gameObject.activeSelf);
        }
    }
}
