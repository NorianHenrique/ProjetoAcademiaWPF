using ProjetoAcademiaWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetoAcademiaWPF.ViewModel
{
    public class ColaboradorCadastroViewModel : LogradouroViewModel 
    {
        private Colaborador _colaborador;
        public int Id { get { return _colaborador.Id; } set { _colaborador.Id = value; OnPropertyChanged("Id"); } }
        public string Cpf { get { return _colaborador.Cpf; } set { _colaborador.Cpf = value; OnPropertyChanged("Cpf"); } }
        public string Telefone { get { return _colaborador.Telefone; } set { _colaborador.Telefone = value; OnPropertyChanged("Telefone"); } }
        public string Nome { get { return _colaborador.Nome; } set { _colaborador.Nome = value; OnPropertyChanged("Nome"); } }
        public DateTime Nascimento { get { return _colaborador.Nascimento; } set { _colaborador.Nascimento = value; OnPropertyChanged("Nascimento"); } }
        public string Email { get { return _colaborador.Email; } set { _colaborador.Email = value; OnPropertyChanged("Email"); } }
        public int LogradouroId { get { return _colaborador.LogradouroId; } set { _colaborador.LogradouroId = value; OnPropertyChanged("LogradouroId"); } }
        public string Numero { get { return _colaborador.Numero; } set { _colaborador.Numero = value; OnPropertyChanged("Numero"); } }
        public string Complemento { get { return _colaborador.Complemento; } set { _colaborador.Complemento = value; OnPropertyChanged("Complemento"); } }
        public string Senha { get { return _colaborador.Senha; } set { _colaborador.Senha = value; OnPropertyChanged("Senha"); } }
        public DateTime Admissao { get { return _colaborador.Admissao; } set { _colaborador.Admissao = value; OnPropertyChanged("Admissao"); } }
        public EnumColaboradorTipo Tipo { get { return _colaborador.Tipo; } set { _colaborador.Tipo = value; OnPropertyChanged("Tipo"); } }
        public EnumColaboradorVinculo Vinculo { get { return _colaborador.Vinculo; } set { _colaborador.Vinculo = value; OnPropertyChanged("Vinculo"); } }
        public ICommand SalvarColaboradorCommand { get; set; }

        public event EventHandler ColaboradorSalvo;
        public ColaboradorCadastroViewModel(Colaborador colaborador = null)
        {
            _colaborador = colaborador ?? new Colaborador();
            SalvarColaboradorCommand = new RelayCommand(SalvarColaborador);
            // selecionar o logradouro conforme o LogradouroId
            SelectedLogradouro = Logradouros.FirstOrDefault(l => l.Id == _colaborador.LogradouroId);
        }

      
        private void SalvarColaborador(object obj)
        {
            //_colaborador.Senha = ClassFuncoes.Sha256Hash(Senha);

            // Lógica para salvar
            ColaboradorSalvo?.Invoke(this, EventArgs.Empty);
        }
        public Colaborador GetColaborador()
        {
            _colaborador.LogradouroId = SelectedLogradouro.Id;
            return _colaborador;
        }
    }
    /*
    Aqui, ao inves de herdar de ViewModelBase, a classe ColaboradorCadastroViewModel herda de LogradouroViewModel, que por sua vez herda de ViewModelBase.
    Isso é feito para que a classe ColaboradorCadastroViewModel tenha acesso a todos os métodos e propriedades de LogradouroViewModel.
    Algo necessário para que tenhamos como filtrar os logradouros e selecionar um logradouro para o Colaborador.
    */
}

