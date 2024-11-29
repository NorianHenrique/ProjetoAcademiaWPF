using ProjetoAcademiaWPF.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcademiaWPF.DataAccess
{
    public class LogradouroRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }
        public LogradouroRepository()
        {
            // buscar os dados de connectionstring e provider do arquivo de configuração
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
            // define a factory, ou seja, o provedor de dados - Mysql, SqlServer, etc
            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        // método para carregar os dados aqui
        public List<Logradouro> GetAll()
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
            conexao.Open();
            comando.CommandText = @"SELECT id, cep, logradouro, bairro, cidade, uf, pais FROM logradouro";
            using var reader = comando.ExecuteReader();
            // carrega os dados para ser retornado e utilizado no databinding
            List<Logradouro> dadosRetorno = new List<Logradouro>();
            while (reader.Read())
            {
                dadosRetorno.Add(new Logradouro
                {
                    Id = reader.GetInt32(0),
                    Cep = reader.GetString(1),
                    Nome = reader.GetString(2),
                    Bairro = reader.GetString(3),
                    Cidade = reader.GetString(4),
                    Uf = reader.GetString(5),
                    Pais = reader.GetString(6)
                });
            }
            return dadosRetorno;
        }

        // método para inserir os dados aqui
        public void Add(Logradouro dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetro (@campo e valor)
            var cep = comando.CreateParameter(); cep.ParameterName = "@cep";
            cep.Value = dado.Cep; comando.Parameters.Add(cep);

            var pais = comando.CreateParameter(); pais.ParameterName = "@pais";
            pais.Value = dado.Pais; comando.Parameters.Add(pais);

            var uf = comando.CreateParameter(); uf.ParameterName = "@uf";
            uf.Value = dado.Uf; comando.Parameters.Add(uf);

            var cidade = comando.CreateParameter(); cidade.ParameterName = "@cidade";
            cidade.Value = dado.Cidade; comando.Parameters.Add(cidade);

            var bairro = comando.CreateParameter(); bairro.ParameterName = "@bairro";
            bairro.Value = dado.Bairro; comando.Parameters.Add(bairro);

            var logradouro = comando.CreateParameter(); logradouro.ParameterName = "@logradouro";
            logradouro.Value = dado.Nome; comando.Parameters.Add(logradouro);

            conexao.Open();
            comando.CommandText = @"INSERT INTO logradouro (cep, logradouro, bairro, cidade, uf, pais) VALUES (@cep, @logradouro, @bairro, @cidade, @uf, @pais);";
            //Executa o script na conexão e armazena o número de linhas afetadas.
            var linhas = comando.ExecuteNonQuery();
        }

        // método para atualizar os dados aqui
        public void Update(Logradouro dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão

            // Adiciona parâmetro (@campo e valor)
            var id = comando.CreateParameter(); id.ParameterName = "@id"; id.Value = dado.Id; comando.Parameters.Add(id);
            var cep = comando.CreateParameter(); cep.ParameterName = "@cep"; cep.Value = dado.Cep; comando.Parameters.Add(cep);
            var pais = comando.CreateParameter(); pais.ParameterName = "@pais"; pais.Value = dado.Pais; comando.Parameters.Add(pais);
            var uf = comando.CreateParameter(); uf.ParameterName = "@uf"; uf.Value = dado.Uf; comando.Parameters.Add(uf);
            var cidade = comando.CreateParameter(); cidade.ParameterName = "@cidade"; cidade.Value = dado.Cidade; comando.Parameters.Add(cidade);
            var bairro = comando.CreateParameter(); bairro.ParameterName = "@bairro"; bairro.Value = dado.Bairro; comando.Parameters.Add(bairro);
            var logradouro = comando.CreateParameter(); logradouro.ParameterName = "@logradouro"; logradouro.Value = dado.Nome; comando.Parameters.Add(logradouro);

            conexao.Open();
            // Realiza o UPDATE
            comando.CommandText = @"UPDATE logradouro SET cep = @cep, logradouro = @logradouro, bairro = @bairro, cidade = @cidade, uf = @uf, pais = @pais WHERE id = @id;";
            var linhasAfetadas = comando.ExecuteNonQuery(); // Executar o comando no banco de dados
        }

        // método para deletar os dados aqui
        public void Delete(Logradouro dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão

            // Adiciona parâmetro (@campo e valor)
            var id = comando.CreateParameter();
            id.ParameterName = "@id";
            id.Value = dado.Id;

            comando.Parameters.Add(id);
            conexao.Open();

            // Realiza o DELETE
            comando.CommandText = @"DELETE FROM logradouro WHERE id = @id;";
            var linhasAfetadas = comando.ExecuteNonQuery(); // Executar o comando no banco de dados
        }

        public Logradouro GetOne(Logradouro dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetro (@campo e valor)
            var cep = comando.CreateParameter();
            cep.ParameterName = "@cep";
            cep.Value = dado.Cep.Trim();
            comando.Parameters.Add(cep);
            conexao.Open();
            comando.CommandText = @"SELECT id, cep, logradouro, bairro, cidade, uf, pais FROM logradouro WHERE TRIM(cep) = @cep;";
            using var reader = comando.ExecuteReader();

            // carrega os dados para ser retornado e utilizado no databinding
            Logradouro dadosRetorno = new Logradouro();
            while (reader.Read())
            {
                dadosRetorno.Id = reader.GetInt32(0);
                dadosRetorno.Cep = reader.GetString(1);
                dadosRetorno.Nome = reader.GetString(2);
                dadosRetorno.Bairro = reader.GetString(3);
                dadosRetorno.Cidade = reader.GetString(4);
                dadosRetorno.Uf = reader.GetString(5);
                dadosRetorno.Pais = reader.GetString(6);

            }
            return dadosRetorno;
        }
    }
}
