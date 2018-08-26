using System;
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

        //if we've just clicked, or just released click, lock the update (with this != 0)
        //if position, count up, and set index regularly, we are looking to lens
        //if negative, count up, and set index backwards, we are looking back to lens
        int m_TransitionIndex = 0;
        private void Update()
        {
            //look back to lens
            if (Input.GetMouseButtonDown(0))
            {
                if (m_TransitionIndex == 0)
                {
                    m_TransitionIndex = -3;
                }
                //if we release during the 3 frames
                else
                {
                    m_TransitionIndex *= -1;
                }
            }
            //look out from lens
            else if (Input.GetMouseButtonUp(0))
            {
                if (m_TransitionIndex == 0)
                {
                    m_TransitionIndex = 3;
                }
                //if we release during the 3 frames
                else
                {
                    m_TransitionIndex *= -1;
                }
            }

            if (Input.mouseScrollDelta.magnitude > 0f)
            {
                m_Renderer.transform.position += new Vector3(0, 0, Input.mouseScrollDelta.y * Time.deltaTime * (Input.GetKey(KeyCode.LeftControl) ? 24f : 6f));
                if (m_Renderer.transform.position.z < 0)
                {
                    m_Renderer.transform.position = Vector3.zero;
                }
                else if (m_Renderer.transform.position.z > 10f)
                {
                    m_Renderer.transform.position = new Vector3(0, 0, 10);
                }
            }

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

            if (m_TransitionIndex != 0)
            {
                //raycast forward from camera
                Ray r = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                //if it hit the eye sprite's shitty collider plane
                int[] eyeAngleFrames = null;
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

                    eyeAngleFrames = GetEyeAngleFrameIndices(nearestEyeAngle, true);

                }
                Debug.Assert(eyeAngleFrames != null);
                //looking back to lens
                if (m_TransitionIndex < 0)
                {
                    m_Renderer.sprite = m_AtlasArray[eyeAngleFrames[eyeAngleFrames.Length - 1] - ((m_TransitionIndex * -1))];

                    m_TransitionIndex++;
                }
                //looking away from lens to angle
                else if (m_TransitionIndex > 0)
                {
                    m_Renderer.sprite = m_AtlasArray[eyeAngleFrames[0] + (3 - m_TransitionIndex)];

                    m_TransitionIndex--;
                }
            }
            //if left clicking
            else if (Input.GetMouseButton(0))
            {
                //choose random frame from last 20 (staring forward)
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
                    //Debug.Log(nearestEyeAngle);

                    int[] eyeAngleFrames = GetEyeAngleFrameIndices(nearestEyeAngle);
                    m_Renderer.sprite = m_AtlasArray[UnityEngine.Random.Range(eyeAngleFrames[0], eyeAngleFrames[eyeAngleFrames.Length-1])];

                }
            }
        }

        private int[] GetEyeAngleFrameIndices(int nearestEyeAngle, bool includeLookToFromFrames = false)
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
    }
}