﻿using ProjetoAcademiaWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica interna para WindowCadastrarAluno.xaml
    /// </summary>
    public partial class WindowCadastrarAluno : Window
    {
        public WindowCadastrarAluno()
        {
            InitializeComponent();

            DataContext = new AlunoCadastroViewModel();

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
            txtNome.Focus();
        }

        private void txtCpfColaborador_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCPF.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput;
        }

        private void TextBox_Telefone_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Associa a máscara ao campo Telefone
            txtTelefone.PreviewTextInput += ClassFuncoes.TxtTelefone_PreviewTextInput;
        }

        private void txtDataNascimento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Associa a máscara ao campo Data
            if (sender is DatePicker datePicker)
            {
                // Aqui capturar apenas o texto
                e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
            }
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

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
