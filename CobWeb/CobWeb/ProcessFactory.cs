
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb
{
    public class ProcessFactory
    {
        static readonly Dictionary<string, Type> ProcessBaseDic = new Dictionary<string, Type>();
        static readonly Dictionary<string, Type> ProcessBase2Dic = new Dictionary<string, Type>();

        static ProcessFactory()
        {
            //{
            //    var types = CommonCla.FindAllClassByInterface<IProcessBase>();
            //    foreach (var type in types)
            //    {
            //        if (!string.IsNullOrWhiteSpace(type.Name))
            //            if (!ProcessBaseDic.ContainsKey(type.Name))
            //                ProcessBaseDic.Add(type.Name, type);
            //    }
            //}

            //{
            //    var types = CommonCla.FindAllClassByInterface<IProcessBase2>();
            //    foreach (var type in types)
            //    {
            //        if (!string.IsNullOrWhiteSpace(type.Name))
            //            if (!ProcessBase2Dic.ContainsKey(type.Name))
            //                ProcessBase2Dic.Add(type.Name, type);
            //    }
            //}
        }

        //public static IProcessBase GetProcessByMethod(FormSpider form, ParamModel paramModel)
        //{
        //    if (!ProcessBaseDic.ContainsKey(paramModel.Method))
        //        throw new Exception(ArtificialCode.A_UnknownMethod.ToString());

        //    var process = (IProcessBase)Activator.CreateInstance(ProcessBaseDic[paramModel.Method],
        //        form, paramModel);

        //    return process;
        //}

        //public static IProcessBase2 GetProcessByMethod(ArtificialParamModel paramModel)
        //{
        //    if (!ProcessBase2Dic.ContainsKey(paramModel.Method))
        //        throw new Exception(ArtificialCode.A_UnknownMethod.ToString());

        //    var process = (IProcessBase2)Activator.CreateInstance(ProcessBase2Dic[paramModel.Method]);

        //    return process;
        //}
    }
}
