using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
 
    public class ArrayTree<T> : BaseTree<T> where T : IComparable
    {
        class Elem<T> where T : IComparable
        {
            public T info;
            public int left, right;
            public bool Compare(T node)
            {
                return (info.CompareTo(node) == 0);
            }
            public Elem(T node, int leftindex = -1, int rightindex = -1)
            {
                info = node;
                left = leftindex;
                right = rightindex;
            }
        }
        Elem<T>[] TreeNodes;
        int count;
        int lastPos;
        int LastElem;
        public ArrayTree(int s = 200)
        {
            count = 0;
            lastPos = 0;
            TreeNodes = new Elem<T>[s];
        }
        int findNextEmpty()
        {
            int res = 0;
            while (res < TreeNodes.Length && TreeNodes[res] != null)
                ++res;
            if (res == TreeNodes.Length)
                return -1;
            return res;
        }
        public override void Add(T node)
        {
            if (count == TreeNodes.Length)
                throw new FullTreeException();
            if (count == 0)
            {
                TreeNodes[0] = new Elem<T>(node);
                count++;
                lastPos = findNextEmpty();
                LastElem = 0;
            }
            else
            {
                if (!Contains(node))
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
                                TreeNodes[lastPos] = new Elem<T>(node);
                                lastPos = findNextEmpty();
                                LastElem = buf;
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
                            TreeNodes[lastPos] = new Elem<T>(node);
                            TreeNodes[bufForRight].right = lastPos;
                            LastElem = lastPos;
                            lastPos = findNextEmpty();
                        }
                    }
                    count++;
                }
                
            }
        }

        
        public override void Clear()
        {
            for (int i = 0; i < count; ++i)
                TreeNodes[i] = null;
            count = 0;
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
        public override bool Contains(T node)
        {
            return IsThatElem(node, 0)!=-1;
        }
        public override IEnumerator<T> GetEnumerator()
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
        private int GetNewLast()
        {
            if (TreeNodes[0].left == -1 && TreeNodes[0].right == -1)
                return 0;
            if (TreeNodes[0].left == -1)
                return TreeNodes[0].right;
            //???
            if (TreeNodes[0].right == -1)
                return TreeNodes[0].left;
            int res = 0;
            Queue<int> Left = new Queue<int>();
            Queue<int> Right = new Queue<int>();
            Left.Enqueue(TreeNodes[0].left);
            Right.Enqueue(TreeNodes[0].right);
            int BuffLeft = -1;
            int BuffRight = -1;
            while (Left.Count > 0 || Right.Count > 0)
            {
                int cl = Left.Count;
                int cr = Right.Count;
                BuffLeft = -1;
                BuffRight = -1;
                for (int i = 0; i < cl; ++i)
                {
                    BuffLeft = Left.Dequeue();
                    if (TreeNodes[BuffLeft].left != -1)
                        Left.Enqueue(TreeNodes[BuffLeft].left);
                    if (TreeNodes[BuffLeft].right != -1)
                        Right.Enqueue(TreeNodes[BuffLeft].right);
                }
                for (int i = 0; i < cl; ++i)
                {
                    BuffRight = Left.Dequeue();
                    if (TreeNodes[BuffRight].left != -1)
                        Left.Enqueue(TreeNodes[BuffRight].left);
                    if (TreeNodes[BuffRight].right != -1)
                        Right.Enqueue(TreeNodes[BuffRight].right);
                }
            }
            if (BuffRight == -1)
                return BuffLeft;
            return BuffRight;

            //return res;
        }
            
        
        public override void Remove(T node)
        {
            int res = IsThatElem(node, 0);
            if (res!=-1)
            {
                Help.Swap(ref TreeNodes[res].info, ref TreeNodes[LastElem].info);
                int p = FindParent(LastElem, 0);
                TreeNodes[LastElem] = null;
                if (TreeNodes[p].left == LastElem)
                    TreeNodes[p].left = -1;
                else
                    TreeNodes[p].right = -1;
                LastElem = GetNewLast();
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
        public override int Count { get { return count; } }
        public override bool IsEmpty { get { return count>0; } }
        public override IEnumerable<T> nodes {get { return this; } }
        public static ArrayTree<T> Create() { return new ArrayTree<T>();}
    }
}
