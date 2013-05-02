using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            plt_list static_env = new plt_list();
            static_env.add("+", new plt_lambda_add());

            string input = parsing.format_string(@"((lambda (a) (+ a 1)) 10)");
            plt_node ret = parsing.make_exp(ref input);
            parsing.set_all_symbol_env(ret as plt_list, static_env);
            ret = plt_node.eval_node(ret);

            textBox1.Text = ret.ToString();
        }

    }
}
