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
using AutoLotModel1;
namespace Vinkler_Robert_Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
 {
 New,
 Edit,
 Delete,
 Nothing
}
public partial class MainWindow : Window
    {
    ActionState action = ActionState.Nothing;
    AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
    CollectionViewSource customerViewSource;
    public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
    }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
        customerViewSource.Source = ctx.Customers.Local;
        ctx.Customers.Load();
        System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
        action = ActionState.New;
        btnCancel.IsEnabled = true;
        btnSave.IsEnabled = true;
        btnEdit.IsEnabled = false;
        btnDelete.IsEnabled = false;
        btnPrev.IsEnabled = false;
        btnNext.IsEnabled = false;
        BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);

    }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //adaugam directiva:
            // using AutoLotModel;
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                        Contract_value = Decimal.Parse(contract_valueTextBox.Text.Trim()),
                        Contract_date = contract_dateDatePicker.SelectedDate
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    customer.Contract_value = Decimal.Parse(contract_valueTextBox.Text.Trim());
                    customer.Contract_date = contract_dateDatePicker.SelectedDate;
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();

            }
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnNext.IsEnabled = true;
            btnPrev.IsEnabled = true;
            customerDataGrid.IsEnabled = true;
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }
    }
}
