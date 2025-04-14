using ForexFlow.ViewModel.ViewModels;
using System.Windows;

namespace ForexFlow.View
{
    public partial class CurrencyManagement : Window
    {
        private readonly CurrencyManagementViewModel _sharedContext;

        public CurrencyManagement(CurrencyManagementViewModel currencyManagementViewModel)
        {
            InitializeComponent();
            DataContext = currencyManagementViewModel;
            _sharedContext = currencyManagementViewModel;
        }

        private void OpenAddForm(object sender, RoutedEventArgs e)
        {
            AddCurrencyForm currencyMangement = new AddCurrencyForm(_sharedContext);
            currencyMangement.Show();
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            MainWindow homeMenu = new MainWindow();
            homeMenu.Show();
            this.Close();
        }
    }
}

