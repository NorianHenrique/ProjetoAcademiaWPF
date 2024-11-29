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
    /// Lógica interna para a janela de cadastro de frequência.
    /// Esta janela é responsável por gerenciar a interface para o cadastro de frequências na aplicação.
    /// </summary>
    public partial class WindowCadastroFrequencia : Window
    {
        /// <summary>
        /// Construtor da classe <see cref="WindowCadastroFrequencia" />.
        /// Inicializa os componentes e associa o evento Loaded à janela,
        /// permitindo que ajustes na interface sejam realizados quando a janela for carregada.
        /// </summary>
        public WindowCadastroFrequencia()
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

        private void txtCpfColaborador_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCpfAluno.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput;
        }

        private void txtHoraEntrada_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHoraEntrada.PreviewTextInput += ClassFuncoes.TxtHora_PreviewTextInput;
        }

        private void txtHoraSaida_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHoraSaida.PreviewTextInput += ClassFuncoes.TxtHora_PreviewTextInput;
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
