using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace lb
{
    //i'd advise decimal for big fractional amounts
    public class NumberTicker : MonoBehaviour, IEconomyUpdate
    {
        [SerializeField]
        //Pop., £, ♥
        protected Text m_ValueDisplay;

        [SerializeField]
        private decimal m_Value;
        [Sirenix.OdinInspector.OnValueChanged("OnValueValueChanged")]
        [SerializeField]
        private float __InspectorEditorValue__ = 0;
        //nonsense to update text in inspector
        private void OnValueValueChanged() { Value = (decimal)__InspectorEditorValue__; }
        public decimal Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                //@TODO: handle concat w/ M, B, T, ...
                //TODO: The reactions to a change in value will need to be a function eventually..
                //parameter N2 or N3
                m_Value = value;
                m_ValueDisplay.text = GenerateString();
            }
        }

        public FedSymbols m_Format;

        private string GenerateString()
        {
            string x = "";

            // + string.Format(m_Format.GetValueStrFormat(), m_Value) + m_Post;
            decimal MAX_FACTOR = 7;//this actually doesn't work, formatting wise. past K^7 (1,000^7) the formatting gets fucked up and i don't know why. have a go :(
            decimal currentFactorValue = 1000;
            if (m_Value >= 1000m)
            {
                for (int i = 0; i < MAX_FACTOR; i++)
                {
                    //if, for the first time, value > 
                    if (m_Value > (currentFactorValue))
                    {
                        //get first 3 integer digits and the "fractional portion" (remove current factor). <3
                        decimal shortenedValue = m_Value / (currentFactorValue);

                        if (i < FedSymbols.c_FactorShorthands.Length)
                        {
                            x = m_Format.dollar + string.Format(m_Format.GetValueStrFormat(), shortenedValue) + FedSymbols.c_FactorShorthands[i];
                        }
                        else
                        {
                            x = m_Format.dollar + string.Format(m_Format.GetValueStrFormat(), shortenedValue) + FedSymbols.c_FactorOverflowUnit + i.ToString();
                        }
                    }
                    else { break; }
                    currentFactorValue *= 1000m;
                }
            }
            else
            {
                x = m_Format.dollar + string.Format(m_Format.GetValueStrFormat(), m_Value);
            }
            return x;
        }

        void IEconomyUpdate.Update(float dt)
        {
            //screaming
            //Debug.Log(UnityEngine.Random.Range(0, 13) <= 5 ? "a" : "A");
        }

        private void Awake()
        {
            EconomyUpdateManager.Instance.AddUpdater(this);
        }
    }
}