using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    [CreateAssetMenu(fileName = "TreeSkeletonData_", menuName = "GenerationDatas/TreeSkeleton", order = 1)]
    public class TreeSkeletonData : ScriptableObject
    {
        [Tooltip("after every fork, there is a chance that those forked branches will split. this defines that as a function over time. probability should go down (falloff) as more splits occur (t++)")]
        public AnimationCurve m_ForkProbabilityFalloff;

        [Range(1, 8)]
        [Tooltip("max # of branches at each fork. may want to define this over f(t) (AnimationCurve)")]
        public int m_MaxBranchesAtFork = 2;

        [MinMaxSlider(1, 20, true)]
        [Tooltip("range of total height of tree. not exact. angle range/limiting levels will affect this.")]
        public Vector2 m_MinMaxHeight;

        [Tooltip("height of JUST the trunk, up to the first fork. multiplies by total height.")]
        [MinMaxSlider(0f, 1f, true)]
        public Vector2 m_MinMaxTrunkHeightMultiplier = new Vector2(0.1f, 0.2f);

        [Range(2, 12)]
        [Tooltip("how many consecutive forks is the absolute maximum you want to allow? (how deep does the tree go)")]
        public int m_MaxLevels;

        [Tooltip("trunk/branch color gradient over some defined function")]
        public Gradient m_BranchColorGradient;
        //@TODO: add an enum var to choose which function this gradient is run through

        [Tooltip("at each fork, push each new branch down it's root branch just a little bit. can never exceed previous branch length. multiplies with *current* branch length.")]
        [MinMaxSlider(0f, 6f, true)]
        public Vector2 m_MinMaxWidth = new Vector2(0.025f, 0.025f);

        [Tooltip("define a function to control width over t (node depth), between max/min width")]
        public AnimationCurve m_WidthCurve;
    }
}