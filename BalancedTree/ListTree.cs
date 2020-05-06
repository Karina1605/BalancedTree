using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;


//Разработать библиотеку для работы со сбалансированным деревом
namespace BalancedTree
{
    /// <summary>
    /// Вспомогательный класс, описывающий узел дерева
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OneNode<T> where T : IComparable
    {

        //Информационная часть узла
        public T info;

        //Графическое представление узла
        public TreeNode nodeview;

        //Сслыки на левое и правое поддеревья
        public OneNode<T> left, right;


        //Конструктор с 1 обязательным параметром - информационной частью
        public OneNode(T info, TreeNode v=null)
        {
            this.info = info;
            left = right = null;
            if (v!=null)
                nodeview = v;
            else
                nodeview = new TreeNode();
        }
        
        
       
        //Перегрузка ьазового ToString
        public override string ToString()
        {
            return info.ToString();
        }

        //Отображение узла дерева, параметр  - родительский узел (рекурсивно)
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
    /// <summary>
    /// Цепочное представление дерева
    /// Шаблонный класс сбалансированного дерева, поддерживающий интефейс ITree, параметр T должен поддерживать IComparable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListTree<T> : ITree<T> where T: IComparable
    {
        //Корень дерева
        public OneNode<T> Root { get; private set; }

        //Графическое представление дерева
        TreeView view;
        //Хранит элемент наденный последней вызываемой функцией Contains (для сокращения времени удаления)
        OneNode<T> finded;

        //Стек, хранящий историю добавления узлов (нужен в удалении)
        Stack<OneNode<T>> OrderNodes;
        /// <summary>
        /// Свойство, хранящее кол-во узлов в дереве в данный момент, только для чтения
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Свойство, предоставляющее информацию о том, пусто дерево или нет, только для чтения
        /// </summary>
        public bool IsEmpty { get { return Root==null; } }
        /// <summary>
        /// Свойство, возвращающее объект, поддерживаемый для foreach
        /// </summary>
        public IEnumerable<T> nodes { get { return this; } }


        //Конструктор без обязательных параметров
        public ListTree(TreeView tree=null)
        {
            Root = null;
            finded = null;
            Count = 0;
            OrderNodes = new Stack<OneNode<T>>();
            if (tree != null)
                view = tree;
            else
                view = new TreeView();
        }
        
       
        /// <summary>
        /// Добавление нового уникального элемента в дерево
        /// </summary>
        /// <param name="node"></param>
        public void Add(T node)
        {
            //если дерево пусто - просто добавляем в корень
            if (Root == null)
            {
                TreeNode rootNode = new TreeNode(node.ToString());
                view.Nodes.Add(rootNode);
                Root = new OneNode<T>(node, rootNode);
            }
            else
            {
                if (!Contains(node))
                {
                    //Очередь из узлов по уровням
                    Queue<OneNode<T>> nodes = new Queue<OneNode<T>>();
                    //Буфер для чтения след узла
                    OneNode<T> buf;
                    //Буфер для хранения узла с пустым правым поддеревом
                    OneNode<T> bufForRight = null;
                    nodes.Enqueue(Root);
                    while (nodes.Count > 0)
                    {
                        //Определяем кол-во узлов на уровне
                        int c = nodes.Count;
                        //Проход по уровню
                        for (int i = 0; i < c; ++i)
                        {
                            buf = nodes.Dequeue();
                            //если у узла левое поддерево пусто, то добавляем его и возвращаем из функции
                            if (buf.left == null)
                            {
                                TreeNode NewNode = buf.nodeview.Nodes.Add(node.ToString());
                                buf.left = new OneNode<T>(node, NewNode);
                                OrderNodes.Push(buf);
                                ++Count;
                                return;
                            }
                            else
                            {
                                //Если нет, то кладем в очередь и анализируем правое поддерево
                                nodes.Enqueue(buf.left);
                                //Если таких узлов еще не встречалось, а это первый - запоминаем его
                                if (buf.right == null && bufForRight == null)
                                    bufForRight = buf;
                            }
                            //Иначе -также кладем в очередь
                            if (buf.right != null)
                                nodes.Enqueue(buf.right);
                        }
                        //Ессли нашли узел с пустым правым поддеревом - то создаем в правом поддереве новый узел и возвращаем его
                        if (bufForRight != null)
                        {
                            TreeNode NewNode1 = bufForRight.nodeview.Nodes.Add(node.ToString());
                            bufForRight.right = new OneNode<T>(node, NewNode1);
                            OrderNodes.Push(bufForRight);
                            ++Count;
                            return;
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// Очистка цепочного дерева
        /// </summary>
        public void Clear()
        {
            view.Nodes.Clear();
            Count = 0;
            Root = null;
            OrderNodes.Clear();
        }

        /// <summary>
        /// Поиск элемента в дереве
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// В классе определен параметр finded для сокращения времени при удалении
        public bool Contains(T node)
        {
            if (Root == null)
                return false;
            OneNode<T> el = Root;
            Queue<OneNode<T>> elems = new Queue<OneNode<T>>();
            elems.Enqueue(el);
            while (elems.Count>0)
            {
                el = elems.Dequeue();
                if (el.info.CompareTo(node) == 0)
                {
                    finded = el;
                    return true;
                }
                    
                if (el.left != null)
                    elems.Enqueue(el.left);
                if (el.right != null)
                    elems.Enqueue(el.right);
            }
            finded = null;
            return false;
            
        }

        /// <summary>
        /// Реализация IEnumerable (обход дерева в ширину (Если дерево пусто - исключение))
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {

            if (IsEmpty)
                throw new TryToGetEmptyTree();
            Queue<OneNode<T>> nodesTree= new Queue<OneNode<T>>();
            nodesTree.Enqueue(Root);
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

        /// <summary>
        /// Удаление элемента из дерева, если дерево пусто  - исключение
        /// </summary>
        /// <param name="node"></param>
        public void Remove(T node)
        {
            if (IsEmpty)
                throw new RemovingFromEmptyTree();
            if (Contains(node))
            {
                if (OrderNodes.Count == 0)
                    Clear();
                else
                {
                    OneNode<T> inf = OrderNodes.Peek().right == null ? OrderNodes.Peek().left : OrderNodes.Peek().right;
                    finded.info = inf.info;
                    finded.nodeview.Text = finded.info.ToString();
                    OneNode<T> peeked = OrderNodes.Pop();
                    if (peeked.right == null)
                    {
                        peeked.left.nodeview.Remove();
                        peeked.left = null;
                    }
                    else
                    {
                        peeked.right.nodeview.Remove();
                        peeked.right = null;
                    }
                    --Count;
                }
                
            }
        }

        /// <summary>
        /// Отображение дерева на TreeView; параметр - TreeView, на которое отоборажать
        /// </summary>
        /// <param name="tree"></param>
        public void DisplayAllTree(TreeView tree)
        {
            Root.nodeview = new TreeNode();
            Root.nodeview.Text = Root.info.ToString();
            tree.Nodes.Add(Root.nodeview);
            if (Root.left != null)
                Root.left.Display(Root.nodeview);
            if (Root.right != null)
                Root.right.Display(Root.nodeview);
        }

        /// <summary>
        /// Поддержка IEnumerable
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}