using System;
using System.Runtime.CompilerServices;

namespace ADS
{
    public class Config
    {
        FileInfo _fileInfo = new FileInfo(string.Concat(Directory.GetCurrentDirectory(), "\\config.xml"));
        Form form = new Form();
        XmlConfig xmlConfig = new XmlConfig();
        XmlHelper xmlHelper = new XmlHelper();
        string loadConfig = string.Empty;
        public string LoadConfig(string loadConfig)
        {
            LogHelper logHelper = new LogHelper();
            string configXml = SaveConfig(loadConfig);
            configXml = File.ReadAllText(_fileInfo.FullName);
            try
            {
                xmlConfig.Host = xmlHelper.GetValueXml("/config/host", configXml);
                xmlConfig.Banco = xmlHelper.GetValueXml("/config/banco", configXml);
                xmlConfig.User = xmlHelper.GetValueXml("/config/user", configXml);
                xmlConfig.Password = xmlHelper.GetValueXml("/config/password", configXml);
                logHelper.LogMessage("Configuração carregada com sucesso", LogHelper.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                logHelper.LogMessage($"Erro ao carregar configuração: {ex.Message}", LogHelper.LogLevel.ERROR);
            }
            return configXml;
        }
        public void CreateOrUpdateConfig(bool isUpdate = false)
        {
            string path = string.Concat(Directory.GetCurrentDirectory(), "\\config.xml");
            string configXml = $@"<config><host>{xmlConfig.Host}</host><banco>{xmlConfig.Banco}</banco><user>{xmlConfig.User}</user><password>{xmlConfig.Password}</password></config>";
            if (!File.Exists(path))
                File.WriteAllText(path, configXml);
            _fileInfo = new FileInfo(path);
            if (isUpdate)
                File.WriteAllText(path, configXml);
        }
        public string SaveConfig(string saveConfig)
        {
            saveConfig = string.Concat(Directory.GetCurrentDirectory(), "\\config.xml");
            loadConfig = saveConfig;
            return File.ReadAllText(loadConfig);
        }
    }
}
