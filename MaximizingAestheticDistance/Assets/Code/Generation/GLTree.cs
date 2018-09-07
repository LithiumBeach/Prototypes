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

        public AnimationCurve m_ForkProbabilityFalloff;
        public int m_MaxBranchesAtFork;
        public Vector2 m_MinMaxHeight;
        public int m_MaxLevels;
        [Button("New RNG Seed", Sirenix.OdinInspector.ButtonSizes.Small)]
        public void OnNewRNGSeed() { m_RngSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue); }
        public int m_RngSeed;

        public AnimationCurve m_BranchWidthOverLevelCurve;

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
            m_TreeSkeleton.Generate(GradientFunctors.Instance.m_Gradients[0], m_ForkProbabilityFalloff, m_MaxBranchesAtFork, m_MinMaxHeight, m_MaxLevels, m_RngSeed);

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
                DrawBranch(root, root.m_NextNodes[i]);
                RecursiveDrawBranches(root.m_NextNodes[i]);
            }
        }

        private List<LineRenderer> m_BranchRenderers;
        private void DrawBranch(TreeSkeletonNode startNode, TreeSkeletonNode endNode)
        {
            LineRenderer newBranch = GameObject.Instantiate(m_BranchPrefab.gameObject).GetComponent<LineRenderer>();

            //@TODO:
            //newBranch.startWidth = Mathf.Lerp(0.5f, 0.01f, m_BranchWidthOverLevelCurve.Evaluate(Mathf.Clamp(startNode.m_Level / m_TreeSkeleton.MaxLevels, .05f, 0.95f)));
            //newBranch.endWidth = Mathf.Lerp(0.5f, 0.01f, m_BranchWidthOverLevelCurve.Evaluate(Mathf.Clamp(endNode.m_Level / m_TreeSkeleton.MaxLevels, .05f, 0.95f)));
            newBranch.startWidth = .01f;
            newBranch.endWidth = .01f;


            newBranch.SetPosition(1, startNode.m_Position);
            newBranch.SetPosition(0, endNode.m_Position);
            m_BranchRenderers.Add(newBranch);
        }
    }
}