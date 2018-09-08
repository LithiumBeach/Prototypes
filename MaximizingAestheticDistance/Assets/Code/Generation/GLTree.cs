using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class GLTree : MonoBehaviour
    {
        static Material lineMaterial;

        private List<GLTriangle> m_Triangles;

        private TreeSkeleton m_TreeSkeleton;

        [Required]
        public TreeSkeletonData m_SkeletonData;

        [Button("New RNG Seed", Sirenix.OdinInspector.ButtonSizes.Small)]
        public void OnNewRNGSeed() { m_RngSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue); }
        public int m_RngSeed;

        [Required]
        public LineRenderer m_BranchPrefab;

        public void Start()
        {
            m_Triangles = new List<GLTriangle>();
            GenerateSkeleton(Vector3.zero);
            //GenerateTrianglesInSphere(new Vector3(0, 4, 0), 4, 200);
        }

        private void GenerateSkeleton(Vector3 basePosition)
        {
            m_TreeSkeleton = new TreeSkeleton();
            m_TreeSkeleton.Generate(m_SkeletonData, m_RngSeed);

            m_BranchRenderers = new List<LineRenderer>();
            RecursiveDrawBranches(m_TreeSkeleton.m_Root);
        }

        private void GenerateTrianglesInSphere(Vector3 position, float r, int numTriangles)
        {
            for (int triangleIndex = 0; triangleIndex < numTriangles; triangleIndex++)
            {
                Vector3 randLocalPosition = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(.01f, r);
                m_Triangles.Add(MakeTriangle(randLocalPosition + position, Vector3.forward, Vector3.down, UnityEngine.Random.Range(.2f, .6f), GradientFunctors.Instance.GetGradientAt));
            }
        }

        public GLTriangle MakeTriangle(Vector3 position, Vector3 forward, Vector3 up, float radius, System.Func<Vector3, Color> gradientFunctor)
        {
            //since backface culling is off, the direction of right (vs left) shouldn't matter.
            GLTriangle newTri = new GLTriangle();
            newTri.m_A = position + up * radius;
            newTri.m_AColor = gradientFunctor(newTri.m_A);
            float randAngleB = UnityEngine.Random.Range(15f, 165f);
            newTri.m_B = position + Quaternion.AngleAxis(randAngleB, forward) * (up * radius);
            newTri.m_BColor = gradientFunctor(newTri.m_B);
            float randAngleC = UnityEngine.Random.Range(randAngleB, 345f);
            newTri.m_C = position + Quaternion.AngleAxis(randAngleC, forward * Mathf.Rad2Deg) * (up * radius);
            newTri.m_CColor = gradientFunctor(newTri.m_C);
            return newTri;
        }


        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
            CreateLineMaterial();
            // Apply the line material
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.LoadIdentity();
            GL.MultMatrix(Camera.main.worldToCameraMatrix);

            //Begin GL Draws

            for (int i = 0; i < m_Triangles.Count; i++)
            {
                m_Triangles[i].Draw();
            }

            //End GL Draws

            GL.PopMatrix();
        }

        private void RecursiveDrawBranches(TreeSkeletonNode root)
        {
            for (int i = 0; i < root.m_NextNodes.Count; i++)
            {
                DrawBranch(root, root.m_NextNodes[i],
                    m_TreeSkeleton.m_Data.m_BranchColorGradient.Evaluate(root.m_Position.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height)),
                    m_TreeSkeleton.m_Data.m_BranchColorGradient.Evaluate(root.m_NextNodes[i].m_Position.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height)));
                RecursiveDrawBranches(root.m_NextNodes[i]);
            }
        }

        private List<LineRenderer> m_BranchRenderers;
        private void DrawBranch(TreeSkeletonNode startNode, TreeSkeletonNode endNode, Color startColor, Color endColor)
        {
            LineRenderer newBranch = GameObject.Instantiate(m_BranchPrefab.gameObject).GetComponent<LineRenderer>();

            //@TODO:
            //newBranch.startWidth = Mathf.Lerp(0.5f, 0.01f, m_BranchWidthOverLevelCurve.Evaluate(Mathf.Clamp(startNode.m_Level / m_TreeSkeleton.MaxLevels, .05f, 0.95f)));
            //newBranch.endWidth = Mathf.Lerp(0.5f, 0.01f, m_BranchWidthOverLevelCurve.Evaluate(Mathf.Clamp(endNode.m_Level / m_TreeSkeleton.MaxLevels, .05f, 0.95f)));
            newBranch.startWidth = Mathf.Lerp(m_SkeletonData.m_MinMaxWidth.x,
                                              m_SkeletonData.m_MinMaxWidth.y,
                                              m_SkeletonData.m_WidthCurve.Evaluate((float)startNode.m_Level / (float)m_SkeletonData.m_MaxLevels));
            newBranch.endWidth = Mathf.Lerp(m_SkeletonData.m_MinMaxWidth.x,
                                            m_SkeletonData.m_MinMaxWidth.y,
                                            m_SkeletonData.m_WidthCurve.Evaluate((float)endNode.m_Level / (float)m_SkeletonData.m_MaxLevels));
            //newBranch.startWidth = .025f;
            //newBranch.endWidth = .025f;

            newBranch.SetPosition(0, startNode.m_Position);// - littleStartOffset);
            newBranch.SetPosition(1, endNode.m_Position);

            newBranch.startColor = startColor;
            newBranch.endColor = endColor;
            m_BranchRenderers.Add(newBranch);
        }
    }
}