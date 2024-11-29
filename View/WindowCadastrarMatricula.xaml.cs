using Microsoft.Win32;
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
using ProjetoAcademiaWPF.ViewModel;

namespace ProjetoAcademiaWPF.View
{
    /// <summary>
    /// Lógica interna para WindowCadastrarMatricula.xaml
    /// </summary>
    public partial class WindowCadastrarMatricula : Window
    {
        public WindowCadastrarMatricula()
        {
            InitializeComponent();

            DataContext =  new MatriculaCadastroViewModel();

            this.Loaded += Page_Loaded;
        }

        /// <summary>
        /// Manipulador de evento chamado quando a página é carregada.
        /// Chama o método <see cref="ClassFuncoes.AjustaResources(DependencyObject)"/> 
        /// para aplicar os recursos de idioma definidos para os componentes da página.
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
            this.KeyDown += ClassFuncoes.Window_KeyDown;
            txtIdAluno.Focus();
        }

        private void txtIdAluno_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Associa a máscara ao campo ID Aluno (pode ser CPF ou ID)
            txtIdAluno.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput; // Ajuste conforme necessário
        }

        private void txtIdAtendente_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Associa a máscara ao campo ID Atendente (pode ser CPF ou ID)
            txtIdAtendente.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput; // Ajuste conforme necessário
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
        /// Manipulador de evento acionado quando o botão para upload do laudo médico é clicado.
        /// Exibe um diálogo para seleção de arquivos de imagem e, se uma imagem for selecionada,
        /// ela será exibida na interface. O caminho do arquivo pode ser armazenado para uso posterior.
        /// </summary>
        /// <param name="sender">A origem do evento, que deve ser o botão de upload.</param>
        /// <param name="e">Argumentos do evento que contêm informações sobre o clique do botão.</param>
       
    }
}

