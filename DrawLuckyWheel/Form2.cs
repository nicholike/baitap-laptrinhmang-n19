using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace DrawLuckyWheel
{
    public partial class Form2 : Form
    {
        public Form2(string result)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            label1.Text = result;
            Form4 tempForm = new Form4();
            this.AddOwnedForm(tempForm);
            tempForm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        { 
        }
    }
}