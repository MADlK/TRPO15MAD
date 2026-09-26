using MadTRPO15.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddCategoryPage.xaml
    /// </summary>
    public partial class AddCategoryPage : Page
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MadTrpo15Context db = DbService.Instance.Context;
        private Category _category = new();
        public Category _Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }
        bool isedit = false;
        public AddCategoryPage(Category? edit = null)
        {
            if(edit != null)
            {
                _Category = edit;
                isedit = true;
            }
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(NameTB))
            {
                MessageBox.Show("Если есть ошибки или незаполненные поля то сохранить не получится");
                return;
            }

            if (MessageBox.Show("Вы уверены что хотите сохранить изменения?",
                "Подтверждение редактирования",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {

                return;

            }
            else
            {
                if (isedit)
                {

                    db.SaveChanges();

                }
                else
                {
                    var c = new Category
                    {
                        Name = _Category.Name
                    };
                    db.Add<Category>(c);
                    db.SaveChanges();
                }
                NavigationService.Navigate(new CategoryPage());

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());
        }
    }
}
