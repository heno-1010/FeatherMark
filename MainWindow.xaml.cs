using ModernWpf.Controls.Primitives;
using SQLite;
using System.Collections.ObjectModel;
using System.IO;
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

namespace FeatherMark
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TreeViewDate> TreeViewDatas { get; } = new ObservableCollection<TreeViewDate>();
        private SQLiteConnection db;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadDate();
            treeview.ItemsSource = TreeViewDatas;
            this.Closing += Window_Closing;
        }
        private void InitializeDatabase()
        {
            var dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TreeViewData.db");
            db = new SQLiteConnection(dbPath);
            db.CreateTable<TreeViewDate>();
        }
        private void SaveDate()
        {
            foreach(var root in TreeViewDatas)
            {
                SaveNode(root);
            }
        }
        private void SaveNode(TreeViewDate node)
        {
            node.IsFolderFlag = node.IsFolder;

            if (node.Id == 0) // 新しいノードの場合
            {
                db.Insert(node); // 新規
            }
            else
            {
                db.Update(node); // 更新
            }

            foreach (var child in node.Children)
            {
                child.ParentId = node.Id;
                SaveNode(child); // 子ノードを保存
            }
        }
        private void LoadDate()
        {
            var rootNodes = db.Table<TreeViewDate>().Where(n => n.ParentId == null).ToList();
            TreeViewDatas.Clear();

            foreach (var root in rootNodes)
            {
                LoadNode(root);  // 再帰的に子ノードをロード
                TreeViewDatas.Add(root); // ルートノードのみ追加
            }
        }

        private void LoadNode(TreeViewDate node)
        {
            node.Children = new ObservableCollection<TreeViewDate>();
            var childNodes = db.Table<TreeViewDate>().Where(n => n.ParentId == node.Id).ToList();
            foreach (var child in childNodes)
            {
                LoadNode(child); // 子ノードも再帰的にロード
                node.Children.Add(child); // 親ノードのChildrenにだけ追加
            }
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
            SaveDate();
        }
        private void TreeView_Addfile()
        {
            if (treeview.SelectedItem is TreeViewDate selectedNode) //親ノードが選択されているか
            {
                TreeViewDate parentNode = FindParentNode(selectedNode);//親ノード取得
                if (parentNode == null) {
                    TreeViewDate fileNode = new TreeViewDate
                    {
                        Name = "ファイルノード"
                    };
                    selectedNode.Children.Add(fileNode);
                }
             }
            SaveDate();
        }
        private void TreeView_Delete()
        {
            if(treeview.SelectedItem is TreeViewDate selectedNode)
            {
                TreeViewDate parentNode = FindParentNode(selectedNode);//親ノード取得
                DeleteNodeFromDatabase(selectedNode);
                if (parentNode != null)
                {
                    parentNode.Children.Remove(selectedNode);
                }
                else
                {
                    TreeViewDatas.Remove(selectedNode);
                }
            }
        }
        private void DeleteNodeFromDatabase(TreeViewDate node)
        {
            foreach(var child in node.Children.ToList()){
                DeleteNodeFromDatabase(child);
            }
            db.Delete(node);
        }
        private TreeViewDate FindParentNode(TreeViewDate node)
        {
            foreach (var parent in TreeViewDatas) {
                if (parent.Children.Contains(node))
                {
                    return parent;
                }
            }
            return null;
        }

        private void Treeview_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            if(treeview.SelectedItem is TreeViewDate selectedNode)
            {
                if (!selectedNode.IsFolder)//ノードがファイルなら表示
                {
                    Content.Text = selectedNode.Content;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDate(); // アプリ終了時にデータを保存
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (treeview.SelectedItem is TreeViewDate selectedNode)
            {
                string newname = Microsoft.VisualBasic.Interaction.InputBox("新しい名前を入力してください:", "名前を変更", selectedNode.Name);
                selectedNode.Name = newname;
                db.Update(selectedNode);
                LoadDate();
            }
        }

        private void Treeview_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = e.OriginalSource as DependencyObject;
            while(treeViewItem != null && !(treeViewItem is TreeViewItem))
            {
                treeViewItem = VisualTreeHelper.GetParent(treeViewItem);
            }
            if (treeViewItem is TreeViewItem item)
            {
                item.IsSelected = true;
            }
        }

        private void Content_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox == null) return;

                // 現在のカーソル位置を取得
                int caretIndex = textBox.CaretIndex;

                // テキストを行ごとに分割
                var lines = textBox.Text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("## "))
                    {
                        // ## を削除してタイトルの書式に変更
                        lines[i] = "タイトル：" + lines[i].Substring(3);
                    }
                }

                // 書式変更後のテキストを再設定
                textBox.Text = string.Join("\n", lines);

                // 元のカーソル位置を復元
                textBox.CaretIndex = caretIndex;

                // Enterキーによる改行を無効化
                e.Handled = true;
            }
        }
        private string ConvertMarkdownToText(string inputText)
        {
            if(inputText.StartsWith("## "))
            {
                return "タイトル：" + inputText.Substring(3);
            }
            return inputText;
        }
    }
 }