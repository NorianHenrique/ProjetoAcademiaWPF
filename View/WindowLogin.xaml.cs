using ProjetoAcademiaWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetoAcademiaWPF.View
{
    /// <summary>
    /// Lógica interna para a janela de login.
    /// Esta janela é responsável por gerenciar a interface de login da aplicação, onde os usuários podem autenticar-se.
    /// </summary>
    public partial class WindowLogin : Window
    {
        /// <summary>
        /// Construtor da classe <see cref="WindowLogin" />.
        /// Inicializa os componentes da janela e associa o evento Loaded para ajustes de recursos da interface.
        /// </summary>
        public WindowLogin()
        {
            InitializeComponent();

            DataContext = new ViewModel.ColaboradorViewModel();

            this.Loaded += Page_Loaded;

            // registra o evento de login bem sucedido
            if (DataContext is ColaboradorViewModel colaboradorViewModel)
            {
                colaboradorViewModel.LoginSucceeded += OnLoginSucceeded; // Registro do evento de sucesso
                colaboradorViewModel.ErrorOccurred += ViewModel_ErrorOccurred; // Registro do evento de erro
            }

        }

        // trata o evento de login bem sucedido, fechando a janela de login
        private void OnLoginSucceeded(object? sender, EventArgs e)
        {
            this.DialogResult = true; // Define o resultado do diálogo como verdadeiro
            this.Close(); // Fecha a janela
        }
        
        private void ViewModel_ErrorOccurred(object sender, string errorMessage)
        {
            MessageBox.Show("Error: " + errorMessage);
        }

        /// <summary>
        /// Manipulador de evento chamado quando a janela é carregada.
        /// Chama o método <see cref="ClassFuncoes.AjustaResources(DependencyObject)"/> 
        /// para aplicar os recursos de idioma e outros ajustes necessários na interface.
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
            this.KeyDown += ClassFuncoes.Window_KeyDown;
            txtCpf.Focus();
        }

        private void Box_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.LightCyan;
            if(sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Background = cor;
            }
            else if(sender is PasswordBox)
            {
                PasswordBox passwordBox = (PasswordBox)sender;
                passwordBox.Background = cor;
            }
        }

        private void Box_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.White;
            if(sender is TextBox)
                {
                    TextBox textBox = (TextBox)sender;
                    textBox.Background = cor;

                }
            else if(sender is PasswordBox)
            {
                    PasswordBox passwordBox = (PasswordBox)sender;
                    passwordBox.Background = cor;
            }
        }

        /// <summary>
        /// Manipulador de evento chamado quando o botão de cancelar é clicado.
        /// Fecha a janela de login, sem realizar ações adicionais.
        /// </summary>
        /// <param name="sender">O objeto que gerou o evento.</param>
        /// <param name="e">Os argumentos do evento.</param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtCpfColaborador_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCpf.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput;
        }
    }
}
