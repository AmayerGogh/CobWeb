using CobWeb.AProcess;
using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util;
using CobWeb.Util.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.Core.Process
{
    public class ProcessFactory
    {
        static readonly Dictionary<string, Type> ProcessBaseDic = new Dictionary<string, Type>();
        static readonly Dictionary<string, Type> ProcessBase2Dic = new Dictionary<string, Type>();
        public static Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
        static Dictionary<string, object> classs = new Dictionary<string, object>();
        static ProcessFactory()
        {           
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                foreach (var t in type.GetInterfaces())
                {
                    if (t == typeof(IProcessBase))
                    {
                        ProcessBaseDic.Add(t.Name, t);
                        break;
                    }
                    else if(t == typeof(IProcessBase2))
                    {
                        ProcessBase2Dic.Add(t.Name, t);
                        break;
                    }
                }
            } 
        }
        public static IProcessBase GetProcessByMethod(FormBrowser formBrowser, SocketRequestModel paramModel)
        {

            if (!string.IsNullOrEmpty(paramModel.FileName))
            {
                paramModel.FileName = "TaobaoSpider.dll";
                paramModel.IsUseForm = true;
                paramModel.Method = "TaoBaoSpider";
                if (string.IsNullOrEmpty(paramModel.FileName))
                {
                    return GetProcessInAssembly(formBrowser, paramModel);
                }
            }
       
            if (!ProcessBaseDic.ContainsKey(paramModel.Method))
                throw new Exception(SocketResponseCode.A_UnknownMethod.ToString());
            var process = (IProcessBase)Activator.CreateInstance(ProcessBaseDic[paramModel.Method],
                formBrowser, paramModel);
            return (IProcessBase)process;
        }
        //public static IProcessBase GetProcessByMethod(FormSpider form, ParamModel paramModel)
        //{
        //    if (!ProcessBaseDic.ContainsKey(paramModel.Method))
        //        throw new Exception(ArtificialCode.A_UnknownMethod.ToString());
        //    var process = (IProcessBase)Activator.CreateInstance(ProcessBaseDic[paramModel.Method],
        //        form, paramModel);
        //    return process;
        //}
        public static IProcessBase2 GetProcessByMethod2(SocketRequestModel paramModel)
        {
            if (!ProcessBase2Dic.ContainsKey(paramModel.Method))
                throw new Exception(SocketResponseCode.A_UnknownMethod.ToString());
            var process = (IProcessBase2)Activator.CreateInstance(ProcessBase2Dic[paramModel.Method]);
            return process;
        }
        //ParamModel paramModel = new ParamModel()
        //{
        //    FileName = "TaobaoSpider.dll",
        //    IsUseForm = true,
        //    Method = "TaoBaoSpider"
        //};
        public static IProcessBase GetProcessInAssembly(FormBrowser formBrowser, SocketRequestModel paramModel)
        {
            if (!dictionary.ContainsKey(paramModel.Method))
            {
                string dllPath = Path.Combine(Application.StartupPath, "CobProcess", paramModel.FileName);
                Assembly assembly = Assembly.LoadFrom(dllPath);
                var types = assembly.GetTypes();
                foreach (var item in types)
                {
                    var i = item.BaseType;
                    if (i == typeof(ProcessBaseUseBrowser))
                    {
                        dictionary.Add(item.Name, item);
                    }
                }
            }
            if (!dictionary.ContainsKey(paramModel.Method))
            {
                Console.WriteLine("未能加载到");
            }
            var type = dictionary[paramModel.Method];
            var process = Activator.CreateInstance(type, formBrowser, paramModel);
            var thisP = process as IProcessBase;
            thisP?.Begin();
            return (IProcessBase)process;
        }
    }
}
