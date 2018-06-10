namespace pe
{
    public class CommandStrings
    {
        public static bool CheckString(string _str, string[] _strArray)
        {
            for (int i = 0; i < _strArray.Length; i++)
            {
                if (_str == _strArray[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static readonly string[] FocusAndMoveTo = {
            "MOVE",
            "MOV",
            "GO",
            "WARP",
            "FLYTO",
            "CD",
            "FOCUS"
        };

        public static readonly string[] MoveDirections = {
            "UP",
            "DOWN",
            "LEFT",
            "RIGHT",
            "FORWARD",
            "BACKWARD"
        };

        public static readonly string[] ClearLog = {
            "C",
            "CLS",
            "CLEAR",
            "CLEARLOG",
            "CLC",
        };

        public static readonly string[] LS = {
            "LS",
            "LIST",
            "QUADRANT",
            "WHEREAMI",
            "SURROUNDINGS"
        };

        public static readonly string[] Explode = {
            "E",
            "EXPLODE"
        };

        public static readonly string[] LSBack = {
            "..",
            "../",
            //"..\",
        };

        public static readonly string[] Info = {
            "I",
            "INFO",
            "SCAN",
            "OBSERVE",
            "WHATIS",
        };
    }
}
