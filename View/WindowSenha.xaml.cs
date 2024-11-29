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
    /// Lógica interna para a janela de senha.
    /// Esta janela permite que os usuários gerenciem a senha, proporcionando uma interface para redefinir ou atualizar suas credenciais.
    /// </summary>
    public partial class WindowSenha : Window
    {
        /// <summary>
        /// Construtor da classe <see cref="WindowSenha" />.
        /// Inicializa os componentes da janela e associa o evento Loaded para ajustes de recursos da interface.
        /// </summary>
        public WindowSenha()
        {
            InitializeComponent();

            this.Loaded += Page_Loaded;
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
            txtCPF.Focus();
        }

        /// <summary>
        /// Manipulador de evento chamado quando o botão de cancelar é clicado.
        /// Fecha a janela de senha, sem realizar ações adicionais.
        /// </summary>
        /// <param name="sender">O objeto que gerou o evento.</param>
        /// <param name="e">Os argumentos do evento.</param>
        /// 

        private void Box_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.LightCyan;
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Background = cor;
            }
            else if (sender is PasswordBox)
            {
                PasswordBox passwordBox = (PasswordBox)sender;
                passwordBox.Background = cor;
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
            else if (sender is PasswordBox)
            {
                PasswordBox passwordBox = (PasswordBox)sender;
                passwordBox.Background = cor;
            }
        }

        private void txtCpfColaborador_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCPF.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
