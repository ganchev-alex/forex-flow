using ForexFlow.ViewModel.ViewModels;
using System.Windows;
namespace ForexFlow.View
{
    /// <summary>
    /// Interaction logic for InvoicesManagement.xaml
    /// </summary>
    public partial class InvoicesManagement : Window
    {
        private readonly InvoiceManagementViewModel _sharedContext;
        public InvoicesManagement(InvoiceManagementViewModel invoiceManagementViewModel)
        {
            InitializeComponent();
            DataContext = invoiceManagementViewModel;

            _sharedContext = invoiceManagementViewModel;
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            MainWindow homeMenu = new MainWindow();
            homeMenu.Show();
            this.Hide();
        }

        private void OnComposeInvoice(object sender, RoutedEventArgs e)
        {
            _sharedContext.InvoiceComposerCleanUp();
            InvoiceComposer composeInvoce = new InvoiceComposer(_sharedContext);
            composeInvoce.Show();
            this.Hide();
        }

        private void OnPreviewInvoice(object sender, RoutedEventArgs e)
        {
            InvoicePreview invoicePreview = new InvoicePreview(_sharedContext);
            invoicePreview.Show();
        }
    }
}
