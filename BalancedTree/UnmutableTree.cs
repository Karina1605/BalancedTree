using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BalancedTree
{
    public class UnmutableTree<T> : ITree<T> where T: IComparable
    {
        ITree<T> tree;
        public int Count { get { return tree.Count; } }
        public bool IsEmpty { get { return tree.IsEmpty; } }

        public IEnumerable<T> nodes { get { return tree; } }
        public UnmutableTree(ITree<T> ts)
        {
            tree = ts;
        }

        public void Add(T node)
        {
            throw new AttemptOfChangingUnmutableTree();
        }

        public void Clear()
        {
            throw new AttemptOfChangingUnmutableTree();
        }

        public bool Contains(T node)
        {
            return tree.Contains(node);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return tree.GetEnumerator();
        }

        public void Remove(T node)
        {
            throw new AttemptOfChangingUnmutableTree();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tree.GetEnumerator();
        }
        public void DisplayAllTree(TreeView tree)
        {

        }
    }
}
