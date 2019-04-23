using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NamedPipeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            FormServer formClient = new FormServer();

            Application.Run(formClient);
        }
    }
}
