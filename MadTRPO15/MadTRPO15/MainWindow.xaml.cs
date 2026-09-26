using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MadTRPO15.Pages;

namespace MadTRPO15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if(e.Content is Page currentPage && !string.IsNullOrEmpty(currentPage.Title))
            {
                this.Title = $"EShop - {currentPage.Title}";
            }
            else
            {
                this.Title = $"EShop";
            }
        }
    }
}