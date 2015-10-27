using System;
using System.Data;
using System.Data.SqlClient;

namespace AcessoBancoDados
{
    public class AcessoDadosSqlServer
    {
        // Criar Conexão
        private SqlConnection CriarConexao()
        {
            return new SqlConnection(Properties.Settings.Default.SqlServer_ConnectionSetting);
        }

        // Criar Conexão (Sobrecarga)
        private SqlConnection CriarConexao(string stringConnection)
        {
            return new SqlConnection(stringConnection);
        }

        // Parâmentros que vão para o banco
        private SqlParameterCollection sqlParameterCollection = new SqlCommand().Parameters;

        // Adicionar novo paramentro
        public void AdcionarParametro(string nomeParametro, object valorParametro)
        {
            sqlParameterCollection.Add(new SqlParameter(nomeParametro, valorParametro));
        }

        // Limpar paramentros
        public void LimparParametros()
        {
            sqlParameterCollection.Clear();
        }

        // Persistência - Inserir - Alterar, Excluir
        public object ExecutarManipulacao(CommandType commandType, string nomeStoredProssedureOuTextoSql)
        {
            try
            {
                // Criar conexão
                SqlConnection sqlConnection = CriarConexao();

                // Abrir conexão
                sqlConnection.Open();

                // Criar um sqlCommand
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                // Preparando sqlCommand para ser entregue ao banco
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = nomeStoredProssedureOuTextoSql;
                sqlCommand.CommandTimeout = 7200; // Segundos

                // Adcionado paramentros ao sqlCommand
                foreach (SqlParameter sqlParameter in sqlParameterCollection)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                }

                // Retornando resultado da manupulação
                return sqlCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        // Consultar registros do banco de dados
        public DataTable ExecutarConsulta(CommandType commandType, string nomeStoredProssedureOuTextoSql)
        {
            try
            {
                // Criar conexão
                SqlConnection sqlConnection = CriarConexao();

                // Abrir conexão
                sqlConnection.Open();

                // Criar um sqlCommand
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                // Preparando sqlCommand para ser entregue ao banco
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = nomeStoredProssedureOuTextoSql;
                sqlCommand.CommandTimeout = 7200; // Segundos

                // Adcionado paramentros ao sqlCommand
                foreach (SqlParameter sqlParameter in sqlParameterCollection)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                }

                // Adaptador de Dados SQL
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // DataTable para ser retornada
                DataTable dataTable = new DataTable();

                // Preenchendo dataTable
                sqlDataAdapter.Fill(dataTable);

                // Retornando resultado da manupulação
                return dataTable;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
