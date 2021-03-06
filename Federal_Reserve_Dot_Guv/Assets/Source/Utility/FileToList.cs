﻿using System.Collections.Generic;
using UnityEngine;

namespace util
{
    public class FileToList : SingletonBehavior<FileToList>
    {
        private static string[] Load(TextAsset _asset, ref List<string> namesManaged)
        {
            string[] r = _asset.text.Split('\n', '\r');

            namesManaged = new List<string>(r);

            for (int i = 0; i < namesManaged.Count; i++)
            {
                if (namesManaged[i] == "" || namesManaged[i] == "\n")
                {
                    namesManaged.RemoveAt(i);
                }
            }

            return r;
        }
    }
}
