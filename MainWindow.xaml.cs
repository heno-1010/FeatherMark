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
            TreeView_Addfolder();
        }

        private void Addfile_Click(object sender, RoutedEventArgs e)
        {
            TreeView_Addfile();
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            TreeView_Delete();
        }

        private void TreeView_Addfolder()
        {
            TreeViewDate data = new TreeViewDate
            {
                Name = "フォルダノード"
            };
            TreeViewDatas.Add(data);
        }
        private void TreeView_Addfile()
        {
            if (treeview.SelectedItem is TreeViewDate selectedNode) //親ノードが選択されているか
            {
                    TreeViewDate fileNode = new TreeViewDate
                    {
                        Name = "ファイルノード"
                    };
                    selectedNode.Children.Add(fileNode);
             }
         }
        private void TreeView_Delete()
        {

        }
     }
 }