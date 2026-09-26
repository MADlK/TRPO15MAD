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
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page, INotifyPropertyChanged
    {
        private MadTrpo15Context db = new MadTrpo15Context();
        public event PropertyChangedEventHandler? PropertyChanged;
        private DateTime _seldate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _seldate;
            set
            {
                _seldate = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Product _p = new();
        public Product Prod
        {
            get => _p;
            set
            {
                _p = value;
                OnPropertyChanged();
            }
        }
        bool isedit = false;
        public AddProductPage(Product? prod = null)
        {
            if(prod != null)
            {
                Prod = prod;
                isedit = true;
            }
            InitializeComponent();
            categorycb.ItemsSource = db.Categories.ToList();
            brandcb.ItemsSource = db.Brands.ToList();
            List.ItemsSource = db.Tags.ToList();

            if(isedit && Prod.Tags != null)
            {
                foreach (var tag in List.Items.OfType<Tag>())
                {
                    if(Prod.Tags.Any(t => t.Id == tag.Id))
                    {
                        List.SelectedItems.Add(tag);
                    }
                }
            }


            if(isedit && !string.IsNullOrEmpty(Prod.CreatedAt))
            {
                if (DateTime.TryParse(Prod.CreatedAt, out DateTime date))
                {
                    DpCreatedDate.SelectedDate = date;
                }
                else
                    DpCreatedDate.SelectedDate = DateTime.Now;
            }

            
            
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(System.Windows.Controls.Validation.GetHasError(NameTB) ||
               System.Windows.Controls.Validation.GetHasError(PriceTB)||
               System.Windows.Controls.Validation.GetHasError(StockTB)||
               System.Windows.Controls.Validation.GetHasError(DpCreatedDate)||
               System.Windows.Controls.Validation.GetHasError(categorycb)||
               System.Windows.Controls.Validation.GetHasError(brandcb))
            {
                MessageBox.Show("Если есть ошибки или незаполненные поля то сохранить не получится");
                return;
            }

            if (categorycb.SelectedItem == null ||
                brandcb.SelectedItem == null ||
                DpCreatedDate.SelectedDate == null)
            {
                MessageBox.Show("Если есть ошибки или незаполненные поля то сохранить не получится");
                return;
            }
                if (isedit)
                {
                    if (MessageBox.Show("Вы уверены что хотите сохранить изменения?",
                "Подтверждение редактирования",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return;
                    }
                        Prod.Tags.Clear();
                    foreach (Tag seltag in List.SelectedItems)
                    {
                        var t = db.Tags.Find(seltag.Id);
                        if (t != null)
                        {
                            Prod.Tags.Add(t);
                        }
                    }
                    db.SaveChanges();
                }
                else
                { 
                    string datestr = SelectedDate.ToString("yyyy-mm-dd") ?? DateTime.Now.ToString("yyyy-mm-dd");
                    var c = new Product
                    {
                        Name = Prod.Name,
                        Price = Prod.Price,
                        CategoryId = Prod.CategoryId,
                        BrandId = Prod.BrandId,
                        CreatedAt = datestr,
                        Rating = 0,
                        Description = Prod.Description,
                        Stock = Prod.Stock,

                        

                    };
                    foreach(Tag seltag in List.SelectedItems)
                    {
                        var t = db.Tags.Find(seltag.Id);
                        if (seltag != null)
                        {
                            c.Tags.Add(t);
                        }
                    }
                    db.Add(c);
                    db.SaveChanges();
                }
                NavigationService.Navigate(new MainPage());
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
