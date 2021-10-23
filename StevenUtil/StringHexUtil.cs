using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StevenUtility
{
    /// String-Hex 변환 Utility
    ///     -. 문자열 변환 
    ///             (1) ByteArrToString : {0x41, 0x42, 0x43, 0x44} => "ABCD"
    ///             (2) ByteArrToHexString : {0x03, 0xAB, 0xFF, 0x04} => " 03-AB-FF-04"
    ///             (3) StringToByteArr : "03 0f 04 01" => {0x03, 0x0F, 0x04, 0x01}
    static class StringHexUtil
    {
        static public byte[] ConvertStringToByteArr(string str, char delimiter = '-')
        {
            if (str == null) return null;
            if (str == "") return null;
            
            string[] str_arr = str.Split(delimiter);
            List<byte> data = new List<byte>();
            // byte[] data = new byte[0];
            try
            {
                foreach (string hex in str_arr)
                {
                    if (hex == "") continue;
                    if ((hex == "\n") || hex.ElementAt(0) == '#') break;

                    int val = Convert.ToInt16(hex, 16);
                    data.Add((byte)val);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return data.ToArray();
        }

        static public string ConvertByteArrToString(byte[] data, char delimiter = '-')
        {
            if (data == null) return (string)"";

            string str = Encoding.Default.GetString(data);
            //string str = Encoding.Unicode.GetString(data);
            string del = "" + delimiter;
            str = str.Replace("-", del);
            return str;
        }

        static public string ConvertByteArrToHexString(byte[] data, char delimiter = '-')
        {
            if (data == null) return (string)"";
           
            string str = BitConverter.ToString(data);
            string del = "" + delimiter;
            str = str.Replace("-", del);
            return str;
        }

        static public string RemoveComent(string str)
        {
            if (str.Length == 0) return "";
            if (str.ElementAt(0) == '#') return "";
            int i = str.IndexOf('#');
            string resultStr = str;
            if ( (i>0) && (i<str.Length) )
            {
                resultStr = str.Substring(0, i);
            }
            return resultStr;
        }
    }
}
