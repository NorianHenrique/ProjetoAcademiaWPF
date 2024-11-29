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
    /// Interação lógica para PageListaMatricula.xam
    /// </summary>
    public partial class PageListaMatricula : Page
    {
        private string ConnectionString { get; set; }

        private string ProviderName { get; set; }

        public PageListaMatricula(string providerName, string connectionString)
        {
            InitializeComponent();

            ConnectionString = connectionString;
            ProviderName = providerName;
        }
    }
}
