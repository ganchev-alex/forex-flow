using ForexFlow.ViewModel.DataTransferObjects;
using ForexFlow.ViewModel.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ForexFlow.View
{
    /// <summary>
    /// Interaction logic for AmountsManagement.xaml
    /// </summary>
    public partial class AmountsManagement : Window
    {

        public AmountsManagement(AmountsManagementViewModel amountsManagementViewModel)
        {
            InitializeComponent();
            DataContext = amountsManagementViewModel;
        }

        private void OnOpenActions(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button?.DataContext as DisplayedSingleCurrencyAmount;

            if (dataContext != null)
            {
                var accountId = dataContext.AmountId;

                var actionsWindow  = new SingleAmountActions(accountId);
                actionsWindow.Show();
                this.Close();
            }
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            MainWindow homeMenu = new MainWindow();
            homeMenu.Show();
            this.Hide();
        }
    }
}
