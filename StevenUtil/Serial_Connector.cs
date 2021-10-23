using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace StevenUtility
{
    namespace SerialConnector
    {
        /// <summary>
        /// Class Serial_Connector
        ///    시리얼 통신관련 유틸리티
        ///     -. 시리얼 포트 리스트 확인, open, close

        /// </summary>
        public class Serial_Connector
        {
            public SerialPort com;

            public bool IsOpen
            { get { return com.IsOpen; } }

            /**  ****************************************************
             * 
             *          CONSTRUCTOR and Private Functions
             *      
             *      *************************************************/

            /// <summary>
            /// Creator
            /// </summary>
            public Serial_Connector()
            {
                InitializeCompoenets();     // initialize
            }

            private void InitializeCompoenets()
            {

                // --- Port Default set ---
                com = new SerialPort();
                com.PortName = "UNKOWN";
                com.BaudRate = 115200;
                com.Encoding = Encoding.Default;
                com.Parity = System.IO.Ports.Parity.None;
                com.DataBits = 8;
                com.StopBits = System.IO.Ports.StopBits.One;
            }

            /* ************************************************
             * 
             *      PUBLIC FUNCTIONS
             *      
             * **********************************************/

            //public byte[] ConvertStringToByteArr(string str, char delimiter='-')
            //{
            //    string[] str_arr = str.Split(delimiter);
            //    List<byte> data = new List<byte>();
            //    // byte[] data = new byte[0];
            //    try
            //    {
            //        foreach (string hex in str_arr)
            //        {
            //            if (hex == "") continue;
            //            if ( (hex == "\n") || hex.ElementAt(0) == '#') break;

            //            int val = Convert.ToInt16(hex, 16);
            //            data.Add((byte)val);
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        return null;
            //    }

            //    return data.ToArray();
            //}

            //public string ConvertByteArrToString(byte[] data, char delimiter='-')
            //{
            //    string str = Encoding.Default.GetString(data);
            //    string del = "" + delimiter;
            //    str = str.Replace("-", del);
            //    return str;
            //}

            //public string ConvertByteArrToHexString(byte[] data, char delimiter='-')
            //{
            //    string str = BitConverter.ToString(data);
            //    string del = "" + delimiter;
            //    str = str.Replace("-", del);
            //    return str;
            //}

            /// <summary>
            /// @Method  
            ///     get port name list
            /// </summary>
            /// <returns>string list</returns>
            public List<String> get_port_names()
            {
                List<String> port_name_list = new List<string>();

                port_name_list.Clear();
                foreach (string name in SerialPort.GetPortNames())
                {
                    port_name_list.Add(name);
                }
                return port_name_list;
            }

            /// <summary>
            /// @Method 
            ///     Update port list combo box
            /// </summary>
            /// <param name="cb"></param>
            public void Refresh_port_list(ComboBox cb)
            {
                List<String> names = get_port_names();
                names.Sort();
                cb.Items.Clear();
                foreach (String str in names)
                {
                    cb.Items.Add(str);
                }

                // check if an available port is selected
                bool matched = false;
                foreach (String str in names)
                {
                    if (cb.Text == str)
                    {
                        matched = true;
                        break;
                    }
                }

                if (matched == false)
                {
                    if ((cb.Text == "") && (cb.Items.Count > 0)) cb.SelectedIndex = 0;
                    else if (cb.Items.Count == 0) cb.Text = "";
                    else
                    {
                        cb.SelectedIndex = 0;
                    }
                }
            }

            public string Get_port_status_string()
            {
                if (com.IsOpen == true)
                {
                    string str = com.PortName + " " + com.BaudRate.ToString();
                    return str;
                }
                else
                {
                    string str = "Port Closed";
                    return str;
                }
            }
        }
    }
}