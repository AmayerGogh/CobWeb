using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Core.Process
{
    public interface IBrowserBase
    {
        void Stop();
        void Close();


        string GetResult();
        void SetResult(string result);
        bool IsWorking();
        bool SetWorking(bool iswork);
        bool IsShowForm();

        void ExcuteRecord(string txt);
        void AddAssistAction(Action action);
    }
}
