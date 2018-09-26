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
        //static Material lineMaterial = null;

        [Required]
        public Material m_VertexColorsMaterial;


        private class LeafData
        {
            public LeafData(Mesh m, float[] _t) { mesh = m; t = _t; Debug.Assert(t.Length == 3); }
            public Mesh mesh;

            //ie: relative height in leaves-space (yPos between min/max leaves yPos)
            //vertices 0, 1, and 2 (for a triangle)
            public float[] t;
        }

        private class BranchData
        {
            public BranchData(Mesh m, float[] _t) { mesh = m; t = _t; Debug.Assert(t.Length == 2); }
            public Mesh mesh;

            //ie: relative height in leaves-space (yPos between min/max leaves yPos)
            //vertices 0 and 1 (for a line)
            public float[] t;//@TODO: we need to generate a mesh instead of using linerenderers
        }

        //needs a Triangle structure. to store local height & mesh
        private List<LeafData> m_Triangles;
        private List<BranchData> m_Branches;//@TODO

        private TreeSkeleton m_TreeSkeleton;

        private Transform m_LeavesRoot = null;
        private Transform m_BranchesRoot = null;

        [Required]
        public TreeSkeletonData m_SkeletonData;

        [Button("New RNG Seed", Sirenix.OdinInspector.ButtonSizes.Small)]
        public void OnNewRNGSeed() { m_RngSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue); }
        public int m_RngSeed;

        [Required]
        public LineRenderer m_BranchPrefab;

        //cache transform reference
        private Transform m_TransformComponent;

        //cache data blended gradients here.
        private Gradient m_LeafGradient;
        private Gradient m_BranchesGradient;

        public void Start()
        {
            m_Triangles = new List<LeafData>();
            //CreateLineMaterial();

            //cache transform component to avoid GetComponent<Transform> calls (implicitly called with MonoBehaviour.transform).
            m_TransformComponent = transform;

            //instance a root for all leaves (this tree)
            m_LeavesRoot = new GameObject().GetComponent<Transform>();
            m_LeavesRoot.name = "leaves_root";
            m_LeavesRoot.SetParent(transform);
            m_LeavesRoot.transform.localPosition = Vector3.zero;
            m_LeavesRoot.transform.localRotation = Quaternion.identity;
            m_LeavesRoot.transform.localScale = Vector3.one;

            //instance a root for all branches (this tree)
            m_BranchesRoot = new GameObject().GetComponent<Transform>();
            m_BranchesRoot.name = "branches_root";
            m_BranchesRoot.SetParent(transform);
            m_BranchesRoot.transform.localPosition = Vector3.zero;
            m_BranchesRoot.transform.localRotation = Quaternion.identity;
            m_BranchesRoot.transform.localScale = Vector3.one;

            //cache both gradients at this t (0)
            m_BranchesGradient = m_SkeletonData.m_BranchColorGradient.m_GradientGroups[0].m_Gradient;
            m_LeafGradient = m_SkeletonData.m_LeafGradient.m_GradientGroups[0].m_Gradient;

            GenerateSkeleton(Vector3.zero);
        }

        private void Update()
        {
            float globalT = dd.SceneManager.Instance.DayNightCycleManagers[0].m_GlobalT;
            m_BranchesGradient = m_SkeletonData.m_BranchColorGradient.GetGradientAt(globalT);
            m_LeafGradient = m_SkeletonData.m_LeafGradient.GetGradientAt(globalT);

            Color32[] colors;
            //update all leaf colors with m_T
            for (int i = 0; i < m_Triangles.Count; i++)
            {
                colors = new Color32[3];
                colors[0] = m_LeafGradient.Evaluate(m_Triangles[i].t[0]);
                colors[1] = m_LeafGradient.Evaluate(m_Triangles[i].t[1]);
                colors[2] = m_LeafGradient.Evaluate(m_Triangles[i].t[2]);
                m_Triangles[i].mesh.colors32 = colors;
            }
        }

        private void GenerateSkeleton(Vector3 basePosition)
        {
            m_TreeSkeleton = new TreeSkeleton();
            m_TreeSkeleton.Generate(m_SkeletonData, m_RngSeed);

            m_BranchRenderers = new List<LineRenderer>();
            RecursiveMakeTree(m_TreeSkeleton.m_Root);

            //only here do we know the min/max y bounds of all the triangles in the tree.
            for (int i = 0; i < m_Triangles.Count; ++i)
            {
                //this is where that leaf t comes back. we have already set these 3 t values to the world space y of each vertex of the leaf.
                m_Triangles[i].t[0] = Mathf.InverseLerp(m_MinLeafY, m_MaxLeafY, m_Triangles[i].t[0]);
                m_Triangles[i].t[1] = Mathf.InverseLerp(m_MinLeafY, m_MaxLeafY, m_Triangles[i].t[1]);
                m_Triangles[i].t[2] = Mathf.InverseLerp(m_MinLeafY, m_MaxLeafY, m_Triangles[i].t[2]);
            }
        }

        private void GenerateTrianglesInSphere(Vector3 position, float r, int numTriangles, Vector2 minMaxLeafRadius)
        {
            for (int triangleIndex = 0; triangleIndex < numTriangles; ++triangleIndex)
            {
                Vector3 forward = UnityEngine.Random.onUnitSphere;
                Vector3 randLocalPosition = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(.01f, r);
                m_Triangles.Add(MakeTriangle(randLocalPosition + position, forward, Vector3.Cross(forward, Vector3.up),
                                             UnityEngine.Random.Range(minMaxLeafRadius.x, minMaxLeafRadius.y)
                                             ));
            }
        }

        private float m_MinLeafY = Mathf.Infinity;
        private float m_MaxLeafY = -Mathf.Infinity;
        private LeafData MakeTriangle(Vector3 position, Vector3 forward, Vector3 up, float radius)
        {
            //since backface culling is off, the direction of right (vs left) shouldn't matter.
            Mesh newTri = new Mesh();
            Vector3[] newVerts = new Vector3[3];
            Color32[] newColors32 = new Color32[3];


            newVerts[0] = position + up * radius;
            //newColors32[0] = GetLeafGradientAt(newVerts[0]);
            if (m_MinLeafY > newVerts[0].y) { m_MinLeafY = newVerts[0].y; }
            if (m_MaxLeafY < newVerts[0].y) { m_MaxLeafY = newVerts[0].y; }

            float randAngleB = UnityEngine.Random.Range(15f, 165f);
            newVerts[1] = position + Quaternion.AngleAxis(randAngleB, forward) * (up * radius);
            //newColors32[1] = GetLeafGradientAt(newVerts[1]);
            if (m_MinLeafY > newVerts[1].y) { m_MinLeafY = newVerts[1].y; }
            if (m_MaxLeafY < newVerts[1].y) { m_MaxLeafY = newVerts[1].y; }

            float randAngleC = UnityEngine.Random.Range(randAngleB, 345f);
            newVerts[2] = position + Quaternion.AngleAxis(randAngleC, forward * Mathf.Rad2Deg) * (up * radius);
            //newColors32[2] = GetLeafGradientAt(newVerts[2]);
            if (m_MinLeafY > newVerts[2].y) { m_MinLeafY = newVerts[2].y; }
            if (m_MaxLeafY < newVerts[2].y) { m_MaxLeafY = newVerts[2].y; }


            newTri.vertices = newVerts;
            newTri.colors32 = newColors32;

            int[] tris = new int[6];
            tris[0] = 0; tris[1] = 1; tris[2] = 2;
            tris[3] = 2; tris[4] = 1; tris[5] = 0;

            newTri.SetTriangles(tris, 0, false);

            //newTri.RecalculateNormals();
            //Vector3 newNormal = newTri.normals[0];
            //newTri.SetNormals(new List<Vector3>{ newNormal, -newNormal });

            GameObject newObject = new GameObject();
            Transform newTransform = newObject.transform;
            newTransform.SetParent(m_LeavesRoot);
            newTransform.localPosition = Vector3.zero;
            newTransform.localRotation = Quaternion.identity;
            newTransform.localScale = Vector3.one;
            newTransform.name = "leaf_" + m_LeavesRoot.childCount;
            //newObject.layer = 8;

            //@TODO: make this into prefab and instantiate m_NullMeshPrefab
            MeshFilter newMF = newObject.AddComponent<MeshFilter>();
            MeshRenderer newMR = newObject.AddComponent<MeshRenderer>();
            newMF.mesh = newTri;
            newMR.material = m_VertexColorsMaterial;
            newMR.receiveShadows = false;
            newMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            newMR.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            newMR.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

            //!! Important note on this: that y is not actually the t we will use for gradient blending yet.
            //we'll use this y value later, when we know the min/max bounds of all the leaves in this tree.
            return new LeafData(newMF.mesh, new float[] { newVerts[0].y, newVerts[1].y, newVerts[2].y });
        }

        //static void CreateLineMaterial()
        //{
        //    if (!lineMaterial)
        //    {
        //        // Unity has a built-in shader that is useful for drawing
        //        // simple colored things.
        //        Shader shader = Shader.Find("Hidden/Internal-Colored");
        //        lineMaterial = new Material(shader);
        //        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        //        // Turn on alpha blending
        //        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //        // Turn backface culling off
        //        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        //        // Turn off depth writes
        //        lineMaterial.SetInt("_ZWrite", 0);
        //    }
        //}

        private void RecursiveMakeTree(TreeSkeletonNode root)
        {
            for (int i = 0; i < root.m_NextNodes.Count; i++)
            {
                //TODO:
                MakeBranch(root, root.m_NextNodes[i], Color.white, Color.black);
                //m_TreeSkeleton.m_Data.m_BranchColorGradient.Evaluate(root.m_Position.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height)),
                //m_TreeSkeleton.m_Data.m_BranchColorGradient.Evaluate(root.m_NextNodes[i].m_Position.y / (m_TreeSkeleton.m_Root.m_Position.y + m_TreeSkeleton.Height)));
                RecursiveMakeTree(root.m_NextNodes[i]);
            }
        }

        private List<LineRenderer> m_BranchRenderers;
        private void MakeBranch(TreeSkeletonNode startNode, TreeSkeletonNode endNode, Color startColor, Color endColor)
        {
            LineRenderer newBranch = GameObject.Instantiate(m_BranchPrefab.gameObject).GetComponent<LineRenderer>();

            //set branch width over tree height according to data defined width curve
            newBranch.startWidth = Mathf.Lerp(m_SkeletonData.m_MinMaxWidth.x,
                                              m_SkeletonData.m_MinMaxWidth.y,
                                              m_SkeletonData.m_WidthCurve.Evaluate((float)startNode.m_Level / (float)m_SkeletonData.m_MaxLevels));
            newBranch.endWidth = Mathf.Lerp(m_SkeletonData.m_MinMaxWidth.x,
                                            m_SkeletonData.m_MinMaxWidth.y,
                                            m_SkeletonData.m_WidthCurve.Evaluate((float)endNode.m_Level / (float)m_SkeletonData.m_MaxLevels));

            //generate leaves
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
            //newBranch.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            newBranch.startColor = startColor;
            newBranch.endColor = endColor;
            //newBranch.material = lineMaterial;
            m_BranchRenderers.Add(newBranch);

            //child all branches to this. make sure UseWorldSpace is off in your linerenderer prefab.
            newBranch.transform.SetParent(m_BranchesRoot);
            newBranch.name = "branch_" + transform.childCount;
        }
    }
}