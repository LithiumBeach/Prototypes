using TMPro;
using UnityEngine;
using util;

//base class for financial values

namespace lb
{
    public class FinEntry : MonoBehaviour
    {
        //ui object refs
        public TextMeshProUGUI m_NameUI;
        public NumberTicker m_ValueUI;
        
        //font style
        public FontStyle m_NameFontStyle;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.OnValueChanged("OnDataChanged", includeChildren: true)]
        [Sirenix.Serialization.OdinSerialize]
#endif
        public FinEntryData m_Data;
#if UNITY_EDITOR
        void OnDataChanged()
        {
            m_ValueUI.SetValue(m_Data.m_Value);
            
            m_NameUI.SetText(m_NameFontStyle.PixelSize + m_Data.m_Name);
            UnityEditor.SceneView.RepaintAll();

            #region unity and textmeshpro are made for each other <3
            //heinous. i can't find a way to update the value in the viewport immediately, always stays a character
            //behind until unity.exe loses focus. a thrilling challenge i will not be beating.
            //UnityEditor.EditorUtility.SetDirty(m_NameUI.font);
            //UnityEditor.AssetDatabase.SaveAssets();
            #endregion
        }
#endif

        //this may need to be a (yeild) curve?
        //although maybe interest is calculated in the InterestAccum fn, and that can use a curve if it really wants to.
        //this may be unnecessary, for the base class, since more complex things can be used in inheriting classes
        //public float m_InterestRate;

        //float m_...Risk?

        public virtual void InterestAccum(float deltaTime)
        {
            
        }
    }
}