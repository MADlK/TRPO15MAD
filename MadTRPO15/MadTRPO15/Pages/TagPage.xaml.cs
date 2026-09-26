using MadTRPO15.Models;
using System;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Windows.Navigation;


namespace MadTRPO15.Pages
{
    /// <summary>
    /// Логика взаимодействия для TagPage.xaml
    /// </summary>
    public partial class TagPage : Page
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _searchQuery;

        public Tag _Selectedtag;
        public Tag Selectedtag
        {
            get => _Selectedtag;
            set
            {
                _Selectedtag = value;
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
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public ICollectionView TagsView { get; set; }
        public TagPage()
        {
            TagsView = CollectionViewSource.GetDefaultView(Tags);
            TagsView.Filter = FilterCatery;
            LoadList();
            InitializeComponent();
        }
        public bool FilterCatery(object obj)
        {
            if (obj is not MadTRPO15.Models.Tag)
                return false;
            var t = (Tag)obj;

            if (searchQuery != null && !t.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }

        public void LoadList()
        {
            var ts = db.Tags
                .ToList();

            Tags.Clear();
            foreach (var t in ts)
                Tags.Add(t);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TagsView.Refresh();
        }

        //пусто
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите сохранить изменения?",
                "Подтверждение редактирования",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            db.Remove<Tag>(Selectedtag);
            db.SaveChanges();
            LoadList();
            TagsView.Refresh();




        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTagsPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Selectedtag == null)
            {
                MessageBox.Show("Вы ничего не выбрали");
                return;
            }
            NavigationService.Navigate(new AddTagsPage(Selectedtag));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandsPage());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
