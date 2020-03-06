using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BalancedTree;

namespace WindowsFormsApp2
{
    
    public partial class Form1 : Form
    {
        BaseTree<int> treeInt;
        BaseTree<string> treeString;
        BaseTree<MyPoint> treeMyPoint;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Type.Text == "" || Implement.Text == "")
                MessageBox.Show("Выберите параметры");
            else
            {
                switch (Implement.Text)
                {
                    case "Сплошная":
                        switch(Type.Text)
                        {
                            case "int":
                                treeInt = new ArrayTree<int>();
                                break;
                            case "string":
                                treeString = new ArrayTree<string>();
                                break;
                            case "class Point":
                                treeMyPoint = new ArrayTree<MyPoint>();
                                break;
                        }
                        break;
                    case "Цепочная":
                        switch (Type.Text)
                        {
                            case "int":
                                treeInt = new ListTree<int>();
                                break;
                            case "string":
                                treeString =new Lis
                                break;
                            case "class Point":
                                break;
                        }
                        break;
                }
            }
        }
    }
    class MyPoint : IComparable
    {
        int x;
        int y;
        public MyPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return "x: " + x + " y: " + y;
        }
        public int CompareTo(object obj)
        {
            int res = ((MyPoint)obj).x * ((MyPoint)obj).x + ((MyPoint)obj).y * ((MyPoint)obj).y;
            int o = x * x + y * y;
            if (res == 0)
                return 0;
            if (res < 0)
                return -1;
            return 1;
        }
    }
}
