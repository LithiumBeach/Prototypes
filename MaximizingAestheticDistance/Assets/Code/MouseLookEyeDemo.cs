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

        //TODO: put these into a singleton manager
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
            tempLookForwardIndex = m_AtlasArray.Length - 21;
        }

        //TODO: Manage update function to update spriteframes at x intervals
        float fps = 1f / 16f;
        float updateTimer = 0f;
        int tempLookForwardIndex;
        int tempLookForwardDirection = 1;
        private void Update()
        {
            //TODO: ... you know what to do lol this is very temp
            updateTimer += Time.deltaTime;
            if (updateTimer < fps)
            {
                return;
            }
            else
            {
                updateTimer = 0f;
            }


            Vector2 mousePos = Input.mousePosition;

            //if left clicking
            if (Input.GetMouseButton(0))
            {
                //choose random frame from last 20 (staring forward)
                //m_Renderer.sprite = m_AtlasArray[UnityEngine.Random.Range(m_AtlasArray.Length - 21, m_AtlasArray.Length - 1)];
                m_Renderer.sprite = m_AtlasArray[tempLookForwardIndex];
                tempLookForwardIndex += tempLookForwardDirection;
                if (tempLookForwardDirection == 1 && tempLookForwardIndex >= m_AtlasArray.Length)
                {
                    tempLookForwardIndex -= tempLookForwardDirection * 2;
                    tempLookForwardDirection = -1;
                }
                else if (tempLookForwardDirection == -1 && tempLookForwardIndex < m_AtlasArray.Length - 21)
                {
                    tempLookForwardIndex -= tempLookForwardDirection * 2;
                    tempLookForwardDirection = 1;
                }
            }
            else
            {
                //raycast forward from camera
                Ray r = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                //if it hit the eye sprite's shitty collider plane
                if (Physics.Raycast(r, out hit))
                {
                    //get direction from center of eyes to hit point
                    Vector2 spriteToHitDir = (hit.point - m_Renderer.transform.position).normalized;
                    //Debug.DrawRay(m_Renderer.transform.position, (hit.point - m_Renderer.transform.position), Color.green, 10f);

                    float angle = Mathf.Atan2(hit.point.y - m_Renderer.transform.position.y, hit.point.x - m_Renderer.transform.position.x);
                    angle *= Mathf.Rad2Deg;
                    angle -= 90;
                    if (angle < 0f)
                    {
                        angle = 360 + angle;
                    }
                    //reverse all angles
                    angle = 360 - angle;
                    angle = Mathf.Repeat(angle, 360);
                    //Debug.Log(angle);

                    int nearestEyeAngle = (int)Math.Round(angle / 15) * 15;
                    if (nearestEyeAngle == 360)
                    {
                        nearestEyeAngle = 0;
                    }
                    Debug.Log(nearestEyeAngle);

                    int[] eyeAngleFrames = GetEyeAngleFrames(nearestEyeAngle);
                    m_Renderer.sprite = m_AtlasArray[UnityEngine.Random.Range(eyeAngleFrames[0], eyeAngleFrames[eyeAngleFrames.Length-1])];

                }
            }
        }

        private int[] GetEyeAngleFrames(int nearestEyeAngle, bool includeLookToFromFrames = false)
        {
            //14 frames including lookframes, 8 frames in each static angle
            int numFrames = includeLookToFromFrames ? 14 : 8;
            int[] r = new int[numFrames];

            //15 degree increments, 14 frames per clip
            int startIndex = ((nearestEyeAngle / 15)) * 14;

            for (int i = 0; i < numFrames; i++)
            {
                //if not including look frames, skip the first 3 (which are the look towards frames)
                r[i] = startIndex + i + (includeLookToFromFrames ? 0 : 3);
            }
            return r;
        }

        //private IEnumerator tempDoIt()
        //{
        //    int tmpIndex = 0;
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(0.05f);
        //        m_Renderer.sprite = m_AtlasArray[tmpIndex];
        //        tmpIndex++;
        //        if (tmpIndex >= m_AtlasArray.Length)
        //        {
        //            tmpIndex = 0;
        //        }
        //    }
        //}
    }
}