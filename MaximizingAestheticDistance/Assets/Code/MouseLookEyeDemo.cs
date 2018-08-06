using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

namespace dd
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MouseLookEyeDemo : MonoBehaviour
    {
        private SpriteRenderer m_Renderer;
        public SpriteAtlas m_Atlas0 = null;
        public SpriteAtlas m_Atlas1 = null;
        private Sprite[] m_AtlasArray;

        public void Awake()
        {
            m_Renderer = GetComponent<SpriteRenderer>();
            m_AtlasArray = new Sprite[297];
            int numSprites0 = m_Atlas0.GetSprites(m_AtlasArray);
            Sprite[] atlasArrayTemp = new Sprite[63];
            int numSprites1 = m_Atlas1.GetSprites(atlasArrayTemp);

            Array.Sort(m_AtlasArray, new EyeCompare());
            Array.Sort(atlasArrayTemp, new EyeCompare());
            m_AtlasArray = m_AtlasArray.Concat(atlasArrayTemp).ToArray();

            Debug.Log("Sprites In Atlas 0: " + numSprites0);

            StartCoroutine("tempDoIt");
        }

        private IEnumerator tempDoIt()
        {
            int tmpIndex = 0;
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                m_Renderer.sprite = m_AtlasArray[tmpIndex];
                tmpIndex++;
                if (tmpIndex >= m_AtlasArray.Length)
                {
                    tmpIndex = 0;
                }
            }
        }
    }
}