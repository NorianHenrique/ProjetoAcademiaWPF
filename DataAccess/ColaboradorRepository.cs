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
    public class ColaboradorRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }
        public ColaboradorRepository()
        {
            // buscar os dados de connectionstring e provider do arquivo de configuração
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
            // define a factory, ou seja, o provedor de dados - Mysql, SqlServer, etc
            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        // implementa os métodos de CRUD, utilizando DBProviderFactory
        // método para carregar os dados aqui
        public List<Colaborador> GetAll()
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
            conexao.Open();
            comando.CommandText = @"SELECT id, cpf, telefone, nome, nascimento, email, logradouro_id, numero,
            complemento, senha, admissao, tipo, vinculo FROM tb_colaborador;";
            using var reader = comando.ExecuteReader();
            // carrega os dados para ser retornado e utilizado no databinding
            List<Colaborador> dadosRetorno = new List<Colaborador>();
            while (reader.Read())
            {
                var colaborador = new Colaborador
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
                    Admissao = reader.GetDateTime(10)
                };

                // Converta tipos com segurança
                var tipoChar = reader.GetString(11);
                if (!string.IsNullOrEmpty(tipoChar))
                {
                    colaborador.Tipo = (EnumColaboradorTipo)Convert.ToChar(tipoChar);
                }

                var vinculoChar = reader.GetString(12);
                if (!string.IsNullOrEmpty(vinculoChar))
                {
                    colaborador.Vinculo = (EnumColaboradorVinculo)Convert.ToChar(vinculoChar);
                }

                dadosRetorno.Add(colaborador);
            }
            return dadosRetorno;
        }

        // método para inserir os dados aqui
        public void Add(Colaborador dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão

            var cpf = comando.CreateParameter(); cpf.ParameterName = "@cpf"; cpf.Value = dado.Cpf; comando.Parameters.Add(cpf);
            var telefone = comando.CreateParameter(); telefone.ParameterName = "@telefone"; telefone.Value = dado.Telefone; comando.Parameters.Add(telefone);
            var nome = comando.CreateParameter(); nome.ParameterName = "@nome"; nome.Value = dado.Nome; comando.Parameters.Add(nome);
            var nascimento = comando.CreateParameter(); nascimento.ParameterName = "@nascimento"; nascimento.Value = dado.Nascimento; comando.Parameters.Add(nascimento);
            var email = comando.CreateParameter(); email.ParameterName = "@email"; email.Value = dado.Email; comando.Parameters.Add(email);
            var logradouro_id = comando.CreateParameter(); logradouro_id.ParameterName = "@logradouro_id"; logradouro_id.Value = dado.LogradouroId; comando.Parameters.Add(logradouro_id);
            var numero = comando.CreateParameter(); numero.ParameterName = "@numero"; numero.Value = dado.Numero; comando.Parameters.Add(numero);
            var complemento = comando.CreateParameter(); complemento.ParameterName = "@complemento"; complemento.Value = dado.Complemento; comando.Parameters.Add(complemento);
            var senha = comando.CreateParameter(); senha.ParameterName = "@senha"; senha.Value = dado.Senha; comando.Parameters.Add(senha);
            var admissao = comando.CreateParameter(); admissao.ParameterName = "@admissao"; admissao.Value = dado.Admissao; comando.Parameters.Add(admissao);

            // Aqui está a parte onde você define os valores para tipo e vinculo:
            var tipo = comando.CreateParameter();
            tipo.ParameterName = "@tipo";
            tipo.Value = Convert.ToChar(dado.Tipo); // Esta linha deve converter o enum para um valor char correspondente
            comando.Parameters.Add(tipo);

            var vinculo = comando.CreateParameter();
            vinculo.ParameterName = "@vinculo";
            vinculo.Value = Convert.ToChar(dado.Vinculo); // O mesmo para vinculo
            comando.Parameters.Add(vinculo);


            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_colaborador (cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, admissao, tipo, vinculo) 
                            VALUES (@cpf, @telefone, @nome, @nascimento, @email, @logradouro_id, @numero, @complemento, @senha, @admissao, @tipo, @vinculo);";

            // Logar os dados do comando (Aberto para Debugging)
            foreach (DbParameter param in comando.Parameters)
            {
                Console.WriteLine($"{param.ParameterName}: {param.Value}");
            }

            var linhas = comando.ExecuteNonQuery(); // Execute the command
        }


        // método para atualizar os dados aqui
        public void Update(Colaborador dado)
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

            var cpf = comando.CreateParameter();
            cpf.ParameterName = "@cpf";
            cpf.Value = dado.Cpf;
            comando.Parameters.Add(cpf);

            var telefone = comando.CreateParameter();
            telefone.ParameterName = "@telefone";
            telefone.Value = dado.Telefone;
            comando.Parameters.Add(telefone);

            var nome = comando.CreateParameter();
            nome.ParameterName = "@nome";
            nome.Value = dado.Nome;
            comando.Parameters.Add(nome);

            var nascimento = comando.CreateParameter();
            nascimento.ParameterName = "@nascimento";
            nascimento.Value = dado.Nascimento;
            comando.Parameters.Add(nascimento);

            var email = comando.CreateParameter();
            email.ParameterName = "@email";
            email.Value = dado.Email;
            comando.Parameters.Add(email);

            var logradouro_id = comando.CreateParameter();
            logradouro_id.ParameterName = "@logradouro_id";
            logradouro_id.Value = dado.LogradouroId;
            comando.Parameters.Add(logradouro_id);

            var numero = comando.CreateParameter();
            numero.ParameterName = "@numero";
            numero.Value = dado.Numero;
            comando.Parameters.Add(numero);

            var complemento = comando.CreateParameter();
            complemento.ParameterName = "@complemento";
            complemento.Value = dado.Complemento;
            comando.Parameters.Add(complemento);

            var senha = comando.CreateParameter();
            senha.ParameterName = "@senha";
            senha.Value = dado.Senha;
            comando.Parameters.Add(senha);

            var admissao = comando.CreateParameter();
            admissao.ParameterName = "@admissao";
            admissao.Value = dado.Admissao;
            comando.Parameters.Add(admissao);

            // Declare e inicialize o parâmetro 'tipo' corretamente
            var tipo = comando.CreateParameter();
            tipo.ParameterName = "@tipo";
            tipo.Value = Convert.ToChar(dado.Tipo); // Verifique se 'dado.Tipo' está do tipo adequado
            comando.Parameters.Add(tipo);

            // Declare e inicialize o parâmetro 'vinculo' corretamente
            var vinculo = comando.CreateParameter();
            vinculo.ParameterName = "@vinculo";
            vinculo.Value = Convert.ToChar(dado.Vinculo); // Verifique se 'dado.Vinculo' está do tipo adequado
            comando.Parameters.Add(vinculo);

            conexao.Open();

            //realiza o UPDATE
            comando.CommandText = @"UPDATE tb_colaborador SET cpf = @cpf, telefone = @telefone, nome = @nome, nascimento = @nascimento, email = @email, logradouro_id = @logradouro_id,
            numero = @numero, complemento = @complemento, senha = @senha, admissao = @admissao, tipo = @tipo, vinculo = @vinculo WHERE id = @id;";

            // Executa o comando no banco de dados
            _ = comando.ExecuteNonQuery();
        }

        // método para deletar os dados aqui
        public void Delete(Colaborador dado)
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
            comando.CommandText = @"DELETE FROM tb_colaborador WHERE id = @id;";
            //executa o comando no banco de dados
            _ = comando.ExecuteNonQuery();
        }


        public Colaborador ValidaLogin(Colaborador dado)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = ConnectionString; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetro (@campo e valor) var cpf = comando.CreateParameter();
            var cpf = comando.CreateParameter();                            //Adiciona parâmetro (@campo e valor) var cpf = comando.CreateParameter();
            cpf.ParameterName = "@cpf";
            cpf.Value = dado.Cpf;
            comando.Parameters.Add(cpf);

            var senha = comando.CreateParameter();
            senha.ParameterName = "@senha";
            senha.Value = dado.Senha;
            comando.Parameters.Add(senha);

            conexao.Open();
            comando.CommandText = @"SELECT id, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, admissao, tipo, vinculo FROM tb_colaborador WHERE cpf = @cpf AND senha = @senha;";
            using var reader = comando.ExecuteReader();
            // carrega os dados para ser retornado e utilizado no databinding
            Colaborador dadosRetorno = new Colaborador();
            while (reader.Read())
            {
                dadosRetorno.Id = reader.GetInt32(0);
                dadosRetorno.Cpf = reader.GetString(1);
                dadosRetorno.Telefone = reader.GetString(2);
                dadosRetorno.Nome = reader.GetString(3);
                dadosRetorno.Nascimento = reader.GetDateTime(4);
                dadosRetorno.Email = reader.GetString(5);
                dadosRetorno.LogradouroId = reader.GetInt32(6);
                dadosRetorno.Numero = reader.GetString(7);
                dadosRetorno.Complemento = reader.GetString(8);
                dadosRetorno.Senha = reader.GetString(9);
                dadosRetorno.Admissao = reader.GetDateTime(10);
                dadosRetorno.Tipo = (EnumColaboradorTipo)reader.GetString(11)[0];
                dadosRetorno.Vinculo = (EnumColaboradorVinculo)reader.GetString(12)[0] ;
            }
            return dadosRetorno;
        }

        public Colaborador GetByCpf(string cpf)
        {
            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando.Connection = conexao; // Atribui conexão

            comando.CommandText = @"SELECT id, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, admissao, tipo, vinculo
                            FROM tb_colaborador WHERE cpf = @cpf;";

            // Cria o parâmetro para o CPF
            var parametroCpf = comando.CreateParameter();
            parametroCpf.ParameterName = "@cpf";
            parametroCpf.Value = cpf;
            comando.Parameters.Add(parametroCpf);

            conexao.Open();
            using var reader = comando.ExecuteReader();

            // Verifica se há resultados
            if (reader.Read())
            {
                return new Colaborador
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
                    Admissao = reader.GetDateTime(10),
                    Tipo = (EnumColaboradorTipo)reader.GetString(11)[0],
                    Vinculo = (EnumColaboradorVinculo)reader.GetString(12)[0]
                };

                //return colaborador; // Retorna o colaborador
            }

            return null; // Retorna null se nenhum colaborador for encontrado
        }
    }
}

