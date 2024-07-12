using System.Data.SqlClient;
using System.Xml;

namespace ADS
{
    public partial class readXml : Form
    {
        FileInfo _fileInfo = new FileInfo(string.Concat(Directory.GetCurrentDirectory(), "\\config.xml"));
        XmlDocument doc = new XmlDocument();
        string folderContent;
        string connectionString = string.Empty;
        string[] xmlFiles;
        bool sucessMessagesShow = false;

        public readXml()
        {
            InitializeComponent();

            LoadConfig();

            CreateOrUpdateConfig();
        }

        private void LoadConfig()
        {
            string configXml = File.ReadAllText(_fileInfo.FullName);
            try
            {


                doc.LoadXml(configXml);

                textBoxHost.Text = doc.SelectSingleNode("/config/host").InnerText;
                textBoxBanco.Text = doc.SelectSingleNode("/config/banco").InnerText;
                textBoxUser.Text = doc.SelectSingleNode("/config/user").InnerText;
                textBoxPassword.Text = doc.SelectSingleNode("/config/password").InnerText;

                LogMessage("Configuração carregada com sucesso", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao carregar configuração: {ex.Message}", LogLevel.ERROR);
            }
        }
        private void CreateOrUpdateConfig(bool isUpdate = false)
        {
            string path = string.Concat(Directory.GetCurrentDirectory(), "\\config.xml");

            string configXml = $@"<config><host>{textBoxHost.Text}</host><banco>{textBoxBanco.Text}</banco><user>{textBoxUser.Text}</user><password>{textBoxPassword.Text}</password></config>";
            if (!File.Exists(path))
                File.WriteAllText(path, configXml);

            _fileInfo = new FileInfo(path);

            if (isUpdate)
                File.WriteAllText(path, configXml);
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

                        if (rootNode != null)
                        {
                            if (rootNode.Name == "nfeProc")
                            {
                                ProcessNFe(file);
                            }
                            else if (rootNode.Name == "cteProc")
                            {
                                ProcessCTe(file);
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
                LogMessage("Nenhum arquivo XML encontrado na pasta selecionada.", LogLevel.WARN);
            }
        }
        private string RemoverNamespaceCTe(string xmlFile)
        {
            return File.ReadAllText(xmlFile).Replace("xmlns=\"http://www.portalfiscal.inf.br/cte\"", "").Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", "");
        }

        public void SaveToDatabaseNFe(XmlNFe values)
        {
            connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", textBoxHost.Text, textBoxBanco.Text, textBoxUser.Text, textBoxPassword.Text);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO FerramentaProcXml (chave, cnpjEmissor, cnpjDestinatario, razaoSocialDestinatario, cProd, descricaoProd) VALUES (@Chave, @CNPJEmit, @CNPJdest, @RazaoSocialDestinatario, @CodigoProduto, @NomeProduto)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Chave", values.Chave);
                    command.Parameters.AddWithValue("@CNPJEmit", values.CNPJEmit);
                    command.Parameters.AddWithValue("@CNPJdest", values.CnpjDest);
                    command.Parameters.AddWithValue("@RazaoSocialDestinatario", values.RazaoSocialDestinatario);
                    command.Parameters.AddWithValue("@CodigoProduto", values.CodigoProduto);
                    command.Parameters.AddWithValue("@NomeProduto", values.NomeProduto);

                    try
                    {
                        if (!sucessMessagesShow)
                        {
                            MessageBox.Show("Dados inseridos com sucesso!");
                            sucessMessagesShow = true;
                        }
                        command.ExecuteNonQuery();
                        LogMessage("Dados da NF-e inseridos com sucesso no banco de dados.", LogLevel.INFO);

                    }
                    catch (Exception ex)
                    {
                        LogMessage($"Erro ao inserir dados no banco de dados:{ex.Message}", LogLevel.WARN);
                        MessageBox.Show($"Erro ao inserir dados no banco de dados:{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void SaveToDatabaseCTe(XmlCTe values)
        {
            connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", textBoxHost.Text, textBoxBanco.Text, textBoxUser.Text, textBoxPassword.Text);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO cte (chCTe, cnpjEmitente, xNomeEmitente, cnpjRemetente, xNomeRemetente) VALUES (@Chave, @CNPJEmit, @RazaoSocialEmit, @CNPJTomador, @RazaoSocialTomador)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Chave", values.Chave);
                    command.Parameters.AddWithValue("@CNPJEmit", values.CNPJEmit);
                    command.Parameters.AddWithValue("@RazaoSocialEmit", values.RazaoSocialEmit);
                    command.Parameters.AddWithValue("@CNPJTomador", values.CNPJTomador);
                    command.Parameters.AddWithValue("@RazaoSocialTomador", values.RazaoSocialTomador);
                    try
                    {
                        if (!sucessMessagesShow)
                        {
                            MessageBox.Show("Dados inseridos com sucesso!");
                            sucessMessagesShow = true;
                        }
                        command.ExecuteNonQuery();
                        LogMessage("Dados inseridos com sucesso!", LogLevel.INFO);
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"Erro ao inserir dados no banco de dados:{ex.Message}", LogLevel.ERROR);
                        MessageBox.Show($"Erro ao inserir dados no banco de dados:{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void ProcessNFe(string file)
        {
            try
            {
                string xml = File.ReadAllText(file).Replace("xmlns=\"http://www.portalfiscal.inf.br/nfe\"", "").Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", "");
                doc.LoadXml(xml);
                XmlNFe xmlNFe = new XmlNFe();

                xmlNFe.Chave = doc.SelectSingleNode("/nfeProc/protNFe/infProt/chNFe") == null ? string.Empty : doc.SelectSingleNode("/nfeProc/protNFe/infProt/chNFe").InnerText;
                xmlNFe.CNPJEmit = doc.SelectSingleNode("/nfeProc/NFe/infNFe/emit/CNPJ") == null ? string.Empty : doc.SelectSingleNode("/nfeProc/NFe/infNFe/emit/CNPJ").InnerText;
                xmlNFe.CnpjDest = doc.SelectSingleNode("/nfeProc/NFe/infNFe/dest/CNPJ") == null ? string.Empty : doc.SelectSingleNode("/nfeProc/NFe/infNFe/dest/CNPJ").InnerText;
                xmlNFe.RazaoSocialDestinatario = doc.SelectSingleNode("/nfeProc/NFe/infNFe/dest/xNome") == null ? string.Empty : doc.SelectSingleNode("/nfeProc/NFe/infNFe/dest/xNome").InnerText;
                xmlNFe.CodigoProduto = doc.SelectSingleNode("/nfeProc/NFe/infNFe/det/prod/cProd") == null ? string.Empty : doc.SelectSingleNode("/nfeProc/NFe/infNFe/det/prod/cProd").InnerText;
                xmlNFe.NomeProduto = doc.SelectSingleNode("/nfeProc/NFe/infNFe/det/prod/xProd") == null ? string.Empty : doc.SelectSingleNode("/nfeProc/NFe/infNFe/det/prod/xProd").InnerText;

                SaveToDatabaseNFe(xmlNFe);
                LogMessage($"Arquivo NF-e {file} processado com sucesso", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao processar o arquivo NF-e {file} : {ex.Message}", LogLevel.INFO);
                MessageBox.Show($"Erro ao processar o arquivo: {ex.Message}");
            }
        }
        public void ProcessCTe(string file)
        {
            try
            {
                string xml = RemoverNamespaceCTe(file);
                XmlCTe xmlCTe = new XmlCTe();
                doc.LoadXml(xml);

                xmlCTe.Chave = doc.SelectSingleNode("/cteProc/protCTe/infProt/chCTe") == null ? string.Empty : doc.SelectSingleNode("/cteProc/protCTe/infProt/chCTe").InnerText;
                xmlCTe.CNPJEmit = doc.SelectSingleNode("/cteProc/CTe/infCte/emit/CNPJ") == null ? string.Empty : doc.SelectSingleNode("/cteProc/CTe/infCte/emit/CNPJ").InnerText;
                xmlCTe.RazaoSocialEmit = doc.SelectSingleNode("/cteProc/CTe/infCte/emit/xNome") == null ? string.Empty : doc.SelectSingleNode("/cteProc/CTe/infCte/emit/xNome").InnerText;
                xmlCTe.CNPJTomador = doc.SelectSingleNode("/cteProc/CTe/infCte/rem/CNPJ") == null ? string.Empty : doc.SelectSingleNode("/cteProc/CTe/infCte/rem/CNPJ").InnerText;
                xmlCTe.RazaoSocialTomador = doc.SelectSingleNode("/cteProc/CTe/infCte/rem/xNome") == null ? string.Empty : doc.SelectSingleNode("/cteProc/CTe/infCte/rem/xNome").InnerText;

                SaveToDatabaseCTe(xmlCTe);
                LogMessage($"Arquivo CT-e {file} processado com sucesso", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao processar o arquivo CT-e {file} : {ex.Message}", LogLevel.INFO);
                MessageBox.Show($"Erro ao processar o arquivo: {ex.Message}");
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxBanco.Text))
            {
                MessageBox.Show("Está faltando a informação do Banco configuração");
                LogMessage("Está faltando a informação do Banco configuração", LogLevel.ERROR);
            }
            else if (string.IsNullOrEmpty(textBoxHost.Text))
            {
                MessageBox.Show("Está faltando a informação do Host configuração");
                LogMessage("Está faltando a informação do Host configuração", LogLevel.ERROR);
            }
            else if (string.IsNullOrEmpty(textBoxUser.Text))
            {
                MessageBox.Show("Está faltando a informação do Usuário configuração");
                LogMessage("Está faltando a informação do Usuario configuração", LogLevel.ERROR);
            }
            else if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Está faltando a informação do Senha configuração");
                LogMessage("Está faltando a informação do Senha configuração", LogLevel.ERROR);
            }
            else
            {
                CreateOrUpdateConfig(true);
                MessageBox.Show("Configuração realizada com sucesso!", "Salvar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogMessage("Configuração realizada com sucesso!", LogLevel.INFO);
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
                LogMessage("Conexão com o banco de dados realizada com sucesso!", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha na conexão " + ex.Message.ToString(), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMessage($"Houve uma falha de conexão com o banco de dados{ex.Message}", LogLevel.ERROR);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public void LogMessage(string message, LogLevel level)
        {
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "application.log");
            string logMessage = $"{DateTime.Now} [{level}] : {message}";

            const int maxFileSizeInBytes = 5 * 1024 * 1024;

            if (File.Exists(logFilePath) && new FileInfo(logFilePath).Length > maxFileSizeInBytes)
            {
                string arquivFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"application_{DateTime.Now:yyyyMMddHHmmss}.log");
                File.Move(logFilePath, arquivFilePath);
            }

            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        private void selectedFileConfig_Click(object sender, EventArgs e)
        {

        }

        public enum LogLevel
        {
            INFO,
            WARN,
            ERROR
        }
    }
}