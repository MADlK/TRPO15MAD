using MadTRPO15.Models;
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
    /// Логика взаимодействия для CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _searchQuery;

        public Category _SelectedCategory;
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged();
            }
        }
        

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
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ICollectionView CategoriesView { get; set; }
        public CategoryPage()
        {
            CategoriesView = CollectionViewSource.GetDefaultView(Categories);
            CategoriesView.Filter = FilterCatery;
            LoadList();
            InitializeComponent();
        }

        public bool FilterCatery(object obj)
        {
            if (obj is not Category)
                return false;
            var c = (Category)obj;

            if (searchQuery != null && !c.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }

        public void LoadList()
        {
            var cs = db.Categories
                .ToList();

            Categories.Clear();
            foreach (var c in cs)
                Categories.Add(c);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CategoriesView.Refresh();
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
            db.Remove<Category>(SelectedCategory);
            db.SaveChanges();
            LoadList();
            CategoriesView.Refresh();




        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCategoryPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Вы ничего не выбрали");
                return;
            }
            NavigationService.Navigate(new AddCategoryPage(SelectedCategory));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandsPage());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TagPage());
        }
    }
}
