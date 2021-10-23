using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace StevenUtility
{
    /********************************************//**
     * 
     * @brief  스레드에 안전한 텍스트 업데이트
     * 
     *         Serial Port의 RX 이벤트와 같이 UI 스레드가 아닌 다른 스레드에서
     *         폼의 텍스트박스 등의 내용을 수정하려고 하면 스레드 에러가 발생한다.
     *         이를 해결하기 위해서 델리게이션을 써야 하는데,
     *         이 클래스에 델리게이트 함수가 선언되어 있다.
     *         
     * @usage
     * -------------------------
     * 단순히 그냥 사용하면 된다.
     * 
     * SafeUX.SetText( TxtTest, "aaa");
     * SafeUx.AddText( TxtTest, "bbb");
     * 
     *       
     * @author Steven (Heungseok) Park
     * 
     * @history
     *      -. 2019.03.08  First released.
     *      
     *  
     * @copyright   FURONTEER, Korea.   All rights reserved.
     * 
     * *********************************************************************************/
    static public class SafeUX
    {
        // Cross threadSafe
        private delegate void SafeSetText(Control ctl, String text);
        static public void SetText(Control ctl, String text)
        {
            if (ctl.InvokeRequired)
            {
                SafeSetText d = new SafeSetText(SetText);
                ctl.Invoke(d, ctl, text);
            }
            else
                ctl.Text = text;
        }

        private delegate void SafeAddText(Control ctl, String text);
        static public void AddText(Control ctl, String text)
        {

            if (ctl.InvokeRequired)
            {

                SafeSetText d = new SafeSetText(AddText);
                ctl.Invoke(d, ctl, text);
            }
            else
            {
                string cur_str = ctl.Text;
                cur_str += text;    // + "\r\n";
                ctl.Text = cur_str;
                if (ctl is RichTextBox)
                {
                    RichTextBox b = (RichTextBox)ctl;
                    b.Select(b.Text.Length, 0);
                    b.ScrollToCaret();
                }
                else if (ctl is TextBox)
                {
                    TextBox b = (TextBox)ctl;
                    b.Select(b.Text.Length, 0);
                    b.ScrollToCaret();
                }
            }
        }

        static public void AddTextLine(Control ctl, String text)
        {
            AddText(ctl, (text + Environment.NewLine));
            //AddText(ctl, (text + "\r\n"));
        }

    }
}
