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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetoAcademiaWPF.View
{
    /// <summary>
    /// Interação lógica para o UserControl de logradouro.
    /// Este UserControl é responsável por gerenciar a interface relacionada a logradouros
    /// e pode ser reutilizado em diferentes partes da aplicação.
    /// </summary>
    public partial class UserControlLogradouro : UserControl
    {
        /// <summary>
        /// Construtor da classe <see cref="UserControlLogradouro" />.
        /// Inicializa os componentes e associa o evento Loaded à instância do UserControl,
        /// permitindo que ajustes na interface sejam realizados quando o UserControl for carregado.
        /// </summary>
        public UserControlLogradouro()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        /// <summary>
        /// Manipulador de evento chamado quando o UserControl é carregado.
        /// Chama o método <see cref="ClassFuncoes.AjustaResources(DependencyObject)"/> 
        /// para aplicar os recursos de idioma definidos para os componentes do UserControl.
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
            this.KeyDown += ClassFuncoes.Window_KeyDown;
            txtCEP.Focus();
        }

        private void txtCEP_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Associa a máscara ao campo de CEP
            txtCEP.PreviewTextInput += ClassFuncoes.TxtCEP_PreviewTextInput;
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
    }
}
