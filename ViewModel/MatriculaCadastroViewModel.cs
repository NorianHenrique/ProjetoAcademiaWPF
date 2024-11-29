using ProjetoAcademiaWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using ProjetoAcademiaWPF.DataAccess;
using System.Windows;

namespace ProjetoAcademiaWPF.ViewModel
{
    public class MatriculaCadastroViewModel : ViewModelBase
    {
        private Matricula _matricula;

        private AlunoRepository _alunoRepository;

        private ColaboradorRepository _colaboradorRepository;
        public int Id { get { return _matricula.Id; } set { _matricula.Id = value; OnPropertyChanged("Id"); } }
        public int IdAluno { get { return _matricula.IdAluno; } set { _matricula.IdAluno = value; OnPropertyChanged("IdAluno"); } }
        public int IdAtendente { get { return _matricula.IdAtendente; } set { _matricula.IdAtendente = value; OnPropertyChanged("IdAtendente"); } }
        public EnumMatriculaPlano Plano { get { return _matricula.Plano; } set { _matricula.Plano = value; OnPropertyChanged("Plano"); } }
        public DateTime DataInicio { get { return _matricula.DataInicio; } set { _matricula.DataInicio = value; OnPropertyChanged("DataInicio"); } }
        public DateTime DataFim { get { return _matricula.DataFim; } set { _matricula.DataFim = value; OnPropertyChanged("DataFim"); } }
        public EnumMatriculaRestricaoMedica RestricaoMedica { get { return _matricula.RestricaoMedica; } set { _matricula.RestricaoMedica = value; OnPropertyChanged("RestricaoMedica"); } }
        public string ObsRestricao { get { return _matricula.ObsRestricao; } set { _matricula.ObsRestricao = value; OnPropertyChanged("ObsRestricao"); } }
        public byte[] LaudoMedico { get { return _matricula.LaudoMedico; } set { _matricula.LaudoMedico = value; OnPropertyChanged("LaudoMedico"); } }
        public string Objetivo { get { return _matricula.Objetivo; } set { _matricula.Objetivo = value; OnPropertyChanged("Objetivo"); } }

        private string _cpf;
        public string Cpf
        {
            get { return _cpf; }
            set
            {
                _cpf = value;
                OnPropertyChanged("Cpf");
            }
        }

        private string _cpfColaborador;
        public string CpfColaborador
        {
            get { return _cpfColaborador; }
            set
            {
                _cpfColaborador = value;
                OnPropertyChanged("CpfColaborador");
            }
        }


        public ICommand SalvarMatriculaCommand { get; set; }
        public RelayCommand SelecionarLaudoCommand { get; set; }
        public ICommand FiltrarAlunoCommand { get; set; }
        public ICommand FiltrarColaboradorCommand { get; set; }

        public event EventHandler MatriculaSalvo;

        public MatriculaCadastroViewModel(Matricula matricula = null)
        {
            _matricula = matricula ?? new Matricula();
            _alunoRepository = new AlunoRepository();
            _colaboradorRepository = new ColaboradorRepository();
            SelecionarLaudoCommand = new RelayCommand(SelecionarLaudo);
            SalvarMatriculaCommand = new RelayCommand(SalvarMatricula);
            FiltrarAlunoCommand = new RelayCommand(FiltrarAluno);
            FiltrarColaboradorCommand = new RelayCommand(FiltrarColaborador);
        }

        private void FiltrarAluno(object obj)
        {
            // Aqui, a requisição do CPF deve ser considerada

            var aluno = _alunoRepository.GetByCpf(Cpf); // Usa o repositório para buscar aluno

            if (aluno != null)
            {
                IdAluno = aluno.Id; // Se aluno encontrado, armazena o ID
                MessageBox.Show("Aluno encontrado! ID: " + IdAluno);
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.");
                IdAluno = 0; // Reseta o ID se não encontrar
            }
        }

        private void FiltrarColaborador(object obj)
        {
            
            var colaborador = _colaboradorRepository.GetByCpf(CpfColaborador);

            if (colaborador != null)
            {
                IdAtendente = colaborador.Id; // Se colaborador encontrado, armazena o ID
                MessageBox.Show("Colaborador encontrado! ID: " + IdAtendente);
            }
            else
            {
                MessageBox.Show("Colaborador não encontrado.");
                IdAtendente = 0; // Reseta o ID se não encontrar
            }
        }

        private void SalvarMatricula(object obj)
        {
            // Verifica se o aluno foi selecionado
            if (IdAluno <= 0)
            {
                MessageBox.Show("Por favor, insira o CPF do aluno.");
                return;
            }

            // Verifique a idade do aluno
            var aluno = _alunoRepository.GetById(IdAluno); // Você precisa da lógica aqui para pegar o aluno atual
            if (aluno != null)
            {
                int idade = DateTime.Now.Year - aluno.Nascimento.Year;

                // Validações de idade
                if (idade < 12)
                {
                    MessageBox.Show("O aluno deve ser maior de 12 anos.");
                    return;
                }

                if (idade < 16 && LaudoMedico == null)
                {
                    MessageBox.Show("Menores de 16 anos devem apresentar laudo médico.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.");
                return;
            }

            // Cálculo da data de fim da matrícula
            CalcularDataFim();

            // Aqui você adiciona a lógica para registrar a matrícula no banco
            MatriculaSalvo?.Invoke(this, EventArgs.Empty);
        }

        public Matricula GetMatricula()
        {
            // Verifique todos os campos obrigatórios da sua matrícula
            if (_matricula.IdAluno <= 0)  // exemplo de validação
            {
                throw new InvalidOperationException("Id do Aluno deve ser maior que zero.");
            }

            // Retorne a matrícula
            return _matricula;
        }

        public void ConfigurarDatas(DateTime dataInicio)
        {
            DataInicio = dataInicio;
            DataFim = dataInicio.AddMonths(ObterDuracaoDoPlano(Plano)); // Define DataFim com base na duração do plano
        }

        public int ObterDuracaoDoPlano(EnumMatriculaPlano plano)
        {
            switch (plano)
            {
                case EnumMatriculaPlano.Mensal:
                    return 1; // 1 mês
                case EnumMatriculaPlano.Trimestral:
                    return 3; // 3 meses
                case EnumMatriculaPlano.Semestral:
                    return 6; // 6 meses
                case EnumMatriculaPlano.Anual:
                    return 12; // 12 meses
                default:
                    throw new ArgumentException("Plano inválido");
            }
        }

        private void CalcularDataFim()
        {
            
            int duracao = ObterDuracaoDoPlano(Plano);
            DataFim = DataInicio.AddMonths(duracao);
        }

        private void SelecionarLaudo(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LaudoMedico = System.IO.File.ReadAllBytes(openFileDialog.FileName);

                // Verificar se a restrição médica exige laudo
                if (RestricaoMedica != EnumMatriculaRestricaoMedica.Nenhuma && LaudoMedico == null)
                {
                    MessageBox.Show("É necessário apresentar laudo médico para a restrição selecionada.");
                }
            }
        }
    }
}

