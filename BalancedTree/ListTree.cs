using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BalancedTree
{
    public class OneNode<T> where T: IComparable
    {
        public T info;
        public OneNode<T> left, right;
        private bool IsList { get { return (left == null && right == null); } }
        public OneNode(T info)
        {
            this.info = info;
            left = right = null;
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
                            buf.left = new OneNode<T>(newNode);
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
                    bufForRight.right = new OneNode<T>(newNode);
                    return bufForRight.right;
                }
            }
            return null;
        }
        
        public static void delIfLast(ref OneNode<T> Elem,  T node)
        {
            if (Elem!=null)
                if (Elem.IsList)
                {
                    if (Elem.info.CompareTo(node) == 0)
                        Elem = Elem.left;
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
            OneNode<T> el=null;
            if (left != null)
                el = left.ReturnElem(node);
            if (left == null)
                el = right.ReturnElem(node);
            return el;
        }
        public OneNode<T> GetLast()
        {
            if (left == null)
                return this;
            if (right == null)
                return left;
            OneNode<T> res = null;
            Queue<OneNode<T>> Left = new Queue<OneNode<T>>();
            Queue<OneNode<T>> Right = new Queue<OneNode<T>>();
            Left.Enqueue(left);
            Right.Enqueue(right);
            OneNode<T> BuffLeft=null;
            OneNode<T> BuffRight=null;
            while (Left.Count>0 || Right.Count>0)
            {
                int cl = Left.Count;
                int cr = Right.Count;
                BuffLeft = null;
                BuffRight = null;
                for (int i=0; i<cl; ++i)
                {
                    BuffLeft = Left.Dequeue();
                    if (BuffLeft.left != null)
                        Left.Enqueue(BuffLeft.left);
                    if (BuffLeft.right != null)
                        Right.Enqueue(BuffLeft.right);
                }
                for (int i = 0; i < cl; ++i)
                {
                    BuffRight = Left.Dequeue();
                    if (BuffRight.left != null)
                        Left.Enqueue(BuffRight.left);
                    if (BuffRight.right != null)
                        Right.Enqueue(BuffRight.right);
                }    
            }
            if (BuffRight == null)
                return BuffLeft;
            return BuffRight;
        }
       
    }
    public class ListTree<T> : BaseTree<T> where T: IComparable
    {
        OneNode<T> root;
        OneNode<T> LatestElem;
        int count;
        public override int Count { get { return count; }  }
        public override bool IsEmpty { get { return count>0; } }
        public ListTree()
        {
            root = null;
            count = 0;
        }
        public override IEnumerable<T> nodes { get { return this; } }

        public override void Add(T node)
        {
            if (root == null)
            {
                root = new OneNode<T>(node);
                LatestElem = root;
                count++;
            }
            else
            {
                OneNode<T> isCreated = root.Add(node);
                if (isCreated != null)
                {
                    LatestElem = isCreated;
                    count++;
                }
            }
        }

        public override void Clear()
        {
            root = null;
            LatestElem = null;
            count = 0;
        }

        public override bool Contains(T node)
        {
            return root.ReturnElem(node)!=null;
        }

        public override IEnumerator<T> GetEnumerator()
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

        public override void Remove(T node)
        {
            OneNode<T> elem = root.ReturnElem(node);
            if (elem != null)
            {
                if (!elem.Equals(LatestElem))
                    Help.Swap(ref LatestElem.info, ref elem.info);
                OneNode<T>.delIfLast(ref root, node);
                LatestElem = root.GetLast();
                --count;
            }
        }
        public static ListTree<T> Constructor() { return new ListTree<T>(); }
    }
}