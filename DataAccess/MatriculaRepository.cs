using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using ProjetoAcademiaWPF.Model;

namespace ProjetoAcademiaWPF.DataAccess
{
    public class MatriculaRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public MatriculaRepository()
        {
            // buscar os dados de connectionstring e provider do arquivo de configuração
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
            // define a factory, ou seja, o provedor de dados - Mysql, SqlServer, etc
            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        // método para carregar todas as matrículas
        public List<Matricula> GetAll()
        {
            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao!.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando.Connection = conexao; // Atribui conexão
            conexao.Open();
            comando.CommandText = @"SELECT id, id_aluno, id_atendente, plano, dataInicio, dataFim, restricaoMedica, obsRestricao, laudoMedico, objetivo FROM tb_matriculas;";
            using var reader = comando.ExecuteReader();
            // carrega os dados para ser retornado e utilizado no databinding
            List<Matricula> dadosRetorno = new List<Matricula>();
            while (reader.Read())
            {
                var matricula = new Matricula
                {
                    Id = reader.GetInt32(0),
                    IdAluno = reader.GetInt32(1),
                    IdAtendente = reader.GetInt32(2),
                    DataInicio = reader.GetDateTime(4),
                    DataFim = reader.GetDateTime(5),
                    ObsRestricao = reader.GetString(7),
                    LaudoMedico = reader.IsDBNull(8) ? null : (byte[])reader[8],
                    Objetivo = reader.GetString(9)
                };

                // Converta tipos com segurança
                var planoChar = reader.GetString(3);
                if (!string.IsNullOrEmpty(planoChar))
                {
                    matricula.Plano = (EnumMatriculaPlano)Convert.ToChar(planoChar);
                }

                var restricaoMedicaChar = reader.GetString(6);
                if (!string.IsNullOrEmpty(restricaoMedicaChar))
                {
                    matricula.RestricaoMedica = (EnumMatriculaRestricaoMedica)Convert.ToChar(restricaoMedicaChar);
                }

                dadosRetorno.Add(matricula);
            }
            return dadosRetorno;
        }

        // método para inserir os dados
        public void Add(Matricula dado)
        {
            // Validações
            ValidateMatricula(dado);

            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao!.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando.Connection = conexao; // Atribui conexão

            // Adiciona parâmetro (@campo e valor)
            var id_aluno = comando.CreateParameter();
            id_aluno.ParameterName = "@id_aluno";
            id_aluno.Value = dado.IdAluno;
            comando.Parameters.Add(id_aluno);

            var id_atendente = comando.CreateParameter();
            id_atendente.ParameterName = "@id_atendente";
            id_atendente.Value = dado.IdAtendente;
            comando.Parameters.Add(id_atendente);

            var plano = comando.CreateParameter();
            plano.ParameterName = "@plano";
            plano.Value = Convert.ToChar(dado.Plano); // Convertendo Enum para char
            comando.Parameters.Add(plano);

            var dataInicio = comando.CreateParameter();
            dataInicio.ParameterName = "@dataInicio";
            dataInicio.Value = dado.DataInicio;
            comando.Parameters.Add(dataInicio);

            var dataFim = comando.CreateParameter();
            dataFim.ParameterName = "@dataFim";
            dataFim.Value = dado.DataFim;
            comando.Parameters.Add(dataFim);

            var restricaoMedica = comando.CreateParameter();
            restricaoMedica.ParameterName = "@restricaoMedica";
            restricaoMedica.Value = Convert.ToChar(dado.RestricaoMedica); // Convertendo Enum para char
            comando.Parameters.Add(restricaoMedica);

            var obsRestricao = comando.CreateParameter();
            obsRestricao.ParameterName = "@obsRestricao";
            obsRestricao.Value = dado.ObsRestricao;
            comando.Parameters.Add(obsRestricao);

            var laudoMedico = comando.CreateParameter();
            laudoMedico.ParameterName = "@laudoMedico";
            laudoMedico.Value = dado.LaudoMedico;
            comando.Parameters.Add(laudoMedico);

            var objetivo = comando.CreateParameter();
            objetivo.ParameterName = "@objetivo";
            objetivo.Value = dado.Objetivo;
            comando.Parameters.Add(objetivo);

            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_matriculas (id_aluno, id_atendente, plano, dataInicio, dataFim, restricaoMedica, obsRestricao, laudoMedico, objetivo) 
                            VALUES (@id_aluno, @id_atendente, @plano, @dataInicio, @dataFim, @restricaoMedica, @obsRestricao, @laudoMedico, @objetivo);";

            // Logar os dados do comando (para Debugging)
            foreach (DbParameter param in comando.Parameters)
            {
                Console.WriteLine($"{param.ParameterName}: {param.Value}");
            }

            var linhas = comando.ExecuteNonQuery(); // Executa o co
        }

