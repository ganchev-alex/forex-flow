using ForexFlow.DataAccess.Repository;
using ForexFlow.ViewModel.ViewModels;
using System.Windows;

namespace ForexFlow.View
{
    /// <summary>
    /// Interaction logic for AddCurrencyForm.xaml
    /// </summary>
    public partial class AddCurrencyForm : Window
    {
        public AddCurrencyForm(CurrencyManagementViewModel currencyManagementViewModel)
        {
            InitializeComponent();
            DataContext = currencyManagementViewModel;
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
