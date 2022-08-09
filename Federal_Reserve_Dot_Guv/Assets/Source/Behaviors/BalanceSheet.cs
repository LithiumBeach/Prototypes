using System.Collections.Generic;
using UnityEngine;
using util;

namespace lb
{
    public class BalanceSheet : MonoBehaviour
    {
        //cash, cash equivalents, cash not-equivalents.
        public List<FinEntry> m_Assets;

        //all liabilities are POSITIVE values.
        //NEGATIVE liabilities are assets, im like 90% sure this would always be the case.
        public List<FinEntry> m_Liabilities;


    }
}