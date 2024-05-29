using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Supermarket.Helpers;
using Supermarket.Models;

public class AddStockViewModel
{
    public ObservableCollection<Produs> Products { get; set; }
    public Produs SelectedProduct { get; set; }
    public decimal Cantitate { get; set; }
    public string UnitateDeMasura { get; set; }
    public decimal PretAchizitie { get; set; }
    public DateTime? DataExpirarii { get; set; }
    public ICommand AddStockCommand { get; private set; }
    public Action CloseAction { get; set; }

    public AddStockViewModel()
    {
        Products = new ObservableCollection<Produs>(DataService.GetAllProducts());
        AddStockCommand = new RelayCommand(AddStock);
    }

    private void AddStock(object parameter)
    {
        if (SelectedProduct == null)
        {
            MessageBox.Show("Please select a product.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (Cantitate <= 0)
        {
            MessageBox.Show("Quantity must be greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(UnitateDeMasura))
        {
            MessageBox.Show("Unit of measure is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (PretAchizitie <= 0)
        {
            MessageBox.Show("Purchase price must be greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (DataExpirarii.HasValue && DataExpirarii.Value < DateTime.Now)
        {
            MessageBox.Show("Expiration date cannot be in the past.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var newStock = new Stoc
        {
            ProductID = SelectedProduct.ProdusID,
            Cantitate = Cantitate,
            UnitateDeMasura = UnitateDeMasura,
            DataAprovizionarii = DateTime.Now,
            DataExpirarii = DataExpirarii,
            PretAchizitie = PretAchizitie
        };

        try
        {
            DataService.AddStock(newStock);
            CloseAction?.Invoke();
            MessageBox.Show("Stock added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
