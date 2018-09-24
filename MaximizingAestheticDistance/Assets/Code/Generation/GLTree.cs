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
        static Material lineMaterial = null;

        [Required]
        public Material m_VertexColorsMaterial;

        private List<MeshFilter> m_Triangles;

        private TreeSkeleton m_TreeSkeleton;

        private Transform m_LeavesRoot = null;

        [Required]
        public TreeSkeletonData m_SkeletonData;

        [Button("New RNG Seed", Sirenix.OdinInspector.ButtonSizes.Small)]
        public void OnNewRNGSeed() { m_RngSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue); }
        public int m_RngSeed;

        [Required]
        public LineRenderer m_BranchPrefab;

        //cache transform reference
        private Transform m_TransformComponent;

        public void Start()
        {
            m_TransformComponent = GetComponent<Transform>();

            m_Triangles = new List<MeshFilter>();
            m_LeavesRoot = GameObject.Instantiate(new GameObject()).GetComponent<Transform>();
            m_LeavesRoot.name = "leaves_root";
            m_LeavesRoot.SetParent(transform);
            m_LeavesRoot.transform.localPosition = Vector3.zero;
            m_LeavesRoot.transform.localRotation = Quaternion.identity;
            m_LeavesRoot.transform.localScale = Vector3.one;
            GenerateSkeleton(Vector3.zero);
        }

        private void GenerateSkeleton(Vector3 basePosition)
        {
            m_TreeSkeleton = new TreeSkeleton();
            m_TreeSkeleton.Generate(m_SkeletonData, m_RngSeed);

            m_BranchRenderers = new List<LineRenderer>();
            RecursiveMakeBranches(m_TreeSkeleton.m_Root);
        }

        private void GenerateTrianglesInSphere(Vector3 position, float r, int numTriangles, Vector2 minMaxLeafRadius)
        {
            for (int triangleIndex = 0; triangleIndex < numTriangles; triangleIndex++)
            {
                Vector3 forward = UnityEngine.Random.onUnitSphere;
                Vector3 randLocalPosition = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(.01f, r);
                m_Triangles.Add(MakeTriangle(randLocalPosition + position, forward, Vector3.Cross(forward, Vector3.up),
                                             UnityEngine.Random.Range(minMaxLeafRadius.x, minMaxLeafRadius.y)
                                             ));
            }
        }

        public MeshFilter MakeTriangle(Vector3 position, Vector3 forward, Vector3 up, float radius)
        {
            //since backface culling is off, the direction of right (vs left) shouldn't matter.
            Mesh newTri = new Mesh();
            Vector3[] newVerts = new Vector3[3];
            Color32[] newColors32 = new Color32[3];


            newVerts[0] = position + up * radius;
            newColors32[0] = GetLeafGradientAt(newVerts[0]);

            float randAngleB = UnityEngine.Random.Range(15f, 165f);
            newVerts[1] = position + Quaternion.AngleAxis(randAngleB, forward) * (up * radius);
            newColors32[1] = GetLeafGradientAt(newVerts[1]);
            float randAngleC = UnityEngine.Random.Range(randAngleB, 345f);

            newVerts[2] = position + Quaternion.AngleAxis(randAngleC, forward * Mathf.Rad2Deg) * (up * radius);
            newColors32[2] = GetLeafGradientAt(newVerts[2]);


            newTri.vertices = newVerts;
            newTri.colors32 = newColors32;

            int[] tris = new int[6];
            tris[0] = 0; tris[1] = 1; tris[2] = 2;
            tris[3] = 2; tris[4] = 1; tris[5] = 0;

            newTri.SetTriangles(tris, 0, false);

            //newTri.RecalculateNormals();
            //Vector3 newNormal = newTri.normals[0];
            //newTri.SetNormals(new List<Vector3>{ newNormal, -newNormal });

            GameObject newObject = GameObject.Instantiate(new GameObject());
            newObject.transform.SetParent(m_LeavesRoot);
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localRotation = Quaternion.identity;
            newObject.transform.localScale = Vector3.one;
            MeshFilter newMF = newObject.AddComponent<MeshFilter>();
            MeshRenderer newMR = newObject.AddComponent<MeshRenderer>();
            newMF.mesh = newTri;
            newMR.material = m_VertexColorsMaterial;
            return newMF;
        }

        private Color GetLeafGradientAt(Vector3 pos)
        {
            return m_TreeSkeleton.m_Data.m_LeafGradient.Evaluate(pos.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height));
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
        //public void OnRenderObject()
        //{
        //    CreateLineMaterial();
        //    // Apply the line material
        //    lineMaterial.SetPass(0);
        //
        //    GL.PushMatrix();
        //    // Set transformation matrix for drawing to
        //    // match our transform
        //    GL.LoadIdentity();
        //    GL.MultMatrix(Camera.main.worldToCameraMatrix);
        //
        //    //Begin GL Draws
        //
        //    for (int i = 0; i < m_Triangles.Count; i++)
        //    {
        //        m_Triangles[i].Draw(m_TransformComponent.position);
        //    }
        //
        //    //End GL Draws
        //
        //    GL.PopMatrix();
        //}

        private void RecursiveMakeBranches(TreeSkeletonNode root)
        {
            for (int i = 0; i < root.m_NextNodes.Count; i++)
            {
                MakeBranch(root, root.m_NextNodes[i],
                    m_TreeSkeleton.m_Data.m_BranchColorGradient.Evaluate(root.m_Position.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height)),
                    m_TreeSkeleton.m_Data.m_BranchColorGradient.Evaluate(root.m_NextNodes[i].m_Position.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height)));
                RecursiveMakeBranches(root.m_NextNodes[i]);
            }
        }

        private List<LineRenderer> m_BranchRenderers;
        private void MakeBranch(TreeSkeletonNode startNode, TreeSkeletonNode endNode, Color startColor, Color endColor)
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

            if (endNode.m_Level >= (m_TreeSkeleton.m_Data.m_MaxLevels - m_TreeSkeleton.m_Data.m_LevelsFromMaxToSpawnLeaves))
            {
                float rand01 = UnityEngine.Random.Range(0f, 1f);
                if (rand01 < m_TreeSkeleton.m_Data.m_ChanceOfCluster)
                {
                    GenerateTrianglesInSphere((startNode.m_Position + endNode.m_Position) * .5f,
                                                //Vector3.Distance(startNode.m_Position, endNode.m_Position) * .5f,
                                                m_TreeSkeleton.m_Data.m_LeafClusterRadius,
                                                (int)UnityEngine.Random.Range(m_TreeSkeleton.m_Data.m_LeavesPerClusterMinMax.x, m_TreeSkeleton.m_Data.m_LeavesPerClusterMinMax.y),
                                                m_TreeSkeleton.m_Data.m_MinMaxLeafSize);
                }
            }

            newBranch.SetPosition(0, startNode.m_Position + m_TransformComponent.position);// - littleStartOffset);
            newBranch.SetPosition(1, endNode.m_Position + m_TransformComponent.position);

            newBranch.startColor = startColor;
            newBranch.endColor = endColor;
            m_BranchRenderers.Add(newBranch);

            //child all branches to this. make sure UseWorldSpace is off in your linerenderer prefab.
            newBranch.transform.parent = transform;
        }
    }
}