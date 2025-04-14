using ForexFlow.View;
using ForexFlow.ViewModel.ViewModels;
using System.Windows;

namespace ForexFlow;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CurrencyMangement_Click(object sender, RoutedEventArgs e)
    {
        var currencyManagementViewModel = App.ServiceProvider.GetService(typeof(CurrencyManagementViewModel)) as CurrencyManagementViewModel;
        CurrencyManagement currencyMangement = new CurrencyManagement(currencyManagementViewModel);
        currencyMangement.Show();
        this.Hide();
    }

    private void AmountsMangement_Click(object sender, RoutedEventArgs e)
    {
        var amountsManagementViewModel = App.ServiceProvider.GetService(typeof(AmountsManagementViewModel)) as AmountsManagementViewModel;
        AmountsManagement amountsManagement = new AmountsManagement(amountsManagementViewModel);
        amountsManagement.Show();
        this.Hide();
    }

    private void InvoiceManagement_Click(object sender, RoutedEventArgs e)
    {
        var invoiceManagementViewModel = App.ServiceProvider.GetService(typeof(InvoiceManagementViewModel)) as InvoiceManagementViewModel;
        InvoicesManagement invoiceComposer = new InvoicesManagement(invoiceManagementViewModel);
        invoiceComposer.Show();
        this.Hide();
    }
}