using ProjetoAcademiaWPF.DataAccess;
using ProjetoAcademiaWPF.Model;
using ProjetoAcademiaWPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjetoAcademiaWPF.Pdf;

namespace ProjetoAcademiaWPF.ViewModel
{
    public class ColaboradorViewModel : ViewModelBase
    {
        // Objetos utilizados no Databinding, recurso que permite a sincronização automática entre View e ViewModel, através da propriedade DataContext da View.
        public ObservableCollection<Colaborador> Colaboradors { get; set; }
        private Colaborador _selectedColaborador;
        public Colaborador SelectedColaborador
        {
            get { return _selectedColaborador; }
            set
            {
                _selectedColaborador = value;
                OnPropertyChanged("SelectedColaborador");
                // libera somente se houver um Colaborador selecionado
                ColaboradorAtualizarCommand.RaiseCanExecuteChanged();
                ColaboradorRemoverCommand.RaiseCanExecuteChanged();
            }
        }
        // atributo para acessar o banco de dados
        private ColaboradorRepository _repository;
        // Comandos para o CRUD
        public RelayCommand ColaboradorAdicionarCommand { get; set; }
        public RelayCommand ColaboradorAtualizarCommand { get; set; }
        public RelayCommand ColaboradorRemoverCommand { get; set; }
        public RelayCommand ColaboradorValidaLoginCommand { get; set; }
        public RelayCommand GerarPdfCommand { get; set; }

        public ColaboradorViewModel()
        {
            Colaboradors = new ObservableCollection<Colaborador>();
            _repository = new ColaboradorRepository();
            // Inicializando os comandos
            ColaboradorAdicionarCommand = new RelayCommand(AdicionarColaborador);
            ColaboradorAtualizarCommand = new RelayCommand(AtualizarColaborador, CanExecuteSubmit);
            ColaboradorRemoverCommand = new RelayCommand(RemoverColaborador, CanExecuteSubmit);
            ColaboradorValidaLoginCommand = new RelayCommand(ValidaLogin);
            GerarPdfCommand = new RelayCommand(GeraPdf);
            GetAll();
        }

        public event EventHandler? LoginSucceeded;

        public event EventHandler<string> ErrorOccurred; // Evento para notificação de erro

        // Método para validar o login, chamado pelo RelayCommand
        private void ValidaLogin(object obj)
        {
                try
                {
                    var colaborador = new Colaborador
                    {
                        Cpf = "",
                        Senha = ""
                    };

                    if (obj is object[] objTela && objTela.Length >= 2)
                    {
                        colaborador.Cpf = objTela[0] as string;
                        colaborador.Senha = objTela[1] as string;
                    }
                    else
                    {
                        throw new Exception("CPF/Senha não informados.");
                    }

                    SelectedColaborador = _repository.ValidaLogin(colaborador);

                    if (SelectedColaborador != null && SelectedColaborador.Id > 0)
                    {
                        LoginSucceeded?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        throw new Exception("Login inválido.");
                    }
                }
                catch (Exception ex)
                {
                   ErrorOccurred?.Invoke(this, ex.Message);
            }
            }
        

        // Implementação da interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private bool CanExecuteSubmit(object parameter)
        {
            // validação utilizada para liberar ou não os botões de atualizar e remover na view
            return SelectedColaborador != null;
        }

        private void GeraPdf(object obj)
        {
            ClassGeraPDF.ColaboradoresPdf(Colaboradors);
        }

        public void GetAll()
        {
            // busca no banco de dados e carrega em Colaboradors, limpando antes
            Colaboradors.Clear();
            _repository.GetAll().ForEach(data => Colaboradors.Add(data));
        }

        // métodos com as operações de CRUD aqui

        private void AdicionarColaborador(object obj)
        {
            WindowCadastrarColaborador janelaCadastro = new WindowCadastrarColaborador();
            var viewModel = (ColaboradorCadastroViewModel)janelaCadastro.DataContext;
            viewModel.ColaboradorSalvo += (sender, e) =>
            {
                try
                {
                    var novoColaborador = viewModel.GetColaborador();
                    _repository.Add(novoColaborador);
                    janelaCadastro.Close();
                    MessageBox.Show("Sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            };
            janelaCadastro.ShowDialog();
        }

        private void AtualizarColaborador(object obj)
        {
            WindowCadastrarColaborador janelaCadastro = new WindowCadastrarColaborador();
            var viewModel = new ColaboradorCadastroViewModel(SelectedColaborador);
            janelaCadastro.DataContext = viewModel;
            viewModel.ColaboradorSalvo += (sender, e) =>
            {
                try
                {
                    var ColaboradorEditado = viewModel.GetColaborador();
                    _repository.Update(ColaboradorEditado);
                    janelaCadastro.Close();
                    MessageBox.Show("Sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            };
            janelaCadastro.ShowDialog();
        }

        private void RemoverColaborador(object obj)
        {
            if (SelectedColaborador == null) return;
            if (MessageBox.Show("Confirma?", "Colaborador", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedColaborador);
                    MessageBox.Show("Sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            }
        }
    }
}
