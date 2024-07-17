using System.Data.SqlClient;
using System.Xml;

namespace ADS
{
    public partial class readXml : Form
    {
        FileInfo _fileInfo = new FileInfo(string.Concat(Directory.GetCurrentDirectory(), "\\config.xml"));
        XmlDocument doc = new XmlDocument();
        XmlHelper xmlHelper = new XmlHelper();
        LogHelper logHelper = new LogHelper();
        Process process = new Process();
        Config config = new Config();
        XmlConfig xmlConfig = new XmlConfig();
        string folderContent;
        string connectionString = string.Empty;
        string[] xmlFiles;
        string configFilePath = string.Empty;
        public readXml()
        {
            InitializeComponent();
            config.LoadConfig(configFilePath);
            config.CreateOrUpdateConfig();
        }
        private void selectionButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox1.Text = folderDialog.SelectedPath;
                    folderContent = folderDialog.SelectedPath;
                    xmlFiles = Directory.GetFiles(folderContent, "*.xml");
                }
            }
        }
        private void processButton_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Selecione uma pasta primeiro!");
                return;
            }
            else if (Directory.Exists(path) || xmlFiles != null && xmlFiles.Length > 0)
            {
                foreach (string file in xmlFiles)
                {
                    try
                    {
                        doc.Load(file);
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                        nsmgr.AddNamespace("cte", "http://www.portalfiscal.inf.br/cte");
                        nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

                        XmlNode rootNode = doc.DocumentElement;

                        List<string> listarDocInvalid = new List<string>();

                        if (rootNode != null)
                        {
                            switch (rootNode.Name)
                            {
                                case "nfeProc":
                                    process.ProcessNFe(file);
                                    break;
                                case "cteProc":
                                    process.ProcessCTe(file);
                                    break;
                                default:
                                    listarDocInvalid.Add(file);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao processar o arquivo: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhum arquivo XML encontrado na pasta selecionada.");
                logHelper.LogMessage("Nenhum arquivo XML encontrado na pasta selecionada.", LogHelper.LogLevel.WARN);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxBanco.Text))
            {
                MessageBox.Show("Está faltando a informação do Banco configuração");
                logHelper.LogMessage("Está faltando a informação do Banco configuração", LogHelper.LogLevel.ERROR);
            }
            else if (string.IsNullOrEmpty(textBoxHost.Text))
            {
                MessageBox.Show("Está faltando a informação do Host configuração");
                logHelper.LogMessage("Está faltando a informação do Host configuração", LogHelper.LogLevel.ERROR);
            }
            else if (string.IsNullOrEmpty(textBoxUser.Text))
            {
                MessageBox.Show("Está faltando a informação do Usuário configuração");
                logHelper.LogMessage("Está faltando a informação do Usuario configuração", LogHelper.LogLevel.ERROR);
            }
            else if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Está faltando a informação do Senha configuração");
                logHelper.LogMessage("Está faltando a informação do Senha configuração", LogHelper.LogLevel.ERROR);
            }
            else
            {
                config.CreateOrUpdateConfig(true);
                MessageBox.Show("Configuração realizada com sucesso!", "Salvar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logHelper.LogMessage("Configuração realizada com sucesso!", LogHelper.LogLevel.INFO);
            }
        }
        private void buttonTest_Click(object sender, EventArgs e)
        {
            connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", textBoxHost.Text, textBoxBanco.Text, textBoxUser.Text, textBoxPassword.Text);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                MessageBox.Show("Conexão realizada!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logHelper.LogMessage("Conexão com o banco de dados realizada com sucesso!", LogHelper.LogLevel.INFO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha na conexão " + ex.Message.ToString(), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logHelper.LogMessage($"Houve uma falha de conexão com o banco de dados{ex.Message}", LogHelper.LogLevel.ERROR);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        private void selectedFileConfig_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    configFilePath = openFileDialog.FileName;
                    try
                    {
                        string configXml = File.ReadAllText(configFilePath);
                        doc.LoadXml(configXml);
                        xmlConfig.Host = doc.SelectSingleNode("/config/host").InnerText;
                        xmlConfig.Banco = doc.SelectSingleNode("/config/banco").InnerText;
                        xmlConfig.User = doc.SelectSingleNode("/config/user").InnerText;
                        xmlConfig.Password = doc.SelectSingleNode("/config/password").InnerText;
                        logHelper.LogMessage("Configuração carregada com sucesso a partir do arquivo selecionado.", LogHelper.LogLevel.INFO);
                        MessageBox.Show("Configuração carregada com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        config.LoadConfig(configXml);
                    }
                    catch (Exception ex)
                    {
                        logHelper.LogMessage($"Erro ao carregar configuração: {ex.Message}", LogHelper.LogLevel.ERROR);
                        MessageBox.Show($"Erro ao carregar configuração: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}