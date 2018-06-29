using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    [CreateAssetMenu(fileName="LocalizedDialogGroup", menuName="Dialog Director/LocalizedDialogGroup", order=0)]
    public class LocalizedDialogGroup : ScriptableObject
    {
        public DialogData[] m_Lines;
    }
}