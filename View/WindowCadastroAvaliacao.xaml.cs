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
    /// Lógica interna para a janela de cadastro de avaliação.
    /// Esta janela é responsável por gerenciar a interface para o cadastro de avaliações na aplicação.
    /// </summary>
    public partial class WindowCadastroAvaliacao : Window
    {
        /// <summary>
        /// Construtor da classe <see cref="WindowCadastroAvaliacao" />.
        /// Inicializa os componentes e associa o evento Loaded à janela,
        /// permitindo que ajustes na interface sejam realizados quando a janela for carregada.
        /// </summary>
        public WindowCadastroAvaliacao()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        /// <summary>
        /// Manipulador de evento chamado quando a janela é carregada.
        /// Chama o método <see cref="ClassFuncoes.AjustaResources(DependencyObject)"/> 
        /// para aplicar os recursos de idioma definidos para os componentes da janela.
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
            this.KeyDown += ClassFuncoes.Window_KeyDown;
            txtIdAluno.Focus();
        }

        private void txtPeso_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Associa a máscara ao campo de Peso
            txtPeso.PreviewTextInput += ClassFuncoes.TxtPeso_PreviewTextInput;
        }

        private void txtAltura_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Associa a máscara ao campo de Altura
            txtAltura.PreviewTextInput += ClassFuncoes.TxtAltura_PreviewTextInput;
        }

        private void txtDataHora_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHora.PreviewTextInput += ClassFuncoes.TxtHora_PreviewTextInput;
        }

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

        /// <summary>
        /// Manipulador de evento chamado quando o botão de cancelar é clicado.
        /// Fecha a janela sem realizar nenhuma alteração.
        /// </summary>
        /// <param name="sender">O objeto que gerou o evento.</param>
        /// <param name="e">Os argumentos do evento.</param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
