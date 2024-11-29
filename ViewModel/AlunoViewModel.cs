using ProjetoAcademiaWPF.DataAccess;
using ProjetoAcademiaWPF.Model;
using ProjetoAcademiaWPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjetoAcademiaWPF.Pdf;

namespace ProjetoAcademiaWPF.ViewModel
{
    public class AlunoViewModel : ViewModelBase
    {
        // Objetos utilizados no Databinding, recurso que permite a sincronização automática entre View e ViewModel, através da propriedade DataContext da View.
        public ObservableCollection<Aluno> Alunos { get; set; }
        private Aluno _selectedAluno;
        public Aluno SelectedAluno
        {
            get { return _selectedAluno; }
            set
            {
                _selectedAluno = value;
                OnPropertyChanged("SelectedAluno");
                // libera somente se houver um Aluno selecionado
                AlunoAtualizarCommand.RaiseCanExecuteChanged();
                AlunoRemoverCommand.RaiseCanExecuteChanged();
            }
        }

        // atributo para acessar o banco de dados
        private AlunoRepository _repository;
        // Comandos para o CRUD
        public RelayCommand AlunoAdicionarCommand { get; set; }
        public RelayCommand AlunoAtualizarCommand { get; set; }
        public RelayCommand AlunoRemoverCommand { get; set; }
        public RelayCommand GerarPdfCommand { get; set; }


        public AlunoViewModel()
        {
            Alunos = new ObservableCollection<Aluno>();
            _repository = new AlunoRepository();
            // Inicializando os comandos
            AlunoAdicionarCommand = new RelayCommand(AdicionarAluno);
            AlunoAtualizarCommand = new RelayCommand(AtualizarAluno, CanExecuteSubmit);
            AlunoRemoverCommand = new RelayCommand(RemoverAluno, CanExecuteSubmit);
            GerarPdfCommand = new RelayCommand(GeraPdf);
            GetAll();
        }
        private bool CanExecuteSubmit(object parameter)
        {
            // validação utilizada para liberar ou não os botões de atualizar e remover na view
            return SelectedAluno != null;
        }

        public void GetAll()
        {
            // busca no banco de dados e carrega em Alunos, limpando antes
            Alunos.Clear();
            _repository.GetAll().ForEach(data => Alunos.Add(data));
        }

        private void GeraPdf(object obj)
        {
            ClassGeraPDF.AlunosPdf(Alunos);
        }

        // métodos com as operações de CRUD aqui

        private void AdicionarAluno(object obj)
        {
            WindowCadastrarAluno janelaCadastro = new WindowCadastrarAluno();
            var viewModel = (AlunoCadastroViewModel)janelaCadastro.DataContext;
            viewModel.AlunoSalvo += (sender, e) =>
            {
                try
                {
                    var novoAluno = viewModel.GetAluno();
                    _repository.Add(novoAluno);
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

        // abrir a tela e editar selecionado
        private void AtualizarAluno(object obj)
        {
            WindowCadastrarAluno janelaCadastro = new WindowCadastrarAluno();
            var viewModel = new AlunoCadastroViewModel(SelectedAluno);
            janelaCadastro.DataContext = viewModel;
            viewModel.AlunoSalvo += (sender, e) =>
            {
                try
                {
                    var AlunoEditado = viewModel.GetAluno();
                    _repository.Update(AlunoEditado);
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

        // remover o selecionado
        private void RemoverAluno(object obj)
        {
            if (SelectedAluno == null) return;
            if (MessageBox.Show("Confirma?", "Aluno", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedAluno);
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
