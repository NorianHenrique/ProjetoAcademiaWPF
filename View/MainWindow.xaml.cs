using Microsoft.Windows.Themes;
using ProjetoAcademiaWPF.ViewModel;
using System.Configuration;
using System.Globalization;
using System.Windows;
using System.Windows.Input;


namespace ProjetoAcademiaWPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        private const string Administrador = "Administrador";
        private const string Atendente = "Atendente";
        private const string Instrutor = "Instrutor";


        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ClassFuncoes.ValidaConexaoDB();

            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;

            LiberaMenus(false, "0");

            this.Closing += MainWindow_Closing;
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
            this.KeyDown += ClassFuncoes.Window_KeyDown;

        }

        // Implementação do evento para abrir o menu de contexto com clique do botão direito
        private void ScrollViewer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (contextMenu != null)
            {
                contextMenu.IsOpen = true;
            }
        }


        private void ChangeLanguage(string cultureCode)
        {
            // en-US, es-ES, pt-BR
            // Definir a cultura
            CultureInfo culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            // Recargar a interface do usuário para refletir as mudanças
            var oldWindow = this;
            var newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            oldWindow.Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Exibir mensagem de confirmação
            MessageBoxResult result = MessageBox.Show(
                GetConfirmationMessage(),
                GetConfirmationTitle(),
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            // Verifique se o usuário escolheu "Não"
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Cancela o fechamento da janela
            }
        }

        private string GetConfirmationMessage()
        {
            // Aqui retornar a mensagem com base no idioma configurado
            return Properties.Idioma.confirmacaoFecharAplicacao;

        }

        private string GetConfirmationTitle()
        {
            // Aqui retornar o título da mensagem conforme o idioma configurado
            return Properties.Idioma.tituloConfirmacao;
        }

        private void ButtonLogradouro_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaLogradouro)
            {
                MainFrame.Content = new PageListaLogradouro(ProviderName, ConnectionString);
            }
            MessageBox.Show("Lista Logradouros.");
        }

        private void ButtonAluno_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaAluno)
            {
                MainFrame.Content = new PageListaAluno(ProviderName, ConnectionString);
            }
            MessageBox.Show("Lista Alunos.");
        }

        private void ButtonColaborador_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaColaborador)
            {
                MainFrame.Content = new PageListaColaborador(ProviderName, ConnectionString);
            }
            MessageBox.Show("Lista Colaboradores.");
        }

        private void ButtonSenha_Click(object sender, RoutedEventArgs e)
        {

            WindowSenha senhaWindow = new WindowSenha();
            MessageBox.Show("Adiocionar Senha.");
            senhaWindow.ShowDialog();

        }

        private void ButtonMatricula_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaMatricula)
            {
                MainFrame.Content = new PageListaMatricula(ProviderName, ConnectionString);
            }
            MessageBox.Show("Lista Matrículas.");
        }

        private void ButtonAvaliacao_Click(object sender, RoutedEventArgs e)
        {
            WindowCadastroAvaliacao cadastroAvaliacao = new WindowCadastroAvaliacao();
            MessageBox.Show("Adiocionar Avaliação.");
            cadastroAvaliacao.ShowDialog();

        }

        private void ButtonFrequencia_Click(object sender, RoutedEventArgs e)
        {
            WindowCadastroFrequencia cadastroFrequencia = new WindowCadastroFrequencia();
            MessageBox.Show("Adiocionar Frequência.");
            cadastroFrequencia.ShowDialog();

        }

        private void ButtonLoginLoggout_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonHome.IsEnabled)
            {
                LiberaMenus(false, "0");
                txtColaboradorLogado.Text = "Bem-vindo!"; // Reseta o texto ao deslogar
                return;
            }

            WindowLogin windowLogin = new WindowLogin();
            windowLogin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var colaboradorViewModel = windowLogin.DataContext as ColaboradorViewModel;
            windowLogin.ShowDialog();

            if (colaboradorViewModel.SelectedColaborador != null && colaboradorViewModel.SelectedColaborador.Id > 0)
            {
                txtColaboradorLogado.Text = $"Tipo: {colaboradorViewModel.SelectedColaborador.Tipo}";
                LiberaMenus(true, colaboradorViewModel.SelectedColaborador.Tipo.ToString());
            }
            else
            {
                txtColaboradorLogado.Text = "Login falhou. Nenhum colaborador selecionado."; // Mensagem de erro
                LiberaMenus(false, "0");
            }

        }


        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            WindowConfiguracoes windowConfig = new(ProviderName,ConnectionString);
            windowConfig.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MessageBox.Show("Acessar Configurações.");
            windowConfig.ShowDialog();
            // Recarregar a interface do usuário para refletir as mudanças
            var newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            Close();
        }

        public void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Menu Principal.");
        }

        public void LiberaMenus(bool liberar, string grupo)
        {
            if (!liberar)
            {
                // Desabilita todos os botões
                ButtonHome.IsEnabled = liberar;
                ButtonLogradouro.IsEnabled = liberar;
                ButtonAluno.IsEnabled = liberar;
                ButtonColaborador.IsEnabled = liberar;
                ButtonMatricula.IsEnabled = liberar;
                ButtonAvaliacao.IsEnabled = liberar;
                ButtonFrequencia.IsEnabled = liberar;
                ButtonAulas.IsEnabled = liberar;
                ButtonTreinos.IsEnabled = liberar;
                ButtonLoginLoggout.IsEnabled = !liberar;
                ButtonConfig.IsEnabled = liberar;

                // Executar evento botão login
                ButtonHome_Click(null, null);
            }
            else if (grupo.Equals(Administrador)) // Administrador
            {
                // Todos os botões habilitados
                ButtonHome.IsEnabled = liberar;
                ButtonLogradouro.IsEnabled = liberar;
                ButtonAluno.IsEnabled = liberar;
                ButtonColaborador.IsEnabled = liberar;
                ButtonMatricula.IsEnabled = liberar;
                ButtonAvaliacao.IsEnabled = liberar;
                ButtonFrequencia.IsEnabled = liberar;
                ButtonAulas.IsEnabled = liberar;
                ButtonTreinos.IsEnabled = liberar;
                ButtonLoginLoggout.IsEnabled = liberar;
                ButtonConfig.IsEnabled = liberar;
            }
            else if (grupo.Equals(Atendente)) // Atendente
            {
                /* Cadastro logradouro, alunos, senha, matrícula, aula, registro frequência aluno */
                ButtonHome.IsEnabled = liberar;
                ButtonLogradouro.IsEnabled = liberar;
                ButtonAluno.IsEnabled = liberar;
                ButtonColaborador.IsEnabled = !liberar;
                ButtonMatricula.IsEnabled = liberar;
                ButtonAvaliacao.IsEnabled = !liberar;
                ButtonFrequencia.IsEnabled = liberar;
                ButtonAulas.IsEnabled = liberar;
                ButtonTreinos.IsEnabled = !liberar;
                ButtonLoginLoggout.IsEnabled = liberar;
                ButtonConfig.IsEnabled = !liberar;
            }
            else if (grupo.Equals(Instrutor)) // Instrutor
            {
                /* Cadastro avaliação, treinos, aulas, registro frequência aluno */
                ButtonHome.IsEnabled = liberar;
                ButtonLogradouro.IsEnabled = !liberar;
                ButtonAluno.IsEnabled = !liberar;
                ButtonColaborador.IsEnabled = !liberar;
                ButtonMatricula.IsEnabled = !liberar;
                ButtonAvaliacao.IsEnabled = liberar;
                ButtonFrequencia.IsEnabled = liberar;
                ButtonAulas.IsEnabled = liberar;
                ButtonTreinos.IsEnabled = liberar;
                ButtonLoginLoggout.IsEnabled = liberar;
                ButtonConfig.IsEnabled = !liberar;
            }
        }

        private void ButtonTreinos_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageTreino());
            MessageBox.Show("Adiocionar Treinos.");
        }

        private void ButtonAulas_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageAulas());
            MessageBox.Show("Adiocionar Aulas.");
        }
    }
}