using Newtonsoft.Json;
using System.Collections.Generic;

namespace dd
{
    public static class JsonHelper
    {
        //Json.NET (netwonsoft) analog in unity. it's on the asset store.
        //https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347
        //
        //Note: may have to remove this and make something 'good enough'
        internal static DialogData[] FromJsonDialogData(string json)
        {
            return JsonConvert.DeserializeObject<List<DialogData>>(json).ToArray();
        }
    }
}