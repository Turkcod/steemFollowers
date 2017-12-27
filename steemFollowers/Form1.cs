using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace steemFollowers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            // getFollowers function send us a tuple object
            var received = Followers.getFollowers(usernameT.Text);
            label1.Text = "Followers: " + received.Item1.ToString();
            label2.Text = "Following: " + received.Item2.ToString();

            // received ıtems will add in DataGridView
            foreach(follow f in received.Item3)
            {
                dataGridView1.Rows.Add(f.follower, f.following, f.type);
            }

        }
    }
}
