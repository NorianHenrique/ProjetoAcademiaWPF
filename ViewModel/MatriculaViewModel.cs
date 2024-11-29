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
    public class MatriculaViewModel : ViewModelBase
    {
        // Objetos utilizados no Databinding
        public ObservableCollection<Matricula> Matriculas { get; set; }

        private Matricula _selectedMatricula;
        public Matricula SelectedMatricula
        {
            get { return _selectedMatricula; }
            set
            {
                _selectedMatricula = value;
                OnPropertyChanged("SelectedMatricula");
                // libera somente se houver uma matrícula selecionada
                MatriculaAtualizarCommand.RaiseCanExecuteChanged();
                MatriculaRemoverCommand.RaiseCanExecuteChanged();
            }
        }

        private Aluno _selectedAluno;
        public Aluno SelectedAluno
        {
            get { return _selectedAluno; }
            set
            {
                _selectedAluno = value;
                OnPropertyChanged("SelectedAluno");
            }
        }

        // Atributo para acessar o banco de dados
        private MatriculaRepository _repository;
        private AlunoRepository _alunoRepository;

        // Comandos para o CRUD
        public RelayCommand MatriculaAdicionarCommand { get; set; }
        public RelayCommand MatriculaAtualizarCommand { get; set; }
        public RelayCommand MatriculaRemoverCommand { get; set; }
        public RelayCommand FiltrarAlunoCommand { get; set; }
        public RelayCommand GerarPdfCommand { get; set; }

        public MatriculaViewModel()
        {
            Matriculas = new ObservableCollection<Matricula>();
            _repository = new MatriculaRepository();
       
            // Inicializando os comandos
            MatriculaAdicionarCommand = new RelayCommand(AdicionarMatricula);
            MatriculaAtualizarCommand = new RelayCommand(AtualizarMatricula, CanExecuteSubmit);
            MatriculaRemoverCommand = new RelayCommand(RemoverMatricula, CanExecuteSubmit);
            GerarPdfCommand = new RelayCommand(GeraPdf);
            GetAll();
         
        }

        private void GetAll()
        {
            // Busca no banco de dados e carrega em Matriculas
            Matriculas.Clear();
            _repository.GetAll().ForEach(data => Matriculas.Add(data));
        }

        private void GeraPdf(object obj)
        {
            ClassGeraPDF.MatriculaPdf(Matriculas);
        }

        private bool CanExecuteSubmit(object parameter)
        {
            // Validação utilizada para liberar ou não os botões de atualizar e remover na view
            return SelectedMatricula != null;
        }

        private void AdicionarMatricula(object obj)
        {
            WindowCadastrarMatricula janelaCadastro = new WindowCadastrarMatricula();

            // Cheque se o DataContext está configurado
            var viewModel = (MatriculaCadastroViewModel)janelaCadastro.DataContext;

            if (viewModel == null)
            {
                MessageBox.Show("Erro ao carregar a janela de cadastro de matrícula. ViewModel é nulo.");
                return;
            }

            viewModel.MatriculaSalvo += (sender, e) =>
            {
                try
                {
                    var novaMatricula = viewModel.GetMatricula();
                    if (novaMatricula == null)
                    {
                        MessageBox.Show("Erro ao obter a nova matrícula. Matricula é nula.");
                        return;
                    }

                    _repository.Add(novaMatricula);
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

        private void AtualizarMatricula(object obj)
        {
            WindowCadastrarMatricula janelaCadastro = new WindowCadastrarMatricula();
            var viewModel = new MatriculaCadastroViewModel(SelectedMatricula);
            janelaCadastro.DataContext = viewModel;
            viewModel.MatriculaSalvo += (sender, e) =>
            {
                try
                {
                    var matriculaEditada = viewModel.GetMatricula();
                    _repository.Update(matriculaEditada);
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

        private void RemoverMatricula(object obj)
        {
            if (SelectedMatricula == null) return;
            if (MessageBox.Show("Confirma?", "Matrícula", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedMatricula);
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

    
