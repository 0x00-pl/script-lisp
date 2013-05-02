using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl
{


    public class plt_list_lambda : plt_lambda
    {
        public plt_list arg_name;
        public plt_node exp;
        public override void mix_env(plt_list arg)
        {
            if (arg_name == null) return;
            plt_list env = new plt_list();
            for (int i = 0; i < arg_name._member.Count; i++)
            {
                string name = ((plt_symbol)arg_name.get_member(i)).name;
                plt_node value = arg.get_member(i + 1);
                env.set_member(name, value);
            }
            update_all_symbol(exp as plt_list, env);
        }
        public void update_all_symbol(plt_list e, plt_list env)
        {
            if (e == null) { return; }
            foreach (plt_node i in e._member)
            {
                plt_symbol if_symbol = i as plt_symbol;
                if (if_symbol != null)
                {
                    if (env.name_map.ContainsKey(if_symbol.name))
                        if_symbol.point_to = env.get_member(if_symbol.name);
                    continue;
                }

                update_all_symbol(i as plt_list, env);
            }
        }
        public override plt_tail eval()
        {
            return exp.eval();
        }
        public override string ToString() { return "plt_list_lambda"; }
    }



    public class plt_lambda_add : plt_lambda
    {
        plt_int a;
        plt_int b;

        public override void mix_env(plt_list arg)
        {
            a = (plt_int)arg.get_member(1);
            b = (plt_int)arg.get_member(2);
        }
        public override plt_tail eval()
        {
            return new plt_tail(new plt_int(a.num + b.num), false);
        }
        public override string ToString() { return "plt_lambda_add"; }
    }

}
