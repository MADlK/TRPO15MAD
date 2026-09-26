using MadTRPO15.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для BrandsPage.xaml
    /// </summary>
    public partial class BrandsPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _searchQuery;

        public Brand _Selectedbrand;
        public Brand Selectedbrand
        {
            get => _Selectedbrand;
            set
            {
                _Selectedbrand = value;
                OnPropertyChanged();
            }
        }
        private Brand _edit;
        
        public string searchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }
        public MadTrpo15Context db = DbService.Instance.Context;
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ICollectionView BrandsView { get; set; }
        public BrandsPage()
        {
            BrandsView = CollectionViewSource.GetDefaultView(Brands);
            BrandsView.Filter = FilterProducts;
            LoadList();
            InitializeComponent();
        }
        public bool FilterProducts(object obj)
        {
            if (obj is not Brand)
                return false;
            var brand = (Brand)obj;

            if (searchQuery != null && !brand.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;
           
            return true;
        }

        public void LoadList()
        {
            var brands = db.Brands
               
                .ToList();

            Brands.Clear();
            foreach (var brand in brands)
                Brands.Add(brand);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BrandsView.Refresh();
        }

        //пусто
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //пустая кнопка
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите сохранить изменения?",
                "Подтверждение редактирования",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {

                return;

            }
            db.Remove<Brand>(Selectedbrand);
            db.SaveChanges();
            LoadList();
            BrandsView.Refresh();




        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddBrandPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Selectedbrand == null)
            {
                MessageBox.Show("Вы ничего не выбрали");
                return;
            }
            NavigationService.Navigate(new AddBrandPage(Selectedbrand));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TagPage());

        }
    }
}
