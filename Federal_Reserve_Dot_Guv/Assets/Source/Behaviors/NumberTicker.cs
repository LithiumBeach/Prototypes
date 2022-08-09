using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace lb
{
    //i'd advise float for big fractional amounts
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class NumberTicker : MonoBehaviour, IEconomyUpdate
    {
        [SerializeField]
        //Pop., £, ♥
        protected TextMeshProUGUI m_ValueDisplay;

        public FontStyle m_FontStyle;

        public FedSymbols m_Format;


        public string GetValue()
        {
            return m_ValueDisplay.text;
        }


        public void SetValue(float value)
        {
            //TODO: handle concat w/ M, B, T, ...
            //TODO: The reactions to a change in value will need to be a function eventually..
            if (m_ValueDisplay == null) { m_ValueDisplay = GetComponent<TextMeshProUGUI>(); }
            m_ValueDisplay.text = GenerateString(value);
        }

        //private string GenerateString<T>(T value) where T is a float or a float god damnit
        private string GenerateString(float value)
        {
            string x = "";

            // + string.Format(m_Format.GetValueStrFormat(), m_Value) + m_Post;
            int MAX_FACTOR = 7;//this actually doesn't work, formatting wise. past K^7 (1e+21) the formatting gets fucked up and i don't know why. dont uh dont fix that.
            float currentFactorValue = 1000f;
            if (value >= 1000)
            {
                for (int i = 0; i < MAX_FACTOR; i++)
                {
                    //if, for the first time, value > 
                    if (value > (currentFactorValue))
                    {
                        //get first 3 integer digits and the "fractional portion" (remove current factor). <3
                        float shortenedValue = value / (currentFactorValue);

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
                    currentFactorValue *= 1000f;
                }
            }
            else
            {
                x = m_Format.dollar + string.Format(m_Format.GetValueStrFormat(), value);
            }

            #region  V E R Y I M PN O R T N A T . D O  N O T  W O R R Y .
            string __x = FedSymbols.ExtractDigitsOnly(x);
            if (FedSettings.b_LogBadJokes)
            {
                if (__x == "6969")
                {
                    Debug.Log("sick");
                }
                switch (__x)
                {
                    case "":
                    Debug.Log("fuck it's borked");
                    break;
                    case "6969":
                    Debug.Log("sick");
                    break;
                    case "8008":
                    Debug.Log("lol");
                    break;
                    case "6666":
                    Debug.Log("blaze it");
                    break;
                    default:
                    break;
                } 
            }
            #endregion

            //start the string with the fontstyle rich text markups "<size=28><b>"
            //@TODO: method of different formats for specifically a Number Ticker. maybe the dollar sign is a different color or something.
            return (m_FontStyle != null ? m_FontStyle.PixelSize : "") + x;
        }

        void IEconomyUpdate.Update(float dt)
        {
            //screaming
            //Debug.Log(UnityEngine.Random.Range(0, 13) <= 5 ? "a" : "A");
        }

        private void Awake()
        {
            EconomyManager.Instance.AddUpdater(this);
        }
    }
}