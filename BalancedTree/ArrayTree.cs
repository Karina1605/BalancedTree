using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BalancedTree
{
 
    public class ArrayTree<T> : ITree<T> where T : IComparable
    {
        public class Elem<T> where T : IComparable
        {
            public T info;
            public int left, right;
            public TreeNode viewNode { get; set; }
            public bool Compare(T node)
            {
                return (info.CompareTo(node) == 0);
            }
            public Elem(T node, TreeNode nodev =null, int leftindex = -1, int rightindex = -1)
            {
                info = node;
                left = leftindex;
                right = rightindex;
                if (nodev == null)
                    viewNode = new TreeNode();
                else
                    viewNode = nodev;
            }
            public override string ToString()
            {
                return info.ToString();
            }

        }
        Elem<T>[] TreeNodes;
        Stack<int> orderNodes;
        int lastPos;
        TreeView view;
        public Elem<T> this[int i] { get { return TreeNodes[i]; } }
        public ArrayTree (int s=200)
        {
            lastPos = 0;
            TreeNodes = new Elem<T>[s];
            view = new TreeView();
            orderNodes = new Stack<int>();
        }
        public ArrayTree(TreeView tree, int s = 200): this()
        {
            view = tree;
        }
        public void Add(T node)
        {
            if (orderNodes.Count == TreeNodes.Length)
                throw new FullTreeException();
            if (orderNodes.Count == 0)
            {              
                TreeNode treeNode = new TreeNode(node.ToString());
                view.Nodes.Add(treeNode);
                TreeNodes[0] = new Elem<T>(node, treeNode);
                lastPos = orderNodes.Count+1;
                orderNodes.Push(0);
            }
            else
            {
                if (IsThatElem(node, 0)==-1)
                {
                    Queue<int> nodes = new Queue<int>();
                    int buf;
                    int bufForRight = -1;
                    nodes.Enqueue(0);
                    bool isadded = false;
                    while (nodes.Count > 0 && !isadded)
                    {
                        int c = nodes.Count;
                        for (int i = 0; i < c && !isadded; ++i)
                        {
                            buf = nodes.Dequeue();
                            if (TreeNodes[buf].left == -1)
                            {
                                TreeNodes[buf].left = lastPos;
                                TreeNode treeNode = new TreeNode(node.ToString());
                                TreeNodes[buf].viewNode.Nodes.Add(treeNode);
                                TreeNodes[lastPos] = new Elem<T>(node, treeNode);
                                orderNodes.Push(lastPos);
                                lastPos = orderNodes.Count + 1;
                                isadded = true;
                            }
                            else
                            {
                                nodes.Enqueue(TreeNodes[buf].left);
                                if (TreeNodes[buf].right == -1 && bufForRight == -1)
                                    bufForRight = buf;
                            }
                            if (TreeNodes[buf].right != -1)
                                nodes.Enqueue(TreeNodes[buf].right);
                        }
                        if (!isadded && bufForRight != -1)
                        {   
                            TreeNode treeNode = new TreeNode(node.ToString());
                            TreeNodes[lastPos] = new Elem<T>(node, treeNode);
                            TreeNodes[bufForRight].viewNode.Nodes.Add(treeNode);                           
                            TreeNodes[bufForRight].right = lastPos;
                            orderNodes.Push(lastPos);
                            lastPos = orderNodes.Count+1;
                            isadded = true;
                        }
                    }
                }
            }
        }

        
        public void Clear()
        {
           view.Nodes.Clear();
            for (int i = 0; i < orderNodes.Count; ++i)
                TreeNodes[i] = null;
            lastPos = 0;
            orderNodes.Clear();
        }
        private int IsThatElem(T node, int index)
        {
            if (index == -1 || index >= TreeNodes.Length)
                   return -1;
            if (TreeNodes[index].info.CompareTo(node) == 0)
                return index;
            int l = IsThatElem(node, TreeNodes[index].left);
            if (l == -1)
                return IsThatElem(node, TreeNodes[index].right);
            return l;
        }
        public bool Contains(T node)
        {
            int pos = IsThatElem(node, 0);
            if (pos != -1)
            {
                if (TreeNodes[pos].viewNode != null)
                {
                    TreeNodes[pos].viewNode.ForeColor = Color.Red;
                    MessageBox.Show("Элемент найден");
                    TreeNodes[pos].viewNode.ForeColor = Color.Black;
                }
            }
            else
                MessageBox.Show("Нет такого элемента");
            return pos!=-1;
        }
        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty)
                throw new TryToGetEmptyTree();
            Queue<int> indexes = new Queue<int>();
            indexes.Enqueue(0);
            while (indexes.Count > 0)
            {
                int b = indexes.Dequeue();
                yield return TreeNodes[b].info;
                if (TreeNodes[b].left != -1)
                    indexes.Enqueue(TreeNodes[b].left);
                if (TreeNodes[b].right != -1)
                    indexes.Enqueue(TreeNodes[b].right);
            }

        }
        
            
        
        public void Remove(T node)
        {
            if (IsEmpty)
                throw new RemovingFromEmptyTree();
            int res = IsThatElem(node, 0);
            if (res!=-1)
            {
                int last = orderNodes.Pop();
                if (res !=last)
                {
                    TreeNodes[res].info= TreeNodes[last].info;
                }
                TreeNodes[res].viewNode.Text = TreeNodes[res].info.ToString();
                int p = FindParent(last, 0);
                TreeNodes[last].viewNode.Remove();
                TreeNodes[last] = null;
                if (p!=-1)
                    if (TreeNodes[p].left == last)
                        TreeNodes[p].left = -1;
                    else
                        TreeNodes[p].right = -1;
            }
        }
        private int FindParent(int Child, int pos)
        {
            if (pos==-1 || (TreeNodes[pos].left == -1 && TreeNodes[pos].right == -1))
                return -1;
            if (TreeNodes[pos].left == Child || TreeNodes[pos].right == Child)
                return pos;
            int l = FindParent(Child, TreeNodes[pos].left);
            if (l == -1)
                return FindParent(Child, TreeNodes[pos].right);
            return l;
            
        }
        public int Count { get { return orderNodes.Count; } }
        public bool IsEmpty { get { return orderNodes.Count==0; } }
        public IEnumerable<T> nodes {get { return this; } }
        public void DisplayAllTree(TreeView tree)
        {
            TreeNodes[0].viewNode = new TreeNode();
            TreeNodes[0].viewNode.Text = TreeNodes[0].info.ToString();
            tree.Nodes.Add(TreeNodes[0].viewNode);
            if (TreeNodes[0].left != -1)
                DisplayOneNode(TreeNodes[0].left, TreeNodes[0].viewNode);
            if (TreeNodes[0].right != -1)
                DisplayOneNode(TreeNodes[0].right, TreeNodes[0].viewNode);
        }
        private void DisplayOneNode(int pos, TreeNode parent)
        {
            TreeNodes[pos].viewNode = new TreeNode();
            TreeNodes[pos].viewNode.Text = TreeNodes[pos].info.ToString();
            parent.Nodes.Add(TreeNodes[pos].viewNode);
            if (TreeNodes[pos].left != -1)
                DisplayOneNode(TreeNodes[pos].left, TreeNodes[pos].viewNode);
            if (TreeNodes[pos].right!=-1)
                DisplayOneNode(TreeNodes[pos].right, TreeNodes[pos].viewNode);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
