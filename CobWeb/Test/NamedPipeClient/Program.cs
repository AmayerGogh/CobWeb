using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NamedPipeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            FormClient formClient = new FormClient();
            
            Application.Run(formClient);
        }
    }
}
