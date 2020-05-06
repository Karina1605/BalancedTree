using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using BalancedTree;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        //Само дерево
        ITree<int> TreeInt;
        ITree<string> TreeString;
        public Form1()
        {
            InitializeComponent();
        }

        
        //Выбор реализации
        private void button1_Click(object sender, EventArgs e)
        {
            if (Implement.Text == "" || Type.Text=="")
                MessageBox.Show("Выберите параметры");
            else
            {
                NodesTree.Nodes.Clear();
                TreeInt = null;
                TreeString = null;
                switch (Implement.Text)
                {
                    case "Цепочная":
                        switch (Type.Text)
                        {
                            case "int":
                                TreeInt = new ListTree<int>(NodesTree);
                                ForEachbutton.Text = "Умножить все на 2";
                                CheckForAllbutton.Text = "Проверить, что все  делятся на 3";
                                FindAllbutton.Text = "Найти все четные";
                                break;
                            case "string":
                                TreeString = new ListTree<string>(NodesTree);
                                ForEachbutton.Text = "К верзнему регистру";
                                CheckForAllbutton.Text = "Проверить, что длина всех слов <10";
                                FindAllbutton.Text = "Найти все слова длиной 4";
                                break;
                        }
                        break;
                    case "Сплошная":
                        switch (Type.Text)
                        {
                            case "int":
                                TreeInt = new ArrayTree<int>(NodesTree);
                                ForEachbutton.Text = "Умножить все на 2";
                                CheckForAllbutton.Text = "Проверить, что все  делятся на 3";
                                FindAllbutton.Text = "Найти все четные";
                                break;
                            case "string":
                                TreeString = new ArrayTree<string>(NodesTree);
                                TreeString = new ListTree<string>(NodesTree);
                                ForEachbutton.Text = "К верзнему регистру";
                                CheckForAllbutton.Text = "Проверить, что длина всех слов <10";
                                FindAllbutton.Text = "Найти все слова длиной 4";
                                break;
                        }
                        break;
                }
                Addbutton.Enabled = true;
                Removebutton.Enabled = true;
                IsContainbutton.Enabled = true;
                CheckForAllbutton.Enabled = true;
                FindAllbutton.Enabled = true;
                ForEachbutton.Enabled = true;
                BuildUnmutablebutton.Enabled = true;
                button2.Enabled = true;
            }
        }

        //В последующих 4 функциях осущесвляется попытка вызова метода ITree и ловится исключение, если нерево неизменяемо 
        //Добаление узла
        private void Addbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Interaction.InputBox("Введите значение", "Ввод");
                if (TreeInt != null)
                {
                    int r;
                    if (Int32.TryParse(s, out r))
                        TreeInt.Add(r);
                    else
                        MessageBox.Show("Вы должны ввести число");
                }
                else
                    TreeString.Add(s);
            }
            catch (AttemptOfChangingUnmutableTree)
            {
                MessageBox.Show("Нельзя добавлять в неизменяемое дерево",  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FullTreeException)
            {
                MessageBox.Show("Достигнуто максимальное кол-во узлов",  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //Удаление узла
        private void Removebutton_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Interaction.InputBox("Введите значение", "Ввод");
                if (TreeInt != null)
                {
                    int r;
                    if (Int32.TryParse(s, out r))
                        TreeInt.Remove(r);
                    else
                        MessageBox.Show("Вы должны ввести число", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    TreeString.Remove(s);
            }
            catch (AttemptOfChangingUnmutableTree)
            {
                MessageBox.Show("Нельзя удалять из неизменяемого дерева", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (RemovingFromEmptyTree ex)
            {
                MessageBox.Show("Дерево пусто");
            }
            
        }
        
        //Преобразование всех эл-тов дерева
        private void ForEachbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeInt != null)
                    TreeUtils<int>.ForEach(TreeInt, TreeUtils<int>.Action);
                else
                    TreeUtils<string>.ForEach(TreeString, TreeUtils<string>.Action);
                NodesTree.Nodes.Clear();
                if (TreeInt != null)
                    TreeInt.DisplayAllTree(NodesTree);
                else
                    TreeString.DisplayAllTree(NodesTree);
            }
            catch (AttemptOfChangingUnmutableTree)
            {
                MessageBox.Show("Нельзя изменять неизменяемое дерево", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        //Очистка дерева
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeString == null)
                    TreeInt.Clear();
                else
                    TreeString.Clear();
            }
            catch (AttemptOfChangingUnmutableTree)
            {
                MessageBox.Show("Нельзя изменять неизменяемое дерево", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //В послед 4 функциях изменеия дерева нет
        //Проверка, содержится ли элемент в дереве
        private void IsContainbutton_Click(object sender, EventArgs e)
        {
            string s = Interaction.InputBox("Введите значение", "Ввод");
            if (TreeString==null)
            {
                int r;
                if (Int32.TryParse(s, out r))
                {
                    if (TreeInt.Contains(r))
                        MessageBox.Show("Элемент \"" + r.ToString() + "\" содержится в дереве");
                    else
                        MessageBox.Show("Элемента \"" + r.ToString() + "\" нет в дереве");
                }
                else
                    MessageBox.Show("Вы должны ввести число");
            }
            else
            {
                if (TreeString.Contains(s))
                    MessageBox.Show("Элемент \"" + s+ "\" содержится в дереве");
                else
                    MessageBox.Show("Элемента \"" + s+ "\" нет в дереве");
            }
            
        }

        //Проверка соотвествия всех эл-тов условию
        private void CheckForAllbutton_Click(object sender, EventArgs e)
        {
            if (TreeString == null)
                if (TreeUtils<int>.CheckForAll(TreeInt, TreeUtils<int>.Check))
                    MessageBox.Show("Все эл-ты дерева делятся на 3");
                else
                    MessageBox.Show("Не все эл-ты дерева делятся на 3");
            else
                if (TreeUtils<string>.CheckForAll(TreeString, TreeUtils<string>.Check))
                MessageBox.Show("Длина каждой строки дерева меньше 10");
            else
                MessageBox.Show("Есть строки длины больше 4");
        }

        //Выборка всех эл-тов соответствующих условию
        private void FindAllbutton_Click(object sender, EventArgs e)
        {
            //В целях упрощения записи заводится переменная
            ITree<int> res =null;
            ITree<string> resS= null;
            if (TreeInt!=null)
            {

                if (TreeInt is ListTree<int>)
                    res = TreeUtils<int>.FindAll(TreeInt, TreeUtils<int>.CheckForNew, TreeUtils<int>.ListConstuctorDelegate);
                else
                    res = TreeUtils<int>.FindAll(TreeInt, TreeUtils<int>.CheckForNew, TreeUtils<int>.ArrayConstructorDelegate);
            }
            else
            {
                if (TreeString is ListTree<String>)
                    resS = TreeUtils<string>.FindAll(TreeString, TreeUtils<string>.CheckForNew, TreeUtils<string>.ListConstuctorDelegate);
                else
                    resS = TreeUtils<string>.FindAll(TreeString, TreeUtils<int>.CheckForNew, TreeUtils<string>.ArrayConstructorDelegate);

            }
            //Создание и отображение формы с результирующим деревом
            Form form = new Form();
            TreeView view = new TreeView();
            view.Width = 600;
            view.Height = 500;
            if (TreeInt != null)
                res.DisplayAllTree(view);
            else
                resS.DisplayAllTree(view);
            form.Controls.Add(view);
            view.Location = new Point(20, 20);
            form.Width = 800;
            form.Height = 600;
            form.Show();
        }


        //построение неизменяемого дерева
        private void BuildUnmutablebutton_Click(object sender, EventArgs e)
        {
            if (TreeInt!=null)
            {
                UnmutableTree<int> UnTree = new UnmutableTree<int>(TreeInt);
                TreeInt = UnTree;
            }
            else
            {
                UnmutableTree<string> UnTree = new UnmutableTree<string>(TreeString);
                TreeString = UnTree;
            }
            MessageBox.Show("Дерево построено!");
        }

        
    }
}
