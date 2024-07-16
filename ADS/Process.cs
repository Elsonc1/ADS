using System;
using System.Xml;

namespace ADS
{
    public class Process
    {
        XmlDocument doc = new XmlDocument();
        XmlHelper xmlHelper = new XmlHelper();
        LogHelper logHelper = new LogHelper();
        SaveToDataBase saveToDataBase = new SaveToDataBase();
        RemoveAllNameSpace removeAllNameSpace = new RemoveAllNameSpace();
        public void ProcessNFe(string file)
        {
            try
            {
                string xml = File.ReadAllText(file).Replace("xmlns=\"http://www.portalfiscal.inf.br/nfe\"", "").Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", "");
                doc.LoadXml(xml);
                XmlNFe xmlNFe = new XmlNFe();
                xmlNFe.Chave = xmlHelper.GetValueXml("/nfeProc/protNFe/infProt/chNFe", xml);
                xmlNFe.CNPJEmit = xmlHelper.GetValueXml("/nfeProc/NFe/infNFe/emit/CNPJ", xml);
                xmlNFe.CnpjDest = xmlHelper.GetValueXml("/nfeProc/NFe/infNFe/dest/CNPJ", xml);
                xmlNFe.RazaoSocialDestinatario = xmlHelper.GetValueXml("/nfeProc/NFe/infNFe/dest/xNome", xml);
                xmlNFe.CodigoProduto = xmlHelper.GetValueXml("/nfeProc/NFe/infNFe/det/prod/cProd", xml);
                xmlNFe.NomeProduto = xmlHelper.GetValueXml("/nfeProc/NFe/infNFe/det/prod/xProd", xml);
                saveToDataBase.SaveToDatabaseNFe(xmlNFe);
                logHelper.LogMessage($"Arquivo NF-e {file} processado com sucesso", LogHelper.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                logHelper.LogMessage($"Erro ao processar o arquivo NF-e {file} : {ex.Message}", LogHelper.LogLevel.INFO);
                MessageBox.Show($"Erro ao processar o arquivo: {ex.Message}");
            }
        }
        public void ProcessCTe(string file)
        {
            try
            {
                string xml = removeAllNameSpace.RemoverNamespaceCTe(file);
                XmlCTe xmlCTe = new XmlCTe();
                doc.LoadXml(xml);
                xmlCTe.Chave = xmlHelper.GetValueXml("/cteProc/protCTe/infProt/chCTe", xml);
                xmlCTe.CNPJEmit = xmlHelper.GetValueXml("/cteProc/CTe/infCte/emit/CNPJ", xml);
                xmlCTe.RazaoSocialEmit = xmlHelper.GetValueXml("/cteProc/CTe/infCte/emit/xNome", xml);
                xmlCTe.CNPJTomador = xmlHelper.GetValueXml("/cteProc/CTe/infCte/rem/CNPJ", xml);
                xmlCTe.RazaoSocialTomador = xmlHelper.GetValueXml("/cteProc/CTe/infCte/rem/xNome", xml);
                saveToDataBase.SaveToDatabaseCTe(xmlCTe);
                logHelper.LogMessage($"Arquivo CT-e {file} processado com sucesso", LogHelper.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                logHelper.LogMessage($"Erro ao processar o arquivo CT-e {file} : {ex.Message}", LogHelper.LogLevel.INFO);
                MessageBox.Show($"Erro ao processar o arquivo: {ex.Message}");
            }
        }
    }
}
