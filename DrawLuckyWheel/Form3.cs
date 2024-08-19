﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawLuckyWheel
{
    public partial class Form3 : Form
    {
        public Form3(List<string> history)
        {
            InitializeComponent();
            this.TransparencyKey = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;
           
            this.listView1.Columns.Add("Lần");
            this.listView1.Columns.Add("Kết quả");
            for (int i = 0; i < history.Count; i++)
            {
                this.listView1.Items.Add(new ListViewItem(new string[] { (i + 1).ToString(), history[i] }));
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
