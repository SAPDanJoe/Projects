using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            //using System.Security.Cryptography;
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            RSAParameters keyPair = provider.ExportParameters(true);
            string keys = provider.ToXmlString(true);
            int i = 0;
        }
    }
}


