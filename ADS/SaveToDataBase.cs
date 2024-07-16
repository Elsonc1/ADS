using System;
using System.Data.SqlClient;

namespace ADS
{
    public class SaveToDataBase
    {
        string connectionString = string.Empty;
        bool sucessMessagesShow = false;
        LogHelper logHelper = new LogHelper();
        XmlConfig xmlConfig = new XmlConfig();
        public void SaveToDatabaseCTe(XmlCTe values)
        {
            connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", xmlConfig.Host, xmlConfig.Banco, xmlConfig.User, xmlConfig.Password);
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
                        logHelper.LogMessage("Dados inseridos com sucesso!", LogHelper.LogLevel.INFO);
                    }
                    catch (Exception ex)
                    {
                        logHelper.LogMessage($"Erro ao inserir dados no banco de dados:{ex.Message}", LogHelper.LogLevel.ERROR);
                        MessageBox.Show($"Erro ao inserir dados no banco de dados:{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void SaveToDatabaseNFe(XmlNFe values)
        {
            connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", xmlConfig.Host, xmlConfig.Banco, xmlConfig.User, xmlConfig.Password);
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
                        logHelper.LogMessage("Dados da NF-e inseridos com sucesso no banco de dados.", LogHelper.LogLevel.INFO);
                    }
                    catch (Exception ex)
                    {
                        logHelper.LogMessage($"Erro ao inserir dados no banco de dados:{ex.Message}", LogHelper.LogLevel.WARN);
                        MessageBox.Show($"Erro ao inserir dados no banco de dados:{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
