﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class TreeSkeleton
    {
        public TreeSkeletonData m_Data;
        public TreeSkeletonNode m_Root;

        private int m_NodeCount;
        public int NodeCount { get { return m_NodeCount; } }
        private float m_TotalHeight;
        public float Height { get { return m_TotalHeight; } }

        private UnityEngine.Random m_RNG;

        public int MaxLevels { get { return m_Data.m_MaxLevels; } }


        //@Note: if this were to need to be parallelized, i would recommend starting with the leaves and working your way inward to the trunk.
        //harder, but starts with no branch dependencies and works backwards. the current, more intuitive way, starts with all possible dependencies.

        /// <summary>
        /// Will generate the skeleton. Nothing visual. No defined width.
        /// Caller is responsible for any transform stuff(TRS) it wants.
        /// ROOT POSITION DOES NOT MATTER. APPLY TRANSFORM STUFF LOCALIZED TO AN OBJECT.
        /// </summary>
        /// <param name="rngSeed">deterministic if you want it to be. if not, pass in a random seed.</param>
        public void Generate(TreeSkeletonData data, int rngSeed)
        {
            m_Data = data;

            //deterministic
            UnityEngine.Random.InitState(rngSeed);

            m_NodeCount = 0;
            //generate the bottom node of the tree.
            m_Root = new TreeSkeletonNode();
            m_Root.m_Position = Vector3.zero;
            m_Root.m_Level = 0;
            m_Root.m_Color = m_Data.m_BranchColorGradient.GetGradientAt(0).Evaluate(0);


            //calculate our final height
            m_TotalHeight = UnityEngine.Random.Range(m_Data.m_MinMaxHeight.x, m_Data.m_MinMaxHeight.y);

            //put the first fork approx 1/3 the way up the trunk
            float firstForkHeight = UnityEngine.Random.Range(m_TotalHeight * m_Data.m_MinMaxTrunkHeightMultiplier.x,
                                                             m_TotalHeight * m_Data.m_MinMaxTrunkHeightMultiplier.y);

            //generate a spline point upwards-ish, to the first fork.
            TreeSkeletonNode firstForkNode = new TreeSkeletonNode();
            //dont need to take root node position into account because it's always zero
            firstForkNode.m_Position = new Vector3(UnityEngine.Random.Range(-.25f, .25f), //slight x offset
                                            firstForkHeight,
                                            UnityEngine.Random.Range(-.25f, .25f)); //slight z offset
            //set to appropriate gradient color
            firstForkNode.m_Color = m_Data.m_BranchColorGradient.GetGradientAt(0).Evaluate(firstForkHeight / m_TotalHeight);
            firstForkNode.m_Level = 1;

            //maintain n-tree structure
            m_Root.m_NextNodes.Add(firstForkNode);

            //forks happen at each spine interval, random number of forks in range
            //random (small) offset from fork position, up or down, along spline (if you pass a fork choose random branch)
            //forks are recursion.
            //choose random number of branches at each fork. that bit may need to be defined by some falloff curve later on.
            //start at level 2 (after first fork node).
            Fork(firstForkNode, UnityEngine.Random.Range(2, m_Data.m_MaxBranchesAtFork + 1), 2, Vector3.Normalize((firstForkNode.m_Position - m_Root.m_Position)));

        }

        private void Fork(TreeSkeletonNode root, int numForks, int level, Vector3 rootDirectionNormalized)
        {
            if (level > m_Data.m_MaxLevels)
            {
                return;
            }
            m_NodeCount++;
            //List<TreeSkeletonNode> nextNodes = new List<TreeSkeletonNode>();
            for (int forkIndex = 0; forkIndex < numForks; forkIndex++)
            {
                //the implicit line connecting root and nextNode defines the branch.
                TreeSkeletonNode nextNode = new TreeSkeletonNode();

                //quick and dirty noisy falloff function for reducing length of branches as level goes higher.
                float levelToLengthRatio = Mathf.Clamp01(1f - (level / m_Data.m_MaxLevels));
                float randLength = UnityEngine.Random.Range(levelToLengthRatio - .1f, levelToLengthRatio + .1f) * 1.0f;

                //@TODO: get random offset from root node (between root node position and root node position + a little bit) along root dir
                Vector3 randomNodeOffset = Vector3.zero;

                Vector2 angleRange = new Vector2(UnityEngine.Random.Range(1f * levelToLengthRatio, 24f * levelToLengthRatio), UnityEngine.Random.Range(45f * levelToLengthRatio, 65f * levelToLengthRatio));
                //multiply length to a normalized random branch direction determined by this tree. add last position.
                nextNode.m_Position = root.m_Position + randomNodeOffset + GenerateBranchDirection(rootDirectionNormalized,
                    angleRange,
                    level) * randLength;

                //set to appropriate gradient color
                nextNode.m_Color = m_Data.m_BranchColorGradient.GetGradientAt(0).Evaluate(Mathf.Clamp01(nextNode.m_Position.y / m_TotalHeight));
                nextNode.m_Level = level;

                //it's going to go a max of one branch higher than the passed in max height. tell nobody.
                //@TODO: you could do this at the start of the function using root's position.
                bool overHeight = (nextNode.m_Position.y / m_TotalHeight) >= .99f;

                //decide if we will fork according to falloff curve--only if not past height cap
                float rand01 = UnityEngine.Random.Range(0f, 1f);
                bool willFork = !overHeight && rand01 < m_Data.m_ForkProbabilityFalloff.Evaluate((nextNode.m_Position.y) / m_TotalHeight);

                //if we're going to fork
                if (willFork)
                {
                    //and we haven't made an error with our recursion
                    if (!root.m_NextNodes.Contains(nextNode))
                    {
                        //maintain tree structure
                        root.m_NextNodes.Add(nextNode);

                        //Fork
                        int nextNumForkAttempts = UnityEngine.Random.Range(2, m_Data.m_MaxBranchesAtFork + 1);
                        Fork(nextNode, nextNumForkAttempts, level + 1, Vector3.Normalize((nextNode.m_Position - root.m_Position))); 
                    }
                }
                //else don't fork, continue.
            }
        }

        //@TODO: parameterize function with tree types 🙃
        private Vector3 GenerateBranchDirection(Vector3 rootDirNormalized, Vector2 angleRange, int level)
        {
            //imagine a circle about rootDirNormalized. this angle is regarding that circle. it's the direction your pitch angle will pitch in.
            float randAngle = UnityEngine.Random.Range(0f, 360);
            //this is hard to describe. make an L-shape with your index finger and your thumb. pretend your finger and thumb are branches of a tree.
            //this is the angle between your thumb and finger.
            //angleRange = angleRange * Mathf.Clamp01(1f - (level / m_MaxLevels));
            float randPitchAngle = UnityEngine.Random.Range(angleRange.x, angleRange.y);

            //ARBITRARY perpendicular angle. Chosen constants maintain normalized vector. Pretty sure. It's fine we're fine.
            Vector3 arbitraryOrtho = Vector3.Cross(rootDirNormalized, Vector3.up);

            Vector3 newDir = rootDirNormalized;
            Quaternion randPitchRotation = Quaternion.AngleAxis(randPitchAngle, arbitraryOrtho);
            newDir = randPitchRotation * newDir;
            Quaternion randRotation = Quaternion.AngleAxis(randAngle, rootDirNormalized);
            newDir = randRotation * newDir;

            return Vector3.Normalize(newDir);
        }
    }

    public class TreeSkeletonNode
    {
        public Vector3 m_Position;
        public Color m_Color;
        public List<TreeSkeletonNode> m_NextNodes;
        public int m_Level;

        public TreeSkeletonNode()
        {
            m_NextNodes = new List<TreeSkeletonNode>();
            m_Color = Color.magenta;
        }
    }
}