using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class DialogBranchNode : DialogNode
    {
        public DialogBranchNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedNodeStyle, GUIStyle inPointStyleNormal, GUIStyle inPointStylePressed, GUIStyle outPointStyleNormal, GUIStyle outPointStylePressed, Action<DialogConnectionPoint> onClickDownInPoint, Action<DialogConnectionPoint> onClickDownOutPoint, Action<DialogConnectionPoint> onClickReleaseInPoint, Action<DialogConnectionPoint> onClickReleaseOutPoint, Action<DialogNode> onClickRemoveNode, Action<DialogNode> onBecomeStartNode) : base(position, nodeStyle, selectedNodeStyle, inPointStyleNormal, inPointStylePressed, outPointStyleNormal, outPointStylePressed, onClickDownInPoint, onClickDownOutPoint, onClickReleaseInPoint, onClickReleaseOutPoint, onClickRemoveNode, onBecomeStartNode)
        {
            
        }

        public override void Move(Vector2 delta)
        {
            base.Move(delta);

        }
    }
}