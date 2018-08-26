using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace dd
{
    public class EyeCompare : IComparer<Sprite>
    {
        public int Compare(Sprite x, Sprite y)
        {
            int xIndexOfUnderscore = x.name.LastIndexOf('_');
            int yIndexOfUnderscore = y.name.LastIndexOf('_');

            string xID = x.name.Remove(0, xIndexOfUnderscore + 1);
            string yID = y.name.Remove(0, yIndexOfUnderscore + 1);
            xID = new string(xID.Where(char.IsDigit).ToArray());
            yID = new string(yID.Where(char.IsDigit).ToArray());

            if (int.Parse(xID) < int.Parse(yID))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}