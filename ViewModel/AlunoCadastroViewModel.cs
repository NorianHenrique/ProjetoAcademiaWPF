using ProjetoAcademiaWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace ProjetoAcademiaWPF.ViewModel
{
    public class AlunoCadastroViewModel : LogradouroViewModel
    {
        private Aluno _aluno;
        public int Id { get { return _aluno.Id; } set { _aluno.Id = value; OnPropertyChanged("Id"); } }
        public string Cpf { get { return _aluno.Cpf; } set { _aluno.Cpf = value; OnPropertyChanged("Cpf"); } }
        public string Telefone { get { return _aluno.Telefone; } set { _aluno.Telefone = value; OnPropertyChanged("Telefone"); } }
        public string Nome { get { return _aluno.Nome; } set { _aluno.Nome = value; OnPropertyChanged("Nome"); } }
        public DateTime Nascimento { get { return _aluno.Nascimento; } set { _aluno.Nascimento = value; OnPropertyChanged("Nascimento"); } }
        public string Email { get { return _aluno.Email; } set { _aluno.Email = value; OnPropertyChanged("Email"); } }
        public int LogradouroId { get { return _aluno.LogradouroId; } set { _aluno.LogradouroId = value; OnPropertyChanged("LogradouroId"); } }
        public string Numero { get { return _aluno.Numero; } set { _aluno.Numero = value; OnPropertyChanged("Numero"); } }
        public string Complemento { get { return _aluno.Complemento; } set { _aluno.Complemento = value; OnPropertyChanged("Complemento"); } }
        public string Senha { get { return _aluno.Senha; } set { _aluno.Senha = value; OnPropertyChanged("Senha"); } }
        public byte[] Foto { get { return _aluno.Foto; } set { _aluno.Foto = value; OnPropertyChanged("Foto"); } }
        public ICommand SalvarAlunoCommand { get; set; }
        public RelayCommand SelecionarFotoCommand { get; set; }

        public event EventHandler AlunoSalvo;
        public AlunoCadastroViewModel(Aluno aluno = null)
        {
            _aluno = aluno ?? new Aluno();
            SalvarAlunoCommand = new RelayCommand(SalvarAluno);
            SelecionarFotoCommand = new RelayCommand(SelecionarFoto);
            // selecionar o logradouro conforme o LogradouroId
            SelectedLogradouro = Logradouros.FirstOrDefault(l => l.Id == _aluno.LogradouroId);
          
        }
        private void SalvarAluno(object obj)
        {
            // Lógica para salvar
            AlunoSalvo?.Invoke(this, EventArgs.Empty);
        }
        public Aluno GetAluno()
        {
            _aluno.LogradouroId = SelectedLogradouro?.Id ?? 0; 
            return _aluno;
        }

        private void SelecionarFoto(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Foto = System.IO.File.ReadAllBytes(openFileDialog.FileName);
            }
        }

    }
    }

