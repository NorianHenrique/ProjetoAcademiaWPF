using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace ProjetoAcademiaWPF.View
{
    /// <summary>
    /// Lógica interna para a janela de configurações.
    /// Esta janela permite ao usuário alterar as configurações da aplicação, como idioma e região.
    /// </summary>
    /// 

    public partial class WindowConfiguracoes : Window
    {
        /// <summary>
        /// Construtor da classe <see cref="WindowConfiguracoes" />.
        /// Inicializa os componentes da janela e define o idioma selecionado com base nas configurações atuais da aplicação.
        /// </summary>
        /// 
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }
        public WindowConfiguracoes(string providerName, string connectionString)
        {
            InitializeComponent();

            // busca os dados de conexão com o banco de dados, do arquivo de configuração
            ConnectionString = connectionString;
            ProviderName = providerName;

            // seleciona no comboBox o idioma/cultura atual
            cmbIdioma.SelectedItem = ConfigurationManager.AppSettings.Get("IdiomaRegiao");
            // atribui os valores aos controles da janela

            comboBoxProvider.SelectedItem = ProviderName;
            textBoxStringDeConexao.Text = ConnectionString;
            this.KeyDown += new System.Windows.Input.KeyEventHandler(ClassFuncoes.Window_KeyDown);
        }

      

        private void CarregaConfiguracao()
        {
            // Configura os ComboBoxes e TextBox com os valores atuais
            cmbIdioma.SelectedItem = ConfigurationManager.AppSettings.Get("IdiomaRegiao");
            comboBoxProvider.SelectedItem = ProviderName;
            textBoxStringDeConexao.Text = ConnectionString;
        }

        private void Box_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.LightCyan;
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Background = cor;
            }
            else if (sender is ComboBox)
            {
                ComboBox comboBox = (ComboBox)sender;
                comboBox.Background = cor;
            }
        }

        private void Box_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.White;
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Background = cor;

            }
            else if (sender is ComboBox)
            {
                ComboBox comboBox = (ComboBox)sender;
                comboBox.Background = cor;
            }
        }

        /// <summary>
        /// Manipulador de evento chamado quando o botão de salvar é clicado.
        /// Atualiza as configurações do idioma e região na aplicação, salva as alterações no arquivo de configuração,
        /// e atualiza a cultura atual da aplicação.
        /// </summary>
        /// <param name="sender">O objeto que gerou o evento.</param>
        /// <param name="e">Os argumentos do evento.</param>
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            // Abre o arquivo local como leitura/escrita e salva as alterações em AcademiaDoZe_WPF.dll.config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("IdiomaRegiao");
            config.AppSettings.Settings.Add("IdiomaRegiao", cmbIdioma.Text);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            // Atualiza a cultura corrente
            ClassFuncoes.AjustaIdiomaRegiao();
            Close();
            _ = MessageBox.Show("Idioma/região alterada com sucesso!");
        }

        private void SalvarBD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //abre o arquivo local como leitura/escrita - ControleEstoqueDoZe.exe.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //altera os valores de provider e da connectionStrings com nome BD
                config.ConnectionStrings.ConnectionStrings["BD"].ProviderName = comboBoxProvider.Text;
                config.ConnectionStrings.ConnectionStrings["BD"].ConnectionString = textBoxStringDeConexao.Text;
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings"); _ = MessageBox.Show("Dados alterados com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        private void encerrarAplicacao_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
