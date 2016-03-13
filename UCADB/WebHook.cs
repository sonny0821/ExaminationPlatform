using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UCADB
{
    public class WebHook
    {
        public static void Hook(ref WebBrowser wb, HtmlElementEventHandler handler)
        {
            foreach (HtmlElement he in wb.Document.All)
            {
                if (he.GetAttribute("SubmitButton") != null && he.GetAttribute("SubmitButton").Trim() != "")
                {


                    he.Click += handler;
                }
            }
        }

        public static void Hook(ref WebBrowser wb, string hookAttr, HtmlElementEventHandler handler)
        {
            foreach (HtmlElement he in wb.Document.All)
            {
                if (he.GetAttribute(hookAttr) != null && he.GetAttribute(hookAttr).Trim() != "")
                {


                    he.Click += handler;
                }
            }
        }

    }
}
