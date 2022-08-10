using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2GuiVolcanoRobot
{
    class stringSerial
    {
        private string Convert4dString(int number)
        {
            if (number < 10)
            {
                return "000" + number.ToString();
            }
            else if (number < 100)
            {
                return "00" + number.ToString();
            }
            else if (number < 1000)
            {
                return "0" + number.ToString();
            }
            else
            {
                return number.ToString();
            }
        }
        public string Convert2dString(int number)
        {
            if (number < 10)
            {
                return "0" + number.ToString();
            }
            else
            {
                return number.ToString();
            }
        }
        public string putFCS(string getText)//dapatkan FCS
        {
            string oldSTRING = "";
            char FCS = '\0';
            int i = 0;
            for (i = 0; i < getText.Length; i++)
            {
                if (i == 0)
                {
                    FCS = getText[0];
                }
                else
                {
                    FCS = (char)(FCS ^ getText[i]);//operasikan xor tiap data yang masuk
                }
            }
            int getString = (int)FCS;//data yang dalam bentuk char ubah ke angka
            if (getString <= 16)
            {
                oldSTRING = "0";
            }
            oldSTRING = oldSTRING + Convert.ToString(getString, 16);//ubah ke dalam bentuk hexa;
            return oldSTRING.ToUpper();
        }
        private string clearBuffNull(string s)
        {
            string newString = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\0')
                {
                    return newString;
                }
                else
                {
                    newString = newString + s[i];
                }
            }
            return newString;
        }
        public string checkFCS2(string getText)
        {
            string mystring = "#";
            mystring = mystring + clearBuffNull(getText);
            mystring = mystring.Substring(0, mystring.Length - 2);
            return putFCS(mystring);
        }
        public bool checkFCS(string getText)
        {
            try
            {
                int k = 0;
                string mystring = "#";
                string textFCS = "";
                mystring = mystring + clearBuffNull(getText);
                textFCS = mystring.Substring(mystring.Length - 2);
                mystring = mystring.Substring(0, mystring.Length - 2);
                k = string.Compare(putFCS(mystring), textFCS);
                if (k == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        public int parseStringToInt(string s, int strt, int endd)
        {
            try
            {

                string sConvert = "";
                sConvert = s.Substring(strt, endd);
                return Convert.ToInt32(sConvert);
            }
            catch
            {
                return 0;
            }
        }
        public bool checkUNIT(int unit, string getText)
        {
            int TxtUnit = getText[0] - 48;
            if (unit == TxtUnit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*
        //public const string code_attitude = "00";
        //public const string code_humadity = "01";
        //public const string code_airquality = "02";
        //public const string code_battery = "03";
        //public const string code_compass = "04";
        //public const string code_temperature = "05";
        //public const string code_h2s = "06";
        //public const string code_speed = "07";
        //public const string code_roll = "08";
        //public const string code_pitch = "09";
        //public const string code_yaw = "10";
        //public const string unitNumb = "0";
        public stringSerial()
        {

        }
        */
    }
}
