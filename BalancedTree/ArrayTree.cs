using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


//Разработать библиотеку для работы со сбалансированным деревом
namespace BalancedTree
{
    /// <summary>
    /// Сплошное представление дерева
    /// Шаблонный класс сбалансированного дерева, поддерживающий интефейс ITree, параметр T должен поддерживать IComparable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayTree<T> : ITree<T> where T : IComparable
    {
        //Вспомогательный шаблонный класс, описывающий ячейку массива при сплошной реализации, тип T также IComparable
        public class Elem<T> where T : IComparable
        {
            //Информационная часть узла
            public T info;

            //Указатели, содержащие индексы левогои правого потомка в массиве
            public int left, right;

            //Свойство - графическое представление узла
            public TreeNode viewNode { get; set; }

            //Оболочка метода Compare (на вход поступает информация типа T и сранвнивается с полем info узла, 
            //если сравнение дало 0 - возвращается true, иначе - false)
            public bool Compare(T node)
            {
                return (info.CompareTo(node) == 0);
            }

            //Конструктор узла (обязательный параметр только информационная часть, признак конца иерархии в данной реализации - 
            // если пол-указатель на потомка ==-1)
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

            //Перегруженный метод ToString класса Object, возвращет строковое представление информационной части узла
            public override string ToString()
            {
                return info.ToString();
            }
        }

        //Поле, содержащее массив узлов (само дерево)
        Elem<T>[] TreeNodes;

        //Стек для хранения порядка родителей добавления узлов (история добалений в дерево (нужен при удалении узла))
        Stack<int> orderNodes;

        //Первый свободный индекс массива
        int FirstPos;

        //Графическое отображение дерева
        TreeView view;

        //Конструктор с параметрами по умолч (tree - графическое представление, s - размер массива, в котором будет храниться дерево)
        public ArrayTree(TreeView tree = null, int s = 200)
        {
            FirstPos = 0;
            TreeNodes = new Elem<T>[s];
            if (tree == null)
                view = new TreeView();
            else
                view = tree;
            orderNodes = new Stack<int>();
        }

        //Функция добавления, на вход поступет информация, если таковой нет в дереве, то она записывается
        //Алгоитм добавления описан в модуле ITree, при заполненном массиве выбрасывается исключение
        public void Add(T node)
        {
            //Если вставлять некуда - исключние
            if (orderNodes.Count == TreeNodes.Length)
                throw new FullTreeException();
            //Если дерево пусто - просто добавляем на 1-ую позицию с индексом 0, следующее свободное поле имеет индекс 1
            if (FirstPos == 0)
            {
                //Инициализация графического представления
                TreeNode treeNode = new TreeNode(node.ToString());
                //Добавление узла на TreeView
                view.Nodes.Add(treeNode);
                //Заполенение ячейки (индексы левого и првого потомка равны -1 по умолч)
                TreeNodes[0] = new Elem<T>(node, treeNode);
                //Следующая свободная позиция в массива
                FirstPos = 1;
            }
            //Если дерево не пусто
            else
            {
                //Проверяем, что узла с таким значением не в дереве (IsThatElem возвращает индекс узла, у которого ифнормационная часть совпала с параметром)
                if (IsThatElem(node, 0) == -1)
                {
                    //Инициализация очереди для обхода в ширину
                    Queue<int> nodes = new Queue<int>();
                    //Буфер для хранения промежуточных значений, хранящихся в очереди
                    int buf;
                    //Буфер для хранения элемента с пустым правым поддеревом
                    int bufForRight = -1;
                    //Добавление первого элемента в очередь
                    nodes.Enqueue(0);
                    //Логическая переменная для цикла
                    bool isadded = false;
                    //Продолжать поиск, пока элемент не добавлен
                    while (!isadded)
                    {
                        //Переменная, хранящая кол-во узлов на текущем уровне
                        int c = nodes.Count;
                        //Цикл будет выполняться пока не обойдем все узлы текущего уровня или не добавим элемент
                        for (int i = 0; i < c && !isadded; ++i)
                        {
                            //Достаем первый в очереди узел
                            buf = nodes.Dequeue();
                            //Если левое поддерево пусто - добавляем
                            if (TreeNodes[buf].left == -1)
                            {
                                //Теперь идекс левого узла - первая свободная ячейка (в нее и запишется новый узел)
                                TreeNodes[buf].left = FirstPos;
                                //Инициализация графики (раньше, чтобы добавить потомка родительсеому узлу и этот же узел передать в конструктор)
                                TreeNode treeNode = new TreeNode(node.ToString());
                                //Добавление потомка узлу-родителю
                                TreeNodes[buf].viewNode.Nodes.Add(treeNode);
                                //Запись элемента в первую свободную ячейку
                                TreeNodes[FirstPos] = new Elem<T>(node, treeNode);
                                //Добавление индекса в стек
                                orderNodes.Push(buf);
                                //Передвигаем на след свободную ячейку
                                FirstPos++;
                                //Завершаем цикл
                                isadded = true;
                                return;
                            }
                            //Левое поддерево не пусто
                            else
                            {
                                //Добавляем левый индекс в очередь
                                nodes.Enqueue(TreeNodes[buf].left);
                                //Если пусто правое поддерево и не найдена певая позиция эл-та с пустым правым поддеревом - инициализируем переменную
                                if (TreeNodes[buf].right == -1 && bufForRight == -1)
                                    bufForRight = buf;
                            }
                            //если все же и правое поддерево есть - добавяем в очередь
                            if (TreeNodes[buf].right != -1)
                                nodes.Enqueue(TreeNodes[buf].right);
                        }
                        //Если по кончанию цикла элемент не добавлен (все левые поддеревья заняты), но есть эл-т с пустым правым поддеревом, добавляем в него
                        if (!isadded && bufForRight != -1)
                        {
                            //Инициализация графики
                            TreeNode treeNode = new TreeNode(node.ToString());
                            //Создание эл-та
                            TreeNodes[FirstPos] = new Elem<T>(node, treeNode);
                            //Добавление графических потомков узлу-родителю
                            TreeNodes[bufForRight].viewNode.Nodes.Add(treeNode);
                            //Перемещение идекса правого поддерева
                            TreeNodes[bufForRight].right = FirstPos;
                            //Добавление в стек
                            orderNodes.Push(bufForRight);
                            //Переход к след свободной ячейке
                            FirstPos++;
                            //Прекращение цикла
                            isadded = true;
                            return;
                        }
                    }
                }
            }
        }


        //Очистка дерева
        public void Clear()
        {
            //Очистка TreeView
            view.Nodes.Clear();
            //Обнуление всех эл-тов массива
            for (int i = 0; i < orderNodes.Count; ++i)
                TreeNodes[i] = null;
            //Первая свободная позици - 0
            FirstPos = 0;
            //Очистка истории добавления узлов
            orderNodes.Clear();
        }

        /// <summary>
        ///Поиск элемента с помощью рукерсивой ф-ции IsThatElem (если элемент не найден - возвращает -1)
        ///Contains возврвщвает false, если IsThatElem вернет -1 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Contains(T node)
        {
            //node - информационная часть, 0- индекс в массиве, с которго начинать поиск
            int pos = IsThatElem(node, 0);   
            return pos != -1;
        }


        /// <summary>
        ///Перебор для консрукции foreach. Обход в ширину
 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            //если коллекция пуста - исключение
            if (IsEmpty)
                throw new TryToGetEmptyTree();
            //Очередь идексов эл-тов
            Queue<int> indexes = new Queue<int>();
            indexes.Enqueue(0);
            while (indexes.Count > 0)
            {
                //Достаем индекс и возвращаем его информационную часть
                int b = indexes.Dequeue();
                yield return TreeNodes[b].info;
                //Если есть потомки, добавляем в очередь
                if (TreeNodes[b].left != -1)
                    indexes.Enqueue(TreeNodes[b].left);
                if (TreeNodes[b].right != -1)
                    indexes.Enqueue(TreeNodes[b].right);
            }

        }
        /// <summary>
        /// Удаление элемента node типа Т , если дерево пусто -исключение
        /// </summary>
        /// <param name="node"></param>
        public void Remove(T node)
        {
            //Если дерево пусто -исключение
            if (IsEmpty)
                throw new RemovingFromEmptyTree();
            //Ищем узел с такой же информационной частью, как и node
            int res = IsThatElem(node, 0);
            //Если он найден
            if (res != -1)
            {
                if (orderNodes.Count==0)
                {
                    Clear();

                }
                else
                {
                    //Достаем идекс последнего добавленного
                    int last = TreeNodes[orderNodes.Peek()].right == -1 ? TreeNodes[orderNodes.Peek()].left : TreeNodes[orderNodes.Peek()].right;
                    if (last != res)
                        TreeNodes[res].info = TreeNodes[last].info;
                    TreeNodes[res].viewNode.Text = TreeNodes[res].info.ToString();
                    TreeNodes[last].viewNode.Remove();
                    int pl = orderNodes.Pop();
                    if (TreeNodes[pl].right == -1)
                        TreeNodes[pl].left = -1;
                    else
                        TreeNodes[pl].right = -1;
                    TreeNodes[last] = null;
                    FirstPos--;
                }
                
            }
        }

        /// <summary>
        ///кол-во узлов в дереве 
        /// </summary>
        public int Count { get { return FirstPos; } }

        /// <summary>
        ///Пусто ли дерево
        /// </summary>
        public bool IsEmpty { get { return FirstPos== 0; } }


        /// <summary>
        ///Возват объекта, поддерживающего IEnumerable 
        /// </summary>
        public IEnumerable<T> nodes { get { return this; } }

        /// <summary>
        ///Отображение всего дерева, на вход поступет TreeView, на которую отобразить
        /// </summary>
        /// <param name="tree"></param>
        public void DisplayAllTree(TreeView tree)
        {
            //Создание и заполнение нового узла
            TreeNodes[0].viewNode = new TreeNode();
            TreeNodes[0].viewNode.Text = TreeNodes[0].info.ToString();
            //Добавление в tree
            tree.Nodes.Add(TreeNodes[0].viewNode);
            //Если есть потомки - вызываем рекурсивную процедкру добавления
            if (TreeNodes[0].left != -1)
                DisplayOneNode(TreeNodes[0].left, TreeNodes[0].viewNode);
            if (TreeNodes[0].right != -1)
                DisplayOneNode(TreeNodes[0].right, TreeNodes[0].viewNode);
        }

        //Идексатор для доступа к массиву
        public Elem<T> this[int i] { get { return TreeNodes[i]; } }
        //Фунция возвращает идекс элемента, у которого информационная часть совпадает с параметром node, index - параметр индекса элемента в массиве
        private int IsThatElem(T node, int index)
        {
            //если дошли до конца - возвращаем -1
            if (index == -1 || index >= TreeNodes.Length)
                   return -1;
            //если элемент совпал возвращаем индекс
            if (TreeNodes[index].info.CompareTo(node) == 0)
                return index;
            //Если нет - просматриваем левое поддерево
            int l = IsThatElem(node, TreeNodes[index].left);
            //если и там нет - возвращаем результат правого
            if (l == -1)
                return IsThatElem(node, TreeNodes[index].right);
            //Если в левом поддереве есть найденный элемент, то просто его возвращаем
            return l;
        }

        //Реализация интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Поиск родителя, Child - индекс потомка, родителя которого нужно найти pos - позиция текущего узла 
        //Возвращает или -1, если родительне найден (ищем пдителя для корня) или позицию эл-та у которого один из потомков на Child позиции
        //По реализации схоже с IsThatElem
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
        
        //Передается parent - узел в потомки которого нужно записать текущий и pos - индекс элемента
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
    }
}
