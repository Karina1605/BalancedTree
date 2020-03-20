using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    /// <summary>
    /// Делегат шаблонной функции, проверяющий параметр на соответсвие условию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public delegate bool CheckDelegate<T>(T node);

    /// <summary>
    /// Делегат шаблонной функции, совершающий преобразования над параметром типа T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public delegate T ActionDelegate<T>(T node);

    /// <summary>
    /// Делегат-конструктор, возвращает объект класса, поддерживающий ITree<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public delegate ITree<T> TreeConstructor<T>();

    /// <summary>
    /// Статический класс утилит для работы со сбаланчированным деревом
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class TreeUtils<T> where T:IComparable
    {
        /// <summary>
        /// Проверка, существует ли в дереве узел, соответсвующий условию из check 
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool Excists(ITree<T> tree, CheckDelegate<T> check)
        {
            foreach (T node in tree)
            {
                if (check(node))
                    return true;
            }
            return false ;
        }
        /// <summary>
        /// Перебор и преобразование всех эл-тов дерева
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="action"></param>
        public static void ForEach(ITree<T> tree, ActionDelegate<T> action)
        {
            //Если дерево неизменяемо - исключение
            if (tree is UnmutableTree<T>)
                throw new AttemptOfChangingUnmutableTree();
            //Специальная форма для кажого представления
            if (tree is ArrayTree<T>)
            {
                ArrayTree<T> t = (ArrayTree<T>)tree;
                for (int i=0; i<tree.Count; ++i)
                    if (t[i]!=null)
                        t[i].info = action(t[i].info);
            }
            else
            {
                OneNode<T> one = ((ListTree<T>)tree).Root;
                ForEachList(one, action);
            }
        }
        //Специальная функция, на случай, если дерево представлено цепочно
        private static void ForEachList (OneNode<T> node, ActionDelegate<T> action)
        {
            if (node!=null)
            {
                node.info = action(node.info);
                ForEachList(node.left, action);
                ForEachList(node.right, action);
            }       
        }
        /// <summary>
        /// Проверить, что все эл-ты дерева подчинены условию check
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool CheckForAll(ITree<T> tree, CheckDelegate<T> check)
        {
            foreach (T node in tree)
                if (!check(node))
                    return false;
            return true;
        }
        /// <summary>
        /// Поиск всех эл-тов в дереве по заданному условию
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="check"></param>
        /// <param name="constructor"></param>
        /// <returns></returns>
        public static ITree<T> FindAll(ITree<T> tree, CheckDelegate<T> check, TreeConstructor<T> constructor)
        {
            ITree<T> res = constructor();
            foreach (T ob in tree)
                if (check(ob))
                    res.Add(ob);
            return res;
        } 
        /// <summary>
        /// Конструктор цепочного дерева
        /// </summary>
        /// <returns></returns>
        public static ITree<T> ListConstuctorDelegate() { return new ListTree<T>(); }
        /// <summary>
        /// Конструктор сплошного дерева
        /// </summary>
        /// <returns></returns>
        public static ITree<T> ArrayConstructorDelegate() { return new ArrayTree<T>(); }
        //Тестовые функции
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
    //Вспомогательный класс с одним методо, меняющим местами 2 аргумента
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
