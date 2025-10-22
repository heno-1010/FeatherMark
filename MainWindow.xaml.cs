using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace FeatherMark
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TreeViewDate> TreeViewDatas { get; } = new ObservableCollection<TreeViewDate>();

        public MainWindow()
        {
            InitializeComponent();
            treeview.ItemsSource = TreeViewDatas;
        }

        private void Addfolder_Click(object sender, RoutedEventArgs e)
        {
            TreeView_Add();
        }

        private void Addfile_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TreeView_Add()
        {
            TreeViewDate data = new TreeViewDate
            {
                Name = "フォルダノード"
            };
            TreeViewDatas.Add(data);
        }
    }
}