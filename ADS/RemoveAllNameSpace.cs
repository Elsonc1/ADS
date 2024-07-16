using System;

namespace ADS
{
    public class RemoveAllNameSpace
    {
        public string RemoverNamespaceCTe(string xmlFile)
        {
            return File.ReadAllText(xmlFile).Replace("xmlns=\"http://www.portalfiscal.inf.br/cte\"", "").Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", "");
        }
    }
}
