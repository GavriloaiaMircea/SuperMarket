using Supermarket.Helpers;
using Supermarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Supermarket.Views;

public class ProductManagementViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ProdusExtended> _products;
    private ProdusExtended _selectedProduct;

    public ICommand AddProductCommand { get; private set; }
    public ICommand RefreshCommand { get; private set; }
    public ICommand DeleteProductCommand { get; private set; }
    public ICommand SaveChangesCommand { get; private set; }

    public ObservableCollection<ProdusExtended> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged(nameof(Products));
        }
    }

    public ProdusExtended SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged(nameof(SelectedProduct));
        }
    }

    public ProductManagementViewModel()
    {
        LoadProducts();
        AddProductCommand = new RelayCommand(OpenAddProductWindow);
        RefreshCommand = new RelayCommand(RefreshProducts);
        DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
        SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
    }

    private void OpenAddProductWindow(object obj)
    {
        var addProductWindow = new AddProductWindow();
        addProductWindow.ShowDialog();
    }

    private void LoadProducts()
    {
        var products = DataService.GetAllExtendedProducts();
        Products = new ObservableCollection<ProdusExtended>(products);
    }

    private void RefreshProducts(object obj)
    {
        LoadProducts();
    }

    private bool CanDeleteProduct(object obj)
    {
        return SelectedProduct != null;
    }

    private void DeleteProduct(object obj)
    {
        if (SelectedProduct != null)
        {
            try
            {
                DataService.DeleteProduct(SelectedProduct.ProdusID);
                MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshProducts(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private bool CanSaveChanges(object obj)
    {
        return SelectedProduct != null;
    }

    private void SaveChanges(object obj)
    {
        if (SelectedProduct != null)
        {
            try
            {
                DataService.UpdateProduct(SelectedProduct);
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshProducts(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
