using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    public delegate bool CheckDelegate<T>(T node);
    public delegate void ActionDelegate<T>(T node);
    public delegate ITree<T> TreeConstructor<T>();
    public static class TreeUtils
    {
        public static bool Excists<T>(ITree<T> tree, CheckDelegate<T> check)
        {
            foreach (T node in tree)
            {
                if (check(node))
                    return true;
            }
            return false ;
        }
        public static void ForEach<T>(ITree<T> tree, ActionDelegate<T> action)
        {
            foreach (T node in tree)
                action(node);
        }
        public static bool CheckForAll<T>(ITree<T> tree, CheckDelegate<T> check)
        {
            foreach (T node in tree)
                if (!check(node))
                    return false;
            return true;
        }
        public static ITree<T> FindAll<T> (ITree<T> tree, CheckDelegate<T> check, TreeConstructor<T> constructor)
        {
            ITree<T> res = constructor();
            foreach (T ob in tree)
                if (check(ob))
                    res.Add(ob);
            return res;
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
