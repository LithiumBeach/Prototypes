using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class TreeSkeleton
    {
        public TreeSkeletonNode m_Root;

        private UnityEngine.Random m_RNG;
        private AnimationCurve m_ForkProbabilityFalloff;

        private float m_TotalHeight;
        private int m_MaxBranchesAtFork;
        private int m_MaxLevels;
        private Gradient m_ColorGradient;

        //@Note: if this were to need to be parallelized, i would recommend starting with the leaves and working your way inward to the trunk.
        //harder, but starts with no branch dependencies and works backwards. the current, more intuitive way, starts with all possible dependencies.


        /// <summary>
        /// Will generate the skeleton. Nothing visual. No defined width.
        /// Caller is responsible for any transform stuff(TRS) it wants.
        /// </summary>
        /// <param name="colorGradient"></param>
        /// <param name="branchProbabilityFalloff">after every fork, there is a chance that those forked branches will split. This defines that as a function over time.
        /// Probability should go down (falloff) as more splits occur (t++)</param>
        /// <param name="minMaxHeight"></param>
        /// <param name="maxLevels">how many consecutive forks is the absolute maximum you want to allow?</param>
        /// <param name="rngSeed">deterministic if you want it to be. if not, pass in a random seed.</param>
        public void Generate(Gradient colorGradient, AnimationCurve forkProbabilityFalloff, int maxBranchesAtFork, Vector2 minMaxHeight, int maxLevels, int rngSeed)
        {//ROOT POSITION DOES NOT MATTER. APPLY TRANSFORM STUFF LOCALIZED TO AN OBJECT.
            m_ForkProbabilityFalloff = forkProbabilityFalloff;
            m_MaxBranchesAtFork = maxBranchesAtFork;
            m_MaxLevels = maxLevels;
            m_ColorGradient = colorGradient;

            //generate the bottom node of the tree.
            m_Root = new TreeSkeletonNode();
            m_Root.m_Position = Vector3.zero;
            m_Root.m_Color = colorGradient.Evaluate(0);
            m_Root.m_Level = 0;

            //deterministic
            UnityEngine.Random.InitState(rngSeed);

            //calculate our final height
            m_TotalHeight = UnityEngine.Random.Range(minMaxHeight.x, minMaxHeight.y);

            //put the first fork approx 1/3 the way up the trunk
            float firstForkHeight = UnityEngine.Random.Range(m_TotalHeight * .25f, m_TotalHeight * .5f);

            //generate a spline point upwards-ish, to the first fork.
            TreeSkeletonNode firstForkNode = new TreeSkeletonNode();
            //dont need to take root node position into account because it's always zero
            firstForkNode.m_Position = new Vector3(UnityEngine.Random.Range(-.25f, .25f), //slight x offset
                                            firstForkHeight,
                                            UnityEngine.Random.Range(-.25f, .25f)); //slight z offset
            //set to appropriate gradient color
            firstForkNode.m_Color = colorGradient.Evaluate(firstForkHeight / m_TotalHeight);
            firstForkNode.m_Level = 1;

            //maintain n-tree structure
            m_Root.m_NextNodes.Add(firstForkNode);

            //forks happen at each spine interval, random number of forks in range
            //random (small) offset from fork position, up or down, along spline (if you pass a fork choose random branch)
            //forks are recursion.
            //choose random number of branches at each fork. that bit may need to be defined by some falloff curve later on.
            //start at level 2 (after first fork node).
            Fork(firstForkNode, UnityEngine.Random.Range(2, m_MaxBranchesAtFork + 1), 2, Vector3.Normalize((firstForkNode.m_Position - m_Root.m_Position)));

        }

        private void Fork(TreeSkeletonNode root, int numForks, int level, Vector3 rootDirectionNormalized)
        {
            //List<TreeSkeletonNode> nextNodes = new List<TreeSkeletonNode>();
            for (int forkIndex = 0; forkIndex < numForks; forkIndex++)
            {
                //the implicit line connecting root and nextNode defines the branch.
                TreeSkeletonNode nextNode = new TreeSkeletonNode();

                //quick and dirty noisy falloff function for reducing length of branches as level goes higher.
                float levelToLengthRatio = Mathf.Clamp01(1f - level / m_MaxLevels);
                float randLength = UnityEngine.Random.Range(levelToLengthRatio - .1f, levelToLengthRatio + .1f);

                //@TODO: get random offset from root node (between root node position and root node position + a little bit)
                Vector3 randomNodeOffset = Vector3.zero;

                //multiply length to a normalized random branch direction determined by this tree. add last position.
                nextNode.m_Position = root.m_Position + randomNodeOffset + GenerateBranchDirection(rootDirectionNormalized) * randLength;

                //it's going to go a max of one branch higher than the passed in max height. tell nobody.
                //@TODO: you could do this at the start of the function using root's position.
                bool overHeight = (nextNode.m_Position.y / m_TotalHeight) >= .99f;

                //set to appropriate gradient color
                nextNode.m_Color = m_ColorGradient.Evaluate(Mathf.Clamp01(nextNode.m_Position.y / m_TotalHeight));
                nextNode.m_Level = level;

                //chance of this new one forking --only if not past height cap
                float chanceOfFork = overHeight ? 0 : m_ForkProbabilityFalloff.Evaluate((nextNode.m_Position.y) / m_TotalHeight);
                float rand01 = UnityEngine.Random.Range(0f, 1f);
                //if we're going to fork
                if (rand01 < chanceOfFork)
                {
                    //and we haven't made an error with our recursion
                    if (!root.m_NextNodes.Contains(nextNode))
                    {
                        //maintain tree structure
                        root.m_NextNodes.Add(nextNode);

                        //Fork
                        int nextNumForkAttempts = UnityEngine.Random.Range(0, m_MaxBranchesAtFork + 1);
                        Fork(nextNode, nextNumForkAttempts, level + 1, Vector3.Normalize((nextNode.m_Position - root.m_Position))); 
                    }
                }
                //else don't fork, continue.
            }
        }

        //@TODO: parameterize function with tree types 🙃
        private Vector3 GenerateBranchDirection(Vector3 rootDirNormalized)
        {
            //stub

            return Vector3.up;
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