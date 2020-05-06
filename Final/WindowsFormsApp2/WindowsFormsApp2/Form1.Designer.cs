namespace WindowsFormsApp2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Implement = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.NodesTree = new System.Windows.Forms.TreeView();
            this.Addbutton = new System.Windows.Forms.Button();
            this.Removebutton = new System.Windows.Forms.Button();
            this.IsContainbutton = new System.Windows.Forms.Button();
            this.ForEachbutton = new System.Windows.Forms.Button();
            this.CheckForAllbutton = new System.Windows.Forms.Button();
            this.FindAllbutton = new System.Windows.Forms.Button();
            this.BuildUnmutablebutton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Typelabel = new System.Windows.Forms.Label();
            this.Type = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Implement
            // 
            this.Implement.FormattingEnabled = true;
            this.Implement.Items.AddRange(new object[] {
            "Сплошная",
            "Цепочная"});
            this.Implement.Location = new System.Drawing.Point(12, 37);
            this.Implement.Name = "Implement";
            this.Implement.Size = new System.Drawing.Size(143, 24);
            this.Implement.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Выберите реализацию";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(466, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 24);
            this.button1.TabIndex = 4;
            this.button1.Text = "Создать дерево";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NodesTree
            // 
            this.NodesTree.Location = new System.Drawing.Point(12, 111);
            this.NodesTree.Name = "NodesTree";
            this.NodesTree.Size = new System.Drawing.Size(626, 428);
            this.NodesTree.TabIndex = 5;
            // 
            // Addbutton
            // 
            this.Addbutton.Enabled = false;
            this.Addbutton.Location = new System.Drawing.Point(702, 25);
            this.Addbutton.Name = "Addbutton";
            this.Addbutton.Size = new System.Drawing.Size(185, 46);
            this.Addbutton.TabIndex = 6;
            this.Addbutton.Text = "Добавить узел";
            this.Addbutton.UseVisualStyleBackColor = true;
            this.Addbutton.Click += new System.EventHandler(this.Addbutton_Click);
            // 
            // Removebutton
            // 
            this.Removebutton.Enabled = false;
            this.Removebutton.Location = new System.Drawing.Point(702, 77);
            this.Removebutton.Name = "Removebutton";
            this.Removebutton.Size = new System.Drawing.Size(185, 46);
            this.Removebutton.TabIndex = 7;
            this.Removebutton.Text = "Удалить";
            this.Removebutton.UseVisualStyleBackColor = true;
            this.Removebutton.Click += new System.EventHandler(this.Removebutton_Click);
            // 
            // IsContainbutton
            // 
            this.IsContainbutton.Enabled = false;
            this.IsContainbutton.Location = new System.Drawing.Point(702, 129);
            this.IsContainbutton.Name = "IsContainbutton";
            this.IsContainbutton.Size = new System.Drawing.Size(185, 46);
            this.IsContainbutton.TabIndex = 8;
            this.IsContainbutton.Text = "Найти элемент";
            this.IsContainbutton.UseVisualStyleBackColor = true;
            this.IsContainbutton.Click += new System.EventHandler(this.IsContainbutton_Click);
            // 
            // ForEachbutton
            // 
            this.ForEachbutton.Enabled = false;
            this.ForEachbutton.Location = new System.Drawing.Point(702, 181);
            this.ForEachbutton.Name = "ForEachbutton";
            this.ForEachbutton.Size = new System.Drawing.Size(185, 46);
            this.ForEachbutton.TabIndex = 9;
            this.ForEachbutton.UseVisualStyleBackColor = true;
            this.ForEachbutton.Click += new System.EventHandler(this.ForEachbutton_Click);
            // 
            // CheckForAllbutton
            // 
            this.CheckForAllbutton.Enabled = false;
            this.CheckForAllbutton.Location = new System.Drawing.Point(702, 233);
            this.CheckForAllbutton.Name = "CheckForAllbutton";
            this.CheckForAllbutton.Size = new System.Drawing.Size(185, 46);
            this.CheckForAllbutton.TabIndex = 10;
            this.CheckForAllbutton.UseVisualStyleBackColor = true;
            this.CheckForAllbutton.Click += new System.EventHandler(this.CheckForAllbutton_Click);
            // 
            // FindAllbutton
            // 
            this.FindAllbutton.Enabled = false;
            this.FindAllbutton.Location = new System.Drawing.Point(702, 285);
            this.FindAllbutton.Name = "FindAllbutton";
            this.FindAllbutton.Size = new System.Drawing.Size(185, 46);
            this.FindAllbutton.TabIndex = 11;
            this.FindAllbutton.UseVisualStyleBackColor = true;
            this.FindAllbutton.Click += new System.EventHandler(this.FindAllbutton_Click);
            // 
            // BuildUnmutablebutton
            // 
            this.BuildUnmutablebutton.Enabled = false;
            this.BuildUnmutablebutton.Location = new System.Drawing.Point(702, 337);
            this.BuildUnmutablebutton.Name = "BuildUnmutablebutton";
            this.BuildUnmutablebutton.Size = new System.Drawing.Size(185, 46);
            this.BuildUnmutablebutton.TabIndex = 12;
            this.BuildUnmutablebutton.Text = "Построить неизменяемое дерево";
            this.BuildUnmutablebutton.UseVisualStyleBackColor = true;
            this.BuildUnmutablebutton.Click += new System.EventHandler(this.BuildUnmutablebutton_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(702, 390);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(185, 46);
            this.button2.TabIndex = 13;
            this.button2.Text = "Очистить дерево";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Typelabel
            // 
            this.Typelabel.AutoSize = true;
            this.Typelabel.Location = new System.Drawing.Point(193, 8);
            this.Typelabel.Name = "Typelabel";
            this.Typelabel.Size = new System.Drawing.Size(153, 17);
            this.Typelabel.TabIndex = 14;
            this.Typelabel.Text = "Выберите тип данных";
            // 
            // Type
            // 
            this.Type.FormattingEnabled = true;
            this.Type.Items.AddRange(new object[] {
            "int",
            "string"});
            this.Type.Location = new System.Drawing.Point(196, 35);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(143, 24);
            this.Type.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 551);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.Typelabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.BuildUnmutablebutton);
            this.Controls.Add(this.FindAllbutton);
            this.Controls.Add(this.CheckForAllbutton);
            this.Controls.Add(this.ForEachbutton);
            this.Controls.Add(this.IsContainbutton);
            this.Controls.Add(this.Removebutton);
            this.Controls.Add(this.Addbutton);
            this.Controls.Add(this.NodesTree);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Implement);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox Implement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView NodesTree;
        private System.Windows.Forms.Button Addbutton;
        private System.Windows.Forms.Button Removebutton;
        private System.Windows.Forms.Button IsContainbutton;
        private System.Windows.Forms.Button ForEachbutton;
        private System.Windows.Forms.Button CheckForAllbutton;
        private System.Windows.Forms.Button FindAllbutton;
        private System.Windows.Forms.Button BuildUnmutablebutton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label Typelabel;
        private System.Windows.Forms.ComboBox Type;
    }
}

