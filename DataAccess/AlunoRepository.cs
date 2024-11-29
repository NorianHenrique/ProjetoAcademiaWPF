using ProjetoAcademiaWPF.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcademiaWPF.DataAccess
{
    public class AlunoRepository
    {
            private readonly DbProviderFactory factory;
            private string ConnectionString { get; set; }
            private string ProviderName { get; set; }
            public AlunoRepository()
            {
                // buscar os dados de connectionstring e provider do arquivo de configuração
                ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
                ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
                // define a factory, ou seja, o provedor de dados - Mysql, SqlServer, etc
                factory = DbProviderFactories.GetFactory(ProviderName);
            }

        // implementa os métodos de CRUD, utilizando DBProviderFactory

        // método para carregar os dados aqui
        public List<Aluno> GetAll()
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
            conexao.Open();
            comando.CommandText = @"SELECT id, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, foto FROM tb_aluno;";
            using var reader = comando.ExecuteReader();
            // carrega os dados para ser retornado e utilizado no databinding
            List<Aluno> dadosRetorno = new List<Aluno>();
            while (reader.Read())
            {
                dadosRetorno.Add(new Aluno
                {
                    Id = reader.GetInt32(0),
                    Cpf = reader.GetString(1),
                    Telefone = reader.GetString(2),
                    Nome = reader.GetString(3),
                    Nascimento = reader.GetDateTime(4),
                    Email = reader.GetString(5),
                    LogradouroId = reader.GetInt32(6),
                    Numero = reader.GetString(7),
                    Complemento = reader.GetString(8),
                    Senha = reader.GetString(9),
                    Foto = reader.IsDBNull(10) ? null : (byte[])reader[10]
                });
            }
            return dadosRetorno;
        }

        // método para inserir os dados aqui
        public void Add(Aluno dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetro (@campo e valor)
            var cpf = comando.CreateParameter(); cpf.ParameterName = "@cpf"; cpf.Value = dado.Cpf; comando.Parameters.Add(cpf);
            var telefone = comando.CreateParameter(); telefone.ParameterName = "@telefone"; telefone.Value = dado.Telefone; comando.Parameters.Add(telefone);
            var nome = comando.CreateParameter(); nome.ParameterName = "@nome"; nome.Value = dado.Nome; comando.Parameters.Add(nome);
            var nascimento = comando.CreateParameter(); nascimento.ParameterName = "@nascimento"; nascimento.Value = dado.Nascimento; comando.Parameters.Add(nascimento);
            var email = comando.CreateParameter(); email.ParameterName = "@email"; email.Value = dado.Email; comando.Parameters.Add(email);
            var logradouro_id = comando.CreateParameter(); logradouro_id.ParameterName = "@logradouro_id"; logradouro_id.Value = dado.LogradouroId; comando.Parameters.Add(logradouro_id);
            var numero = comando.CreateParameter(); numero.ParameterName = "@numero"; numero.Value = dado.Numero; comando.Parameters.Add(numero);
            var complemento = comando.CreateParameter(); complemento.ParameterName = "@complemento"; complemento.Value = dado.Complemento; comando.Parameters.Add(complemento);
            var senha = comando.CreateParameter(); senha.ParameterName = "@senha"; senha.Value = ""; comando.Parameters.Add(senha);
            var foto = comando.CreateParameter(); foto.ParameterName = "@foto"; foto.Value = dado.Foto; comando.Parameters.Add(foto);

            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_aluno (cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, foto) VALUES (@cpf, @telefone, @nome,
            @nascimento, @email, @logradouro_id, @numero, @complemento, @senha, @foto);";
            //Executa o script na conexão e armazena o número de linhas afetadas.
            var linhas = comando.ExecuteNonQuery();
        }

        // método para atualizar os dados aqui
        public void Update(Aluno dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetro (@campo e valor)
            var id = comando.CreateParameter(); id.ParameterName = "@id"; id.Value = dado.Id; comando.Parameters.Add(id);
            var cpf = comando.CreateParameter(); cpf.ParameterName = "@cpf"; cpf.Value = dado.Cpf; comando.Parameters.Add(cpf);
            var telefone = comando.CreateParameter(); telefone.ParameterName = "@telefone"; telefone.Value = dado.Telefone; comando.Parameters.Add(telefone);
            var nome = comando.CreateParameter(); nome.ParameterName = "@nome"; nome.Value = dado.Nome; comando.Parameters.Add(nome);
            var nascimento = comando.CreateParameter(); nascimento.ParameterName = "@nascimento"; nascimento.Value = dado.Nascimento; comando.Parameters.Add(nascimento);
            var email = comando.CreateParameter(); email.ParameterName = "@email"; email.Value = dado.Email; comando.Parameters.Add(email);
            var logradouro_id = comando.CreateParameter(); logradouro_id.ParameterName = "@logradouro_id"; logradouro_id.Value = dado.LogradouroId; comando.Parameters.Add(logradouro_id);
            var numero = comando.CreateParameter(); numero.ParameterName = "@numero"; numero.Value = dado.Numero; comando.Parameters.Add(numero);
            var complemento = comando.CreateParameter(); complemento.ParameterName = "@complemento"; complemento.Value = dado.Complemento; comando.Parameters.Add(complemento);
            var senha = comando.CreateParameter(); senha.ParameterName = "@senha"; senha.Value = dado.Senha; comando.Parameters.Add(senha);
            var foto = comando.CreateParameter(); foto.ParameterName = "@foto"; foto.Value = dado.Foto; comando.Parameters.Add(foto);

            conexao.Open();
            //realiza o UPDATE
            comando.CommandText = @"UPDATE tb_aluno SET cpf = @cpf, telefone = @telefone, nome = @nome, nascimento = @nascimento, email = @email, logradouro_id = @logradouro_id,
            numero = @numero, complemento = @complemento, senha = @senha, foto = @foto WHERE id= @id;";
            _ = comando.ExecuteNonQuery();
        }

        // método para deletar os dados aqui
        public void Delete(Aluno dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetro (@campo e valor)
            var id = comando.CreateParameter();
            id.ParameterName = "@id";
            id.Value = dado.Id;
            comando.Parameters.Add(id);
            conexao.Open();
            //realiza o DELETE
            comando.CommandText = @"DELETE FROM tb_aluno WHERE id = @id;";
            _ = comando.ExecuteNonQuery();
        }

        public Aluno GetById(int id)
        {
            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao!.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando!.Connection = conexao; // Atribui conexão

            comando.CommandText = @"SELECT id, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha 
                        FROM tb_aluno WHERE id = @id;";

            var parametroId = comando.CreateParameter();
            parametroId.ParameterName = "@id";
            parametroId.Value = id;
            comando.Parameters.Add(parametroId);

            conexao.Open();
            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Aluno
                {
                    Id = reader.GetInt32(0),
                    Cpf = reader.GetString(1),
                    Telefone = reader.GetString(2),
                    Nome = reader.GetString(3),
                    Nascimento = reader.GetDateTime(4),
                    Email = reader.GetString(5),
                    LogradouroId = reader.GetInt32(6),
                    Numero = reader.GetString(7),
                    Complemento = reader.GetString(8),
                    Senha = reader.GetString(9)
                };
            }

            return null; // Retorna null se nenhum aluno for encontrado
        }

        public Aluno GetByCpf(string cpf)
        {
            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao!.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando!.Connection = conexao; // Atribui conexão

            comando.CommandText = @"SELECT id, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha 
                            FROM tb_aluno WHERE cpf = @cpf;";

            var parametroCpf = comando.CreateParameter();
            parametroCpf.ParameterName = "@cpf";
            parametroCpf.Value = cpf;
            comando.Parameters.Add(parametroCpf);

            conexao.Open();
            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Aluno
                {
                    Id = reader.GetInt32(0),
                    Cpf = reader.GetString(1),
                    Telefone = reader.GetString(2),
                    Nome = reader.GetString(3),
                    Nascimento = reader.GetDateTime(4),
                    Email = reader.GetString(5),
                    LogradouroId = reader.GetInt32(6),
                    Numero = reader.GetString(7),
                    Complemento = reader.GetString(8),
                    Senha = reader.GetString(9)
                };
            }

            return null; // Retorna null se nenhum aluno for encontrado
        }
    }
    }
