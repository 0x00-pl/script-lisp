using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl
{
    public class plt_symbol : plt_node
    {
        public string name;
        public plt_symbol() { name = "NONAME"; }
        public plt_symbol(string s) { name = s; }
        public plt_node point_to;
        public override plt_tail eval()
        {
            return new plt_tail(point_to, false);
        }
        public override string ToString() { return name; }
    }

    public class plt_int : plt_node
    {
        public int num;
        public plt_int(int n) { num = n; }
        public override plt_tail eval() { return new plt_tail(this, false); }
        public override string ToString() { return num.ToString(); }
    }
}
