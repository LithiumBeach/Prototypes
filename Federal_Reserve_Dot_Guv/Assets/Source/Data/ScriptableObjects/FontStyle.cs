using UnityEngine;
using util;

// a wrapper for rich text, for text that changes sometimes.
// TextMeshPro should probably support font size in pixels 
// (esp. since it supports RichText somehow) but here it is.
// add things as needed.

namespace lb
{
    [CreateAssetMenu(fileName="FontStyle", menuName="Fed/FontStyle", order=0)]
    public class FontStyle : ScriptableObject
    {
        //<size=28>
        [Range(9,100)]
        public int m_PixelSize = 28;
        public string PixelSize
        {
            get { return string.Format("<size={0}>", m_PixelSize); }
        }

    }
}