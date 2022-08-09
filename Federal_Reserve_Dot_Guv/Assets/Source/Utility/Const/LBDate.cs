
namespace lb
{
    public class LBDate
    {
        //definitions
        public enum EMonth
        {
            Jan,
            Feb,
            Mar,
            Apr,
            May,
            Jun,
            Jul,
            Aug,
            Sep,
            Oct,
            Nov,
            Dec
        }
        public enum EDateFormat
        {
            DD_MM_YYYY,
            MM_DD_YYYY
        }
        private readonly short[] c_DaysPerMonth =
        {
            31,//january
            28, //**29 days every 4th year
            31,//march
            30,//april
            31,//may
            30,//june
            31,//july
            31,//august
            30,//september
            31,//october
            30,//november
            31//december
        };


        //current date
        public short Day;
        public short Month;
        public short Year;
        public readonly short c_FirstYearOfTime = 2028;


        public LBDate()
        {
            Day = 1;
            Month = 1;
            Year = c_FirstYearOfTime;
        }

        //advance day pointer
        public void NextDay()
        {
            Day++;
            if (Day > c_DaysPerMonth[Month-1])
            {
                Day = 1;
                Month++;
                if (Month > 12)
                {
                    Month = 1;
                    Year++;
                }
            }
        }
        //gives date string in format
        public string PrintDate(EDateFormat format)
        {
            switch (format)
            {
                case EDateFormat.DD_MM_YYYY:
                    return Day.ToString("D2") + "/" + Month.ToString("D2") + "/" + Year.ToString("D4");
                case EDateFormat.MM_DD_YYYY:
                    return Month.ToString("D2") + "/" + Day.ToString("D2") + "/" + Year.ToString("D4");
                default:
                    return "??/??/??";
            }
        }
    }
}