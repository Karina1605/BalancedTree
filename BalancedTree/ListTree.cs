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

        //Свойство, указывающее является ли узел листом
        private bool IsList { get { return (left == null && right == null); } }

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
        
        //Добавление в дерево, на вход подается node типа T возвращается созданный узел дерева (обход в ширину)
        public OneNode<T> Add(T newNode)
        {
            //Если нет узла с эквивалентной информационной частью
            if (ReturnElem(newNode) == null)
            {
                //Очередь из узлов по уровням
                Queue<OneNode<T>> nodes = new Queue<OneNode<T>>();
                //Буфер для чтения след узла
                OneNode<T> buf;
                //Буфер для хранения узла с пустым правым поддеревом
                OneNode<T> bufForRight = null;
                nodes.Enqueue(this);
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
                            TreeNode NewNode = buf.nodeview.Nodes.Add(newNode.ToString());
                            buf.left = new OneNode<T>(newNode, NewNode);
                            return buf.left;
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
                        TreeNode NewNode1 = bufForRight.nodeview.Nodes.Add(newNode.ToString());
                        bufForRight.right = new OneNode<T>(newNode, NewNode1);
                        return bufForRight.right;
                    }
                }
            }
            return null;
        }

        //Изменение цвета текста в TreeNode
        public void ChangeColor(Color color)
        {
            if (nodeview != null)
                nodeview.ForeColor = color;
        }

        //Удаление элемента только в том случае, если он - лист с совпадающей информационной частью
        //Elem - узел для проверки, node - с чем сверять
        public static void delIfLast(ref OneNode<T> Elem, T node)
        {
            if (Elem != null)
                //Только если элемент - узел, будем анализировать
                if (Elem.IsList)
                {
                    //Если записи равны удаляем элемент
                    if (Elem.info.CompareTo(node) == 0)
                    {
                        Elem.nodeview.Remove();
                        Elem.nodeview = null;
                        Elem = Elem.left;
                    }
                }
                else
                {
                    //Иначе - идем вниз по дереву
                    delIfLast(ref Elem.left, node);
                    delIfLast(ref Elem.right, node);
                }
        }

        //Функция, осуществляющая поиск элемента и возвращающая узел, его содержащий, если эл-та в дереве нет null
        public OneNode<T> ReturnElem(T node)
        {
            //Если информационная часть и node равны - возвращаем текущий узел
            if (info.CompareTo(node) == 0)
                return this;
            //Если не равны и это лист -null
            if (IsList)
                return null;
            //если поддеревья есть - осуществляем поиск по ним
            OneNode<T> elL = null;
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
        OneNode<T> root;

        //Свойство для чтения корня
        public OneNode<T> Root { get { return root; } }

        //Графическое представление дерева
        TreeView view;

        //Стек, хранящий историю добавления узлов (нужен в удалении)
        Stack<OneNode<T>> OrderNodes;
        /// <summary>
        /// Свойство, хранящее кол-во узлов в дереве в данный момент, только для чтения
        /// </summary>
        public int Count { get {return OrderNodes.Count; }  }
        /// <summary>
        /// Свойство, предоставляющее информацию о том, пусто дерево или нет, только для чтения
        /// </summary>
        public bool IsEmpty { get { return OrderNodes.Count==0; } }
        /// <summary>
        /// Свойство, возвращающее объект, поддерживаемый для foreach
        /// </summary>
        public IEnumerable<T> nodes { get { return this; } }


        //Конструктор без обязательных параметров
        public ListTree(TreeView tree=null)
        {
            root = null;
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
            if (root == null)
            {
                TreeNode rootNode = new TreeNode(node.ToString());
                view.Nodes.Add(rootNode);
                root = new OneNode<T>(node, rootNode);
                OrderNodes.Push(root);
            }
            else
            {
                //Иначе пытаемся добавить новый элемент в дерево - если он уникальный (добавить получилось) - добавляем в стек
                OneNode<T> isCreated = root.Add(node);
                if (isCreated != null)
                    OrderNodes.Push(isCreated);
            }
        }

        /// <summary>
        /// Очистка цепочного дерева
        /// </summary>
        public void Clear()
        {
            view.Nodes.Clear();
            root = null;
            OrderNodes.Clear();
        }

        /// <summary>
        /// Поиск элемента в дереве
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Contains(T node)
        {
            //Результат функции - равен ли returnElem null
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

        /// <summary>
        /// Реализация IEnumerable (обход дерева в ширину (Если дерево пусто - исключение))
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Удаление элемента из дерева, если дерево пусто  - исключение
        /// </summary>
        /// <param name="node"></param>
        public void Remove(T node)
        {
            if (IsEmpty)
                throw new RemovingFromEmptyTree();
            OneNode<T> elem = root.ReturnElem(node);
            //Если удаляемый элемент найден- извлекаем последний эл-т из стека, меняем местами значения 
            //и удаляем последний эл-т из дерева с помощью delIfLast
            if (elem != null)
            {
                OneNode<T> last = OrderNodes.Pop();
                if (!elem.Equals(last))
                {
                    Help.Swap(ref last.info, ref elem.info);
                    elem.nodeview.Text = elem.info.ToString();
                }
                OneNode<T>.delIfLast(ref root, node);
            }
        }

        /// <summary>
        /// Отображение дерева на TreeView; параметр - TreeView, на которое отоборажать
        /// </summary>
        /// <param name="tree"></param>
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