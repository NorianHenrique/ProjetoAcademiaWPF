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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetoAcademiaWPF.View
{
    /// <summary>
    /// Interação lógica para PageListaLogradouro.xam
    /// </summary>
    public partial class PageListaLogradouro : Page
    {
        private string ConnectionString { get; set; }

        private string ProviderName { get; set; }

        private LogradouroViewModel ViewModelLogradouro;

        public PageListaLogradouro(string providerName,string connectionString)
        {
            InitializeComponent();

            ConnectionString = connectionString;
            ProviderName = providerName;

            
        }

       
    }
}
