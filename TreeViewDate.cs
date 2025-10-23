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
        public string Name { get; set; }
        public ObservableCollection<TreeViewDate> Children { get; set; } = new ObservableCollection<TreeViewDate>();
        public TreeViewDate Parent { get; set; } // 親ノードを参照
        public static object Nodes { get; internal set; }

        public TreeViewDate() { 
            Children = new ObservableCollection<TreeViewDate>();
        }
    }
}
