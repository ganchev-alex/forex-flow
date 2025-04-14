using System.Windows;
using ForexFlow.ViewModel.ViewModels;

namespace ForexFlow.View
{
    public partial class InvoicePreview : Window
    {
        private readonly InvoiceManagementViewModel _sharedContext;
        public InvoicePreview(InvoiceManagementViewModel invoiceManagementViewModel)
        {
            InitializeComponent();
            this.DataContext = invoiceManagementViewModel;

            _sharedContext = invoiceManagementViewModel;
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            if(_sharedContext.ActionsPayload.EditingMode)
            {
                InvoiceComposer invoiceComposer = new InvoiceComposer(_sharedContext);
                invoiceComposer.Show();
            }

            this.Close();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            InvoicesManagement invoicesManagement = new InvoicesManagement(_sharedContext);
            invoicesManagement.Show();
            this.Close();
        }
    }
}
