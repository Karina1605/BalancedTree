using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    public delegate bool CheckDelegate<T>(T node);
    public delegate T ActionDelegate<T>(T node);
    public delegate ITree<T> TreeConstructor<T>();
    public static class TreeUtils<T> where T:IComparable
    {
        public static bool Excists(ITree<T> tree, CheckDelegate<T> check)
        {
            foreach (T node in tree)
            {
                if (check(node))
                    return true;
            }
            return false ;
        }
        public static void ForEach(ITree<T> tree, ActionDelegate<T> action)
        {
            if (tree is UnmutableTree<T>)
                throw new AttemptOfChangingUnmutableTree();
            if (tree is ArrayTree<T>)
            {
                ArrayTree<T> t = (ArrayTree<T>)tree;
                for (int i=0; i<tree.Count; ++i)
                    t[i].info = action(t[i].info);
            }
            else
            {
                OneNode<T> one = ((ListTree<T>)tree).Root;
                ForEachList(one, action);
            }
        }
        private static void ForEachList (OneNode<T> node, ActionDelegate<T> action)
        {
            if (node!=null)
            {
                node.info = action(node.info);
                ForEachList(node.left, action);
                ForEachList(node.right, action);
            }       
        }
        public static bool CheckForAll(ITree<T> tree, CheckDelegate<T> check)
        {
            foreach (T node in tree)
                if (!check(node))
                    return false;
            return true;
        }
        public static ITree<T> FindAll(ITree<T> tree, CheckDelegate<T> check, TreeConstructor<T> constructor)
        {
            ITree<T> res = constructor();
            foreach (T ob in tree)
                if (check(ob))
                    res.Add(ob);
            return res;
        } 
        public static ITree<T> ListConstuctorDelegate() { return new ListTree<T>(); }
        public static ITree<T> ArrayConstructorDelegate() { return new ArrayTree<T>(); }
        public static  int Action (int node)
        {
            return node * 2;
        }
        public static bool Check (int node)
        {
            return node % 3 == 0;
        }
        public static bool CheckForNew (int node)
        {
            return node % 2 == 0;
        }
    }
    public static class Help
    {
        public static void Swap<T> (ref T o1, ref T o2)
        {
            T b = o1;
            o1 = o2;
            o2 = b;
        }
    }
}