        // método para atualizar os dados
        public void Update(Matricula dado)
        {
            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao!.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando.Connection = conexao; // Atribui conexão

            // Adiciona parâmetro (@campo e valor)
            var id = comando.CreateParameter();
            id.ParameterName = "@id";
            id.Value = dado.Id;
            comando.Parameters.Add(id);

            var id_aluno = comando.CreateParameter();
            id_aluno.ParameterName = "@id_aluno";
            id_aluno.Value = dado.IdAluno;
            comando.Parameters.Add(id_aluno);

            var id_atendente = comando.CreateParameter();
            id_atendente.ParameterName = "@id_atendente";
            id_atendente.Value = dado.IdAtendente;
            comando.Parameters.Add(id_atendente);

            var plano = comando.CreateParameter();
            plano.ParameterName = "@plano";
            plano.Value = Convert.ToChar(dado.Plano);
            comando.Parameters.Add(plano);

            var dataInicio = comando.CreateParameter();
            dataInicio.ParameterName = "@dataInicio";
            dataInicio.Value = dado.DataInicio;
            comando.Parameters.Add(dataInicio);

            var dataFim = comando.CreateParameter();
            dataFim.ParameterName = "@dataFim";
            dataFim.Value = dado.DataFim;
            comando.Parameters.Add(dataFim);

            var restricaoMedica = comando.CreateParameter();
            restricaoMedica.ParameterName = "@restricaoMedica";
            restricaoMedica.Value = Convert.ToChar(dado.RestricaoMedica); // Convertendo Enum para char
            comando.Parameters.Add(restricaoMedica);

            var obsRestricao = comando.CreateParameter();
            obsRestricao.ParameterName = "@obsRestricao";
            obsRestricao.Value = dado.ObsRestricao;
            comando.Parameters.Add(obsRestricao);

            var laudoMedico = comando.CreateParameter();
            laudoMedico.ParameterName = "@laudoMedico";
            laudoMedico.Value = dado.LaudoMedico;
            comando.Parameters.Add(laudoMedico);

            var objetivo = comando.CreateParameter();
            objetivo.ParameterName = "@objetivo";
            objetivo.Value = dado.Objetivo;
            comando.Parameters.Add(objetivo);

            conexao.Open();

            // Realiza o UPDATE
            comando.CommandText = @"UPDATE tb_matriculas SET id_aluno = @id_aluno, id_atendente = @id_atendente, plano = @plano, dataInicio = @dataInicio, 
                                    dataFim = @dataFim, restricaoMedica = @restricaoMedica, obsRestricao = @obsRestricao, laudoMedico = @laudoMedico, 
                                    objetivo = @objetivo WHERE id = @id;";
            // Executa o comando no banco de dados
            _ = comando.ExecuteNonQuery();
        }

        // método para deletar os dados
        public void Delete(Matricula dado)
        {
            using var conexao = factory.CreateConnection(); // Cria conexão
            conexao!.ConnectionString = ConnectionString; // Atribui a string de conexão
            using var comando = factory.CreateCommand(); // Cria comando
            comando.Connection = conexao; // Atribui conexão

            // Adiciona parâmetro (@campo e valor)
            var id = comando.CreateParameter();
            id.ParameterName = "@id";
            id.Value = dado.Id;
            comando.Parameters.Add(id);

            conexao.Open();
            // Realiza o DELETE
            comando.CommandText = @"DELETE FROM tb_matriculas WHERE id = @id;";
            // Executa o comando no banco de dados
            _ = comando.ExecuteNonQuery();
        }

        private void ValidateMatricula(Matricula dado)
        {
            if (dado == null)
            {
                throw new ArgumentNullException(nameof(dado), "Os dados da matrícula não podem ser nulos.");
            }

            // Verifica se a Data de Início é menor que a Data de Fim
            if (dado.DataInicio >= dado.DataFim)
            {
                throw new ArgumentException("A data de início deve ser anterior à data de fim.");
            }

            // Valida o plano
            if (!Enum.IsDefined(typeof(EnumMatriculaPlano), dado.Plano))
            {
                throw new ArgumentException("Valor inválido para o plano.");
            }

            // Valida a restrição médica
            if (!Enum.IsDefined(typeof(EnumMatriculaRestricaoMedica), dado.RestricaoMedica))
            {
                throw new ArgumentException("Valor inválido para a restrição médica.");
            }

            // Adicione outros testes de validação que você precisar para observações aqui
            if (!string.IsNullOrWhiteSpace(dado.ObsRestricao) && dado.RestricaoMedica == EnumMatriculaRestricaoMedica.Nenhuma)
            {
                throw new ArgumentException("Observação não pode ser registrada sem uma restrição médica.");
            }
        }

    }
}
