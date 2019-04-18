using CobWeb.Util.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebKit;

namespace CobWeb.Browser
{
    public class WebKitForm: FormBrowser
    {
       
         
        
        public WebKitForm(MyWebKitBrowser browser) : base(browser)
        {

             panel1.Controls.Add(browser);
             browser.Location = new System.Drawing.Point(0, 0);
             browser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
             browser.MinimumSize = new System.Drawing.Size(20, 20);
             browser.Name = "webBrowser1";
             browser.Size = new System.Drawing.Size(963, 519);
             browser.TabIndex = 1;
             browser.Navigate("http://www.baidu.com");
        }


    }
}
