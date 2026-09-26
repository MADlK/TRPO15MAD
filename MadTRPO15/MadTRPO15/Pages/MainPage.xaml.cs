using MadTRPO15.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace MadTRPO15.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public MadTrpo15Context db = DbService.Instance.Context;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Product _sprod;
        public Product SelectedProduct
        {
            get => _sprod;
            set
            {
                _sprod = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Product> Products { get; set; } = new();
        public ICollectionView ProductsView { get; set; }
        private string _searchQuery;
        public string searchQuery { get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
                }
        public string _CostFrom;
        public string CostFrom
        {
            get => _CostFrom;
            set
            {
                _CostFrom = value;
                OnPropertyChanged();
            }
        }
        public string _CostTo;
        public string CostTo { get => _CostTo;
            set 
            {
                _CostTo = value;
                OnPropertyChanged();
            }
                }
        public string SelectionTag { get; set; } = null;

        public void LoadList()
        {
            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Tags)
                .ToList();
              
            Products.Clear();
            foreach (var product in products)
                Products.Add(product);
        }
        public MainPage(bool? admin = false)
        {
           
                ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterProducts;
            LoadList();
            InitializeComponent();
            if (admin == false)
            {
                AdminP.Visibility = Visibility.Collapsed;
                AdminPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                AdminP.Visibility = Visibility.Visible;
                AdminPanel.Visibility = Visibility.Visible;
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductsView.SortDescriptions.Clear();
            var cb = (ComboBox)sender;
            var selected = (ComboBoxItem)cb.SelectedItem;
            if(selected == null)
            {
                ProductsView.Refresh();
                return;
            }
            switch (selected.Tag)
            {
                case "Category":
                    ProductsView.SortDescriptions.Add(new SortDescription("Category.Name",
                    ListSortDirection.Ascending));
                    SelectionTag = "Category";
                    break;
                case "Brand":
                    ProductsView.SortDescriptions.Add(new SortDescription("Brand.Name",
                    ListSortDirection.Ascending));
                    SelectionTag = "Brand";
                    break;
                case "CostAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Price",
                    ListSortDirection.Ascending));
                    SelectionTag = "Cost";
                    break;
                case "CostDesc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Price",
                    ListSortDirection.Descending));
                    SelectionTag = "Cost";
                    break;
                case "StockDesc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Stock",
                    ListSortDirection.Descending));
                    SelectionTag = "Stock";
                    break;
                case "StockAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Stock",
                    ListSortDirection.Ascending));
                    SelectionTag = "Stock";
                    break;
                default:
                    SelectionTag = "";
                    break;
            }
            ProductsView.Refresh();
        }
        public bool FilterProducts(object obj)
        {
            if (obj is not Product)
                return false;
            var product = (Product)obj;
            
            if (searchQuery != null && !product.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;
            if(SelectionTag == "Cost")
            {
                if (!CostFrom.IsNullOrEmpty() && Convert.ToDouble(CostFrom)
                    > product.Price)
                    return false;

                if (!CostTo.IsNullOrEmpty() && Convert.ToDouble(CostTo) <
                product.Price)
                    return false;
            }

            

            return true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(CostFrom))
            {
                CostFrom = CostFrom.Trim().Replace('.', ',');
                if (!double.TryParse(CostFrom, out double pars) || pars < 0)
                {
                    MessageBox.Show("В поле ОТ должно быть положительное число");
                    return;

                }
                CostFrom = pars.ToString();
            }
            else
                return;

            if (!string.IsNullOrWhiteSpace(CostTo))
            {
                CostTo = CostTo.Trim().Replace('.', ',');
                if (!double.TryParse(CostTo, out double parsed) || parsed < 0)
                {
                    MessageBox.Show("В поле ОТ должно быть положительное число");
                    return;

                }
            }
            else
                return;

            if (double.Parse(CostFrom) > double.Parse(CostTo))
            {
                MessageBox.Show("ОТ не может быть больше чем ДО");
                return;
            }
            ProductsView.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CostFrom = "";
            CostTo = "";
            CB.SelectedIndex = -1;
            searchQuery = "";

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandsPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TagPage());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductPage());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Вы ничего не выбрали");
                return;
            }
            NavigationService.Navigate(new AddProductPage(SelectedProduct));
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if(SelectedProduct == null)
            {
                MessageBox.Show("Вы ничего не выбрали");
                return;
            }
            if (MessageBox.Show("Вы уверены что хотите сохранить изменения?",
                "Подтверждение редактирования",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            db.Remove<Product>(SelectedProduct);
            db.SaveChanges();
            LoadList();
            ProductsView.Refresh();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ProductsView.Refresh();
        }
    }
}
