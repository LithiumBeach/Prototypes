using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Localization
{
    public static class LocalizationData
    {
        public static List<int> s_Ids;

        public static void InitializeIDs()
        {
            //stub
            s_Ids = new List<int>();
        }

        //find the next invalid id.
        public static int GetNextUniqueID()
        {
            //nevative values are reserved for invalid, start at 0.
            int iterID = 0;

            while (iterID < int.MaxValue)
            {
                //if the key doesn't exist
                if (!s_Ids.Contains(iterID))
                {
                    //this is the next key
                    return iterID;
                }
                iterID++;
            }
            MessageBox.Show("uh oh.. all the keys seems to be taken. invalid key -1 will be returned.");
            return -1;
        }
    }
}
