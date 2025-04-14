using ForexFlow.ViewModel.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;


namespace ForexFlow.View
{
    public partial class SingleAmountActions : Window
    {
        public SingleAmountActions(Guid amountId)
        {
            InitializeComponent();

            var viewModelFactory = App.ServiceProvider.GetRequiredService<Func<Guid, SingleAmountActionsViewModel>>();
            DataContext = viewModelFactory(amountId);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var amountsManagementViewModel = App.ServiceProvider.GetService(typeof(AmountsManagementViewModel)) as AmountsManagementViewModel;
            AmountsManagement amountsManagement = new AmountsManagement(amountsManagementViewModel);
            amountsManagement.Show();
            this.Close();
        }
    }
}


