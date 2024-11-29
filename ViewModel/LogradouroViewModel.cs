using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Common;
using ProjetoAcademiaWPF.Model;
using System.Collections.ObjectModel;
using System.Runtime.ConstrainedExecution;
using ProjetoAcademiaWPF.DataAccess;
using System.Windows.Input;
using System.Windows;
using ProjetoAcademiaWPF.Pdf;


namespace ProjetoAcademiaWPF.ViewModel
{
    public class LogradouroViewModel : ViewModelBase
    {
        // Objetos utilizados no Databinding
        // Recurso que permite a sincronização automática entre View e ViewModel,
        // através da propriedade DataContext da View.
        public ObservableCollection<Logradouro>Logradouros { get; set; }
        private Logradouro _selectedLogradouro;
        public Logradouro SelectedLogradouro
        {
            get { return _selectedLogradouro; }
            set
            {
                _selectedLogradouro = value;
                OnPropertyChanged("SelectedLogradouro");

                LogradouroAtualizarCommand.RaiseCanExecuteChanged();
                LogradouroRemoverCommand.RaiseCanExecuteChanged();
                LogradouroAdicionarCommand.RaiseCanExecuteChanged();
            }
        }

        private LogradouroRepository _repository;

        //Comandos para CRUD
        public RelayCommand LogradouroAdicionarCommand { get; set; }
        public RelayCommand LogradouroAtualizarCommand { get; set; }
        public RelayCommand LogradouroRemoverCommand { get; set; }
        public RelayCommand FiltrarLogradouroCommand { get; set; }
        public RelayCommand GerarPdfCommand { get; set; }

        public LogradouroViewModel()
        {
            Logradouros = new ObservableCollection<Logradouro>();
            _repository = new LogradouroRepository();

            LogradouroAdicionarCommand = new RelayCommand(AdicionarLogradouro,CanExecuteSubmit);
            LogradouroAtualizarCommand = new RelayCommand(AtualizarLogradouro,CanExecuteSubmit);
            LogradouroRemoverCommand = new RelayCommand(RemoverLogradouro,CanExecuteSubmit);
            FiltrarLogradouroCommand = new RelayCommand(FiltrarLogradouro);
            GerarPdfCommand = new RelayCommand(GeraPdf);
            GetAll();
        }

        public void GetAll()
        {
            // busca no banco de dados e carrega em Logradouros
            Logradouros.Clear();
            _repository.GetAll().ForEach(data => Logradouros.Add(data));
        }

        private bool CanExecuteSubmit(object parameter)
        {
            // validação utilizada para liberar ou não os botões de atualizar e remover na view
            return SelectedLogradouro != null;
        }

        private void GeraPdf(object obj)
        {
            ClassGeraPDF.LogradourosPdf(Logradouros);
        }

        // métodos com as operações de CRUD aqui
        private void AdicionarLogradouro(object obj)
        {
            if (SelectedLogradouro == null) return;
            if (MessageBox.Show("Confirma?", "Logradouro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Add(SelectedLogradouro);
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

        private void AtualizarLogradouro(object obj)
        {
            if (SelectedLogradouro == null) return;
            if (MessageBox.Show("Confirma?", "Logradouro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Update(SelectedLogradouro);

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

        private void RemoverLogradouro(object obj)
        {
            if (SelectedLogradouro == null) return;
            if (MessageBox.Show("Confirma?", "Logradouro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedLogradouro);

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

        private void FiltrarLogradouro(object parameter)
        {
            string cep = parameter as string;
            if (string.IsNullOrWhiteSpace(cep))
            {
                MessageBox.Show("Por favor, insira um CEP válido.");
                return;
            }

            var logradouro = new Logradouro
            {
                Cep = cep
            };

            Logradouro resultado = _repository.GetOne(logradouro);

            if (resultado != null && resultado.Id > 0)
            {
                SelectedLogradouro = resultado; // Atribui o resultado apenas se não for nulo
            }
            else
            {
                MessageBox.Show("Nenhum logradouro encontrado para o CEP informado.");
                SelectedLogradouro = null; // Limpa a seleção se não encontrar
            }
        }
    }
}
