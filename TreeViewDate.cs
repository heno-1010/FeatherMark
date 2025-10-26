using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatherMark
{
    class TreeViewDate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        [Ignore]
        public ObservableCollection<TreeViewDate> Children { get; set; } = new ObservableCollection<TreeViewDate>();
        public int? ParentId { get; set; }  // 親ノードのID（親がない場合はnull）
        public bool IsFolder => Children.Count > 0;  // フォルダかファイルかの判定
        public bool IsFolderFlag { get; set; } // trueならフォルダ、falseならファイル
        public TreeViewDate()
        {
            Children = new ObservableCollection<TreeViewDate>();  // 初期化
        }
    }
}
