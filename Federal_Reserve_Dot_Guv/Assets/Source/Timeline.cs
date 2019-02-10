using System.Collections.Generic;
using UnityEngine;

namespace lb
{
    public class Timeline : MonoBehaviour
    {
        private LBDate currentDate;
        private Stack<LBDate> pastDates;

        private void Awake()
        {
            currentDate = new LBDate();
        }

        private void Update()
        {
            Debug.Log(currentDate.PrintDate(LBDate.EDateFormat.DD_MM_YYYY));
            currentDate.NextDay();
        }
    }
}