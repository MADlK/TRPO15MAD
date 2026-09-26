using MadTRPO15.Models;
using MadTRPO15.Validation;
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
    /// Логика взаимодействия для AddBrandPage.xaml
    /// </summary>
    public partial class AddBrandPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MadTrpo15Context db = DbService.Instance.Context;
        private Brand _brand = new();
        public Brand _Brand
        {
            get => _brand;
            set
            {
                _brand = value;
                OnPropertyChanged();
            }
        } 
        bool isedit = false;

       

        public AddBrandPage(Brand? editbrand =null)
        {
            if(editbrand !=null)
            {
                _brand=editbrand;
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
                if(isedit)
                {
                    
                    db.SaveChanges();
                   
                }
                else
                {
                    var b = new Brand
                    {
                        Name = _brand.Name
                    };
                    db.Add<Brand>(b);
                    db.SaveChanges();
                }
                NavigationService.Navigate(new BrandsPage());

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandsPage());
        }
    }
}
