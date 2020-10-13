using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IVPD.Services
{
    [ServiceContract]

    public interface ISample
    {

        [OperationContract]
        string Test(string s, string v);

         
    }
    public class Sample : ISample
    {
        public string Test(string s,string v)
        {
            string a = s + v;
            return a;

        }
       
    }
    
}
