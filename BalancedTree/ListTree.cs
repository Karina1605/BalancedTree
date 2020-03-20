using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace BalancedTree
{
    public class OneNode<T> where T: IComparable
    {
        
        public TreeNode nodeview;  
        public T info;
        public OneNode<T> left, right;
        private bool IsList { get { return (left == null && right == null); } }
        public OneNode(T info, TreeNode v):this(info)
        {
            nodeview = v;
        }
        public OneNode(T info)
        {
            this.info = info;
            left = right = null;
            nodeview = new TreeNode();
        }
        public OneNode<T> Add(T newNode)
        {
            if (ReturnElem(newNode)==null)
            {
                Queue<OneNode<T>> nodes = new Queue<OneNode<T>>();
                OneNode<T> buf;
                OneNode<T> bufForRight = null;
                nodes.Enqueue(this);
                while (nodes.Count > 0)
                {
                    int c = nodes.Count;
                    for (int i = 0; i < c; ++i)
                    {
                        buf = nodes.Dequeue();
                        if (buf.left == null)
                        {
                            TreeNode NewNode =buf.nodeview.Nodes.Add(newNode.ToString());
                            buf.left = new OneNode<T>(newNode, NewNode);
                            return buf.left;
                        }
                        else
                        {
                            nodes.Enqueue(buf.left);
                            if (buf.right == null && bufForRight == null)
                                bufForRight = buf;
                        }
                        if (buf.right != null)
                            nodes.Enqueue(buf.right);
                    }
                    if (bufForRight!=null)
                    {
                        TreeNode NewNode1 = bufForRight.nodeview.Nodes.Add(newNode.ToString());
                        bufForRight.right = new OneNode<T>(newNode, NewNode1);
                        return bufForRight.right;
                    }
                }
            }
            return null;
        }
        public void ChangeColor (Color color)
        {
            if (nodeview != null)
                nodeview.ForeColor = color;
        }
        public static void delIfLast(ref OneNode<T> Elem,  T node)
        {
            if (Elem!=null)
                if (Elem.IsList)
                {
                    if (Elem.info.CompareTo(node) == 0)
                    {
                        Elem.nodeview.Remove();
                        Elem.nodeview = null;
                        Elem = Elem.left;
                    }
                }
                else
                {
                    delIfLast(ref Elem.left, node);
                    delIfLast(ref Elem.right, node);
                }
        }
        public OneNode<T> ReturnElem (T node)
        {
            if (info.CompareTo(node) == 0)
                return this;
            if (left == null && right == null)
                return null;
            OneNode<T> elL=null;
            OneNode<T> elR = null;
            if (left != null)
                elL = left.ReturnElem(node);
            if (right != null)
                    elR = right.ReturnElem(node);
            if (elR == null && elL == null)
                return null;
            if (elR == null)
                return elL;
            return elR;
        }
        public override string ToString()
        {
            return info.ToString();
        }
        public void Display(TreeNode parent)
        {
            nodeview = new TreeNode();
            nodeview.Text = info.ToString();
            parent.Nodes.Add(nodeview);
            if (left != null)
                left.Display(nodeview);
            if (right != null)
                right.Display(nodeview);
        }
    }
    public class ListTree<T> : ITree<T> where T: IComparable
    {
        OneNode<T> root;
        public OneNode<T> Root { get { return root; } }
        TreeView view;
        Stack<OneNode<T>> OrderNodes;
        int count;
        public int Count { get { return count; }  }
        public bool IsEmpty { get { return count==0; } }
        public ListTree(TreeView tree):base()
        {
            view = tree;
            OrderNodes = new Stack<OneNode<T>>();
        }
        public ListTree()
        {
            view = new TreeView();
            root = null;
            count = 0;
            OrderNodes = new Stack<OneNode<T>>();
        }
        public IEnumerable<T> nodes { get { return this; } }

        public void Add(T node)
        {
            if (root == null)
            {
                TreeNode rootNode = new TreeNode(node.ToString());
                view.Nodes.Add(rootNode);
                root = new OneNode<T>(node, rootNode);
                OrderNodes.Push(root);
                count++;
            }
            else
            {
                OneNode<T> isCreated = root.Add(node);
                if (isCreated != null)
                {
                    OrderNodes.Push(isCreated);
                    count++;
                }
            }
        }

        public void Clear()
        {
            view.Nodes.Clear();
            root = null;
            OrderNodes.Clear();
            count = 0;
        }

        public bool Contains(T node)
        {
            OneNode<T> el = root.ReturnElem(node);
            if (el != null)
            {
                el.ChangeColor(Color.Red);
                MessageBox.Show("Элемент найден");
                el.ChangeColor(Color.Black);
            }
            else
                MessageBox.Show("Такого элемента нет");
            return el!=null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty)
                throw new TryToGetEmptyTree();
            Queue<OneNode<T>> nodesTree= new Queue<OneNode<T>>();
            nodesTree.Enqueue(root);
            while (nodesTree.Count > 0)
            {
                OneNode<T> b = nodesTree.Dequeue();
                yield return b.info;
                if (b.left!=null)
                    nodesTree.Enqueue(b.left);
                if (b.right!=null)
                    nodesTree.Enqueue(b.right);
            }

        }

        public void Remove(T node)
        {
            if (IsEmpty)
                throw new RemovingFromEmptyTree();
            OneNode<T> elem = root.ReturnElem(node);
            if (elem != null)
            {
                OneNode<T> last = OrderNodes.Pop();
                if (!elem.Equals(last))
                {
                    Help.Swap(ref last.info, ref elem.info);
                    elem.nodeview.Text = elem.info.ToString();
                }
                OneNode<T>.delIfLast(ref root, node);
                --count;
            }
        }

        public void DisplayAllTree(TreeView tree)
        {
            root.nodeview = new TreeNode();
            root.nodeview.Text = root.info.ToString();
            tree.Nodes.Add(root.nodeview);
            if (root.left != null)
                root.left.Display(root.nodeview);
            if (root.right != null)
                root.right.Display(root.nodeview);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}