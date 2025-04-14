using ForexFlow.ViewModel.ViewModels;
using System.Windows;

namespace ForexFlow.View
{
    /// <summary>
    /// Interaction logic for InvoiceComposer.xaml
    /// </summary>
    public partial class InvoiceComposer : Window
    {
        private readonly InvoiceManagementViewModel _sharedContext;
        public InvoiceComposer(InvoiceManagementViewModel invoiceManagementViewModel)
        {
            InitializeComponent();
            DataContext = invoiceManagementViewModel;
            _sharedContext = invoiceManagementViewModel;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            InvoicesManagement invoicesManagement = new InvoicesManagement(_sharedContext);
            invoicesManagement.Show();
            this.Close();
        }

        private void OnPreview(object sender, RoutedEventArgs e)
        {
            _sharedContext.PreparePreview();

            InvoicePreview invoicePreview = new InvoicePreview(_sharedContext);
            invoicePreview.Show();
            this.Close();
        }
    }
}
