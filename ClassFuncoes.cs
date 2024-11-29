using ProjetoAcademiaWPF.View;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjetoAcademiaWPF
{
    class ClassFuncoes
    {
        /// <summary>
        /// De forma recursiva, varre todos os componentes da tela informada, executando o método ApplyResources em cada um dos componentes localizados.
        /// O ApplyResources realiza a leitura do satellite assembly, ou seja, do arquivo de resource que foi ativo conforme o idioma escolhido pelo usuário,
        /// e com base no nome do campo ajusta todos os parâmetros contidos no resource para aquele campo, por exemplo Text, Content, Font, Size, etc.
        /// Deve possuir, em Properties, um arquivo de resources para cada idioma desejado, devendo ser alimentado na coluna nome o nome do campo e a propriedade que deseja ajustar.
        /// Por exemplo, em cadeias de caracteres labelLogin.Content, em outros textBoxSenha.PasswordChar, ou seja, todas as propriedades podem ser ajustadas conforme o idioma e região em uso.
        /// </summary>
        /// <param name="parent">Informar o container inicial, geralmente this para pegar todos os campos da tela, ou então, por exemplo, o nome de um panel ou usercontrol.</param>
        /// 
        public static void AjustaResources(DependencyObject parent)
        {
            if (parent == null) return;
            // Percorre cada filho direto do objeto pai
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                // Obtém o filho no índice atual
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement element)
                {
                    // Aplicar recursos ao componente
                    ComponentResourceManager resources = new(typeof(Properties.Idioma));
                    resources.ApplyResources(element, element.Name);
                }
                // Chama recursivamente para percorrer os filhos do filho atual
                AjustaResources(child);
            }
        }

        public static void ValidaConexaoDB()
        {
            DbProviderFactory factory;
            string provider = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
            try
            {
                factory = DbProviderFactories.GetFactory(provider);
                using var conexao = factory.CreateConnection();
                conexao!.ConnectionString = connectionString;
                using var comando = factory.CreateCommand();
                comando!.Connection = conexao;
                conexao.Open();
            }
            catch (DbException ex)
            {
                MessageBox.Show($"{ex.Source}\n\n{ex.Message}\n\n{ex.ErrorCode}\n\n{ex.SqlState}\n\n{ex.StackTrace}");
                var auxConfig = new WindowConfiguracoes(provider, connectionString);
                auxConfig.ShowDialog();
                ValidaConexaoDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Source}\n\n{ex.Message}\n\n{ex.StackTrace}");
                var auxConfig = new WindowConfiguracoes(provider, connectionString);
                auxConfig.ShowDialog();
                ValidaConexaoDB();
            }
        }

        /// <summary>
        /// Ajusta o idioma e a cultura da aplicação com base nas configurações especificadas. 
        /// O método lê o valor da chave "IdiomaRegiao" do arquivo de configuração da aplicação e define a cultura para a thread atual.
        /// O valor lido deve ser uma string representando a cultura desejada, tal como "pt-BR", "en-US" ou "es-ES".
        /// Caso o valor não esteja disponível, é tratado para não causar uma exceção, inicializando a cultura com uma string vazia.
        /// Assim, o idioma e a região da aplicação são configurados adequadamente para garantir que as strings e formatações sejam exibidas conforme o idioma do usuário.
        /// </summary>
        public static void AjustaIdiomaRegiao()
        {
            // pt-BR, en-US, es-ES
            // ? indica que o valor pode ser nulo
            string? auxIdiomaRegiao = ConfigurationManager.AppSettings.Get("IdiomaRegiao");
            // no ternário estamos tratando para isso não acontecer
            string idiomaRegiao = (auxIdiomaRegiao is not null) ? auxIdiomaRegiao : "";
            // Definir a cultura e ajusta o idioma/região
            // o operador ! (null-forgiving) afirma que o valor já foi tratado e não será nulo aqui
            CultureInfo culture = new(idiomaRegiao!);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

       // public static string Sha256Hash(string senha)
        //{
            // Usar SHA256 para criar hash da senha
            //using var sha256 = System.Security.Cryptography.SHA256.Create();
           // byte[] bytes =
            //sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
           // return Convert.ToBase64String(bytes);
        /// <summary>
        /// }
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        // Evento para capturar a entrada de texto no TextBox e aplicar a máscara de CPF
        public static void TxtCPF_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBoxCpf)
            {
                e.Handled = !Regex.IsMatch(e.Text, @"^\d+$"); // Ignora entrada se não for um dígito
                if (!e.Handled)
                {
                    // Recupera o texto atual do TextBox
                    var text = textBoxCpf.Text;
                    // Remove qualquer caracter não numérico
                    text = Regex.Replace(text, @"[^\d]", "");
                    // Adiciona o novo caractere
                    text += e.Text;
                    // Aplica a máscara de CPF (###.###.###-##)
                    if (text.Length <= 11)
                    {
                        if (text.Length > 9)
                        {
                            textBoxCpf.Text = $"{text.Substring(0, 3)}.{text.Substring(3, 3)}.{text.Substring(6, 3)}-{text.Substring(9)}";
                        }
                        else if (text.Length > 6)
                        {
                            textBoxCpf.Text = $"{text.Substring(0, 3)}.{text.Substring(3, 3)}.{text.Substring(6)}";
                        }
                        else if (text.Length > 3)
                        {
                            textBoxCpf.Text = $"{text.Substring(0, 3)}.{text.Substring(3)}";
                        }
                        else
                        {
                            textBoxCpf.Text = text;
                        }
                        textBoxCpf.CaretIndex = textBoxCpf.Text.Length; // Move o cursor para o fim
                    }
                    e.Handled = true; // Bloqueia a entrada direta no TextBox
                }
            }
        }

        // Máscara para CEP
        public static void TxtCEP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se a entrada é numérica
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != '\b') // Permite backspace
            {
                e.Handled = true; // Ignora a entrada se não for numérica
                return;
            }

            // Formatação do CEP
            var textBox = sender as TextBox;
            var text = textBox.Text.Replace("-", "").Replace(".", ""); // Remove qualquer formatação anterior

            // Limita a entrada a 8 dígitos
            if (text.Length < 8)
            {
                text += e.Text; // Adiciona o novo caractere
                e.Handled = text.Length > 8; // Se exceder 8 dígitos, ignora a entrada
            }

            if (text.Length >= 5)
            {
                // Adiciona a formatação "00000-"
                text = text.Insert(5, "-");
            }

            textBox.Text = text;
            textBox.CaretIndex = textBox.Text.Length; // Move o cursor para o final
        }

        // Máscara para Telefone
        public static void TxtTelefone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBoxTelefone)
            {
                e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
                if (!e.Handled)
                {
                    var text = ApplyMask(textBoxTelefone.Text, e.Text, @"^\d{0,11}$", "(##) #####-####");
                    textBoxTelefone.Text = text;
                    textBoxTelefone.CaretIndex = textBoxTelefone.Text.Length;
                    e.Handled = true;
                }
            }
        }

        // Máscara para Data
        public static void TxtData_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBoxData)
            {
                e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
                if (!e.Handled)
                {
                    var text = ApplyMask(textBoxData.Text, e.Text, @"^\d{0,10}$", "##/##/####");
                    textBoxData.Text = text;
                    textBoxData.CaretIndex = textBoxData.Text.Length;
                    e.Handled = true;
                }
            }
        }

        // Máscara para Data e Hora
        public static void TxtHora_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Permite apenas números e ":" para backspace
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != ':' && e.Text[0] != '\b')
            {
                e.Handled = true;
                return;
            }

            var textBox = sender as TextBox;
            var textoAntigo = textBox.Text;
            var textoNovo = textoAntigo + e.Text; // Cria uma nova string com a entrada

            // Mantém a estrutura "HH:mm"
            if (textoNovo.Length == 2)
            {
                textoNovo += ":"; // Adiciona o ":" após o segundo dígito
            }

            e.Handled = textoNovo.Length > 5; // Limita a 5 caracteres no total (HH:mm)

            if (!e.Handled) // Apenas atualiza se não foi tratado
            {
                textBox.Text = textoNovo;
                textBox.CaretIndex = textoNovo.Length; // Move o cursor para o final
            }
        }

        // Máscara para Valor
        public static void TxtValor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBoxValor)
            {
                e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
                if (!e.Handled)
                {
                    var text = ApplyMask(textBoxValor.Text, e.Text, @"^\d{0,15}$", "C"); // 'C' para valor monetário
                    textBoxValor.Text = text;
                    textBoxValor.CaretIndex = textBoxValor.Text.Length;
                    e.Handled = true;
                }
            }
        }

        public static void TxtPeso_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Permite apenas números e um ponto (.)
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != '.' && e.Text[0] != '\b') // Permite backspace
            {
                e.Handled = true;
                return;
            }

            var textBox = sender as TextBox;
            var textoAntigo = textBox.Text;

            // Se já houver um ponto, não pode inserir outro
            if (textoAntigo.Contains(".") && e.Text[0] == '.')
            {
                e.Handled = true;
                return;
            }

            // Cria uma nova string com a entrada atual
            var textoNovo = textoAntigo + e.Text;

            // Permite limitar o número de caracteres (opcional)
            // Por exemplo, se for um peso que deverá ser inserido em formato "XXX.XX"
            if (textoNovo.Length > 6) // Ajuste o número conforme necessário
            {
                e.Handled = true;
                return;
            }

            // Atualiza o valor do TextBox
            if (!e.Handled)
            {
                // Atualiza o TextBox para a nova string (textBox.Text)
                textBox.Text = textoNovo;
                textBox.CaretIndex = textoNovo.Length; // Move o cursor para o final
            }
        }

        public static void TxtAltura_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Mesma lógica aplicada para altura
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != '.' && e.Text[0] != '\b') // Permite backspace
            {
                e.Handled = true;
                return;
            }

            var textBox = sender as TextBox;
            var textoAntigo = textBox.Text;

            // Se já houver um ponto, não pode inserir outro
            if (textoAntigo.Contains(".") && e.Text[0] == '.')
            {
                e.Handled = true;
                return;
            }

            // Cria uma nova string com a entrada atual
            var textoNovo = textoAntigo + e.Text;

            // Permite limitar o número de caracteres (opcional)
            // Por exemplo, se for uma altura que deverá ser inserido em formato "X.XX"
            if (textoNovo.Length > 5) // Ajuste o número conforme necessário
            {
                e.Handled = true;
                return;
            }

            // Atualiza o valor do TextBox
            if (!e.Handled)
            {
                // Atualiza o TextBox para a nova string (textBox.Text)
                textBox.Text = textoNovo;
                textBox.CaretIndex = textoNovo.Length; // Move o cursor para o final
            }
        }


        // Método auxiliar para aplicar a máscara
        private static string ApplyMask(string currentText, string newText, string regexPattern, string mask)
        {
            // Remove caracteres não numéricos para obter apenas os dígitos
            var digitsOnly = Regex.Replace(currentText + newText, @"[^\d]", "");

            // Verifica de acordo com a máscara o que deve ser aplicado
            if (digitsOnly.Length > 0 && Regex.IsMatch(digitsOnly, regexPattern))
            {
                var maskedText = string.Empty;
                var maskIndex = 0;

                foreach (var character in mask)
                {
                    if (maskIndex >= digitsOnly.Length) break;

                    if (character == '#')
                    {
                        maskedText += digitsOnly[maskIndex];
                        maskIndex++;
                    }
                    else
                    {
                        maskedText += character;
                    }
                }

                return maskedText;
            }

            return currentText; // Retorna o texto atual se a máscara não se aplicar
        }

        /// <summary>
        /// Tratar eventos de teclado, no caso tecla ENTER funcionando com TAB e tecla ESC para fechar
        /// </summary>
        /// <param name="sender">Objeto que gerou o evento</param>
        /// <param name="e">Evento que foi capturado</param>
        /// <example>No construtor do formulário:
        ///this.KeyDown += new System.Windows.Input.KeyEventHandler(ClassFuncoes.Window_KeyDown);
        ///</example>
        public static void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Se a tecla ENTER for pressionada
            if (e.Key == Key.Enter)
            {
                // Move o foco para o próximo controle, como o TAB faria
                var focusedElement = Keyboard.FocusedElement as UIElement;
                // Move o foco para o próximo controle na ordem de tabulação
                focusedElement?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                e.Handled = true; // Previne comportamento padrão do ENTER (como som)
            }
            // Se a tecla ESC for pressionada
            else if (e.Key == Key.Escape)
            {
                // verifica se é window e fecha
                if (sender is Window window)
                {
                    window.Close();
                }
                // carrega uma página inicial
                else
                {
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        // precisa passar o método para public
                        mainWindow.ButtonHome_Click(sender, e);
                    }
                }
            }
        }
    }
}
