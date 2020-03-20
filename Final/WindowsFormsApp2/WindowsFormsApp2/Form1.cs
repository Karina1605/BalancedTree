﻿using System;
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
        ITree<int> TreeInt;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Implement.Text == "")
                MessageBox.Show("Выберите параметры");
            else
            {
                NodesTree.Nodes.Clear();
                TreeInt = null;
                switch (Implement.Text)
                {
                    case "Цепочная":
                        TreeInt = new ListTree<int>(NodesTree);
                        break;
                    case "Сплошная":
                        TreeInt = new ArrayTree<int>(NodesTree);
                        break;
                }
                Addbutton.Enabled = true;
                Removebutton.Enabled = true;
                IsContainbutton.Enabled = true;
                CheckForAllbutton.Enabled = true;
                FindAllbutton.Enabled = true;
                ForEachbutton.Enabled = true;
                BuildUnmutablebutton.Enabled = true;
            }
        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Interaction.InputBox("Введите значение", "Ввод");
                int r;
                if (Int32.TryParse(s, out r))
                    TreeInt.Add(r);
                else
                    MessageBox.Show("Вы должны ввести число");
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

        private void Removebutton_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Interaction.InputBox("Введите значение", "Ввод");
                int r;
                if (Int32.TryParse(s, out r))
                    TreeInt.Remove(r);
                else
                    MessageBox.Show("Вы должны ввести число", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void IsContainbutton_Click(object sender, EventArgs e)
        {
            string s = Interaction.InputBox("Введите значение", "Ввод");
            int r;
            if (Int32.TryParse(s, out r))
                TreeInt.Contains(r);
            else
                MessageBox.Show("Вы должны ввести число");
        }

        private void ForEachbutton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeUtils<int>.ForEach(TreeInt, TreeUtils<int>.Action);
                NodesTree.Nodes.Clear();
                TreeInt.DisplayAllTree(NodesTree);
            }
            catch (AttemptOfChangingUnmutableTree)
            {
                MessageBox.Show("Нельзя изменять неизменяемое дерево", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void CheckForAllbutton_Click(object sender, EventArgs e)
        {
            if (TreeUtils<int>.CheckForAll(TreeInt, TreeUtils<int>.Check))
                MessageBox.Show("Все эл-ты дерева делятся на 3");
            else
                MessageBox.Show("Не все эл-ты дерева делятся на 3");
        }

        private void FindAllbutton_Click(object sender, EventArgs e)
        {
            ITree<int> res;
            if (TreeInt is ListTree<int>)
                res = TreeUtils<int>.FindAll(TreeInt, TreeUtils<int>.CheckForNew, TreeUtils<int>.ListConstuctorDelegate);
            else
                res = TreeUtils<int>.FindAll(TreeInt, TreeUtils<int>.CheckForNew, TreeUtils<int>.ArrayConstructorDelegate);
            Form form = new Form();
            TreeView view = new TreeView();
            view.Width = 600;
            view.Height = 500;
            res.DisplayAllTree(view);
            form.Controls.Add(view);
            view.Location = new Point(20, 20);
            form.Width = 800;
            form.Height = 600;
            form.Show();
        }

        private void BuildUnmutablebutton_Click(object sender, EventArgs e)
        {
            UnmutableTree<int> UnTree = new UnmutableTree<int>(TreeInt);
            TreeInt = UnTree;
            MessageBox.Show("Дерево построено!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TreeInt.Clear();
            }
            catch (AttemptOfChangingUnmutableTree)
            {
                MessageBox.Show("Нельзя изменять неизменяемое дерево", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
