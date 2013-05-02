using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace pl
{
    public class plt_tail
    {
        public plt_node value;
        public bool is_tail;
        public static plt_tail nil = new plt_tail();
        public plt_tail() { value = null; is_tail = false; }
        public plt_tail(plt_node _value, bool _is_tail)
        {
            this.value = _value;
            this.is_tail = _is_tail;
        }
    } 
    


    public class plt_node
    {
        public static plt_node eval_node(plt_node input)
        {
            plt_tail ret;
            do
            {
                if (input != null){
                    ret = input.eval();
                }
                else {
                    ret = plt_tail.nil;
                }

                if (ret.is_tail)
                {
                    input = ret.value;
                }
            } while (ret.is_tail);

            return ret.value;
        }

        public virtual plt_tail eval() { return new plt_tail(this,false); }
        public override string ToString() { return "plt_node"; }

    }



    public class plt_list : plt_node//, IEnumerable
    {
        private  Dictionary<string, int> _name_map;
        public  Dictionary<string, int> name_map{
            set{_name_map=value;}
            get{
            if(_name_map==null) _name_map=new Dictionary<string,int> ();
            return _name_map;
        }}
        public List<plt_node> _member= new List<plt_node>();
        public plt_node get_member(int index)
        {
            if (index >= 0 && index < _member.Count)
            {
                return _member[index];
            }
            return null;
        }
        public plt_node get_member(string name) {
            if ( ! name_map.ContainsKey(name)) return null;
            return get_member(name_map[name]);
        }
        public void set_member(int index, plt_node value) {
            if (index < _member.Count)
            {
                _member[index] = value;
            }
            else
            {
                _member.Add(value);
            }
        }
        public void set_member(string name, plt_node value)
        {
            if (name_map.ContainsKey(name))
            {
                _member[ name_map[name]]=value;
            }
            else
            {
                name_map.Add(name, _member.Count);
                _member.Add(value);
            }
        }
        public void add(string name, plt_node value) {
            name_map.Add(name, _member.Count);
            _member.Add(value);
        }
        public void add( plt_node value)
        {
            _member.Add(value);
        }
        public override plt_tail eval()
        {
            if (_member.Count == 0) return plt_tail.nil;
            plt_node afirst = plt_node.eval_node(_member[0]);
            if (afirst == null) return plt_tail.nil;


            {//特殊的函数

                //特殊情况-不解释参数 
                if (ReferenceEquals(afirst, plt_lambda.quote))
                {
                    if (_member.Count < 2) return plt_tail.nil;
                    return new plt_tail(_member[1], false);
                }
                if (ReferenceEquals(afirst, plt_lambda.make_lambda))
                {
                    plt_list_lambda ret = new plt_list_lambda();
                    ret.arg_name = _member[1] as plt_list;
                    ret.exp = _member[2];
                    return new plt_tail(ret, false);
                }
                if (ReferenceEquals(afirst, plt_lambda.cond))
                {
                    for (int i = 1; i+1 < _member.Count; i+=2) {
                        plt_node _if = plt_node.eval_node(_member[i]);
                        if (_if != null) {
                            return new plt_tail(_member[i + 1], true);
                        }
                    }
                }
                //宏 不解释参数
                plt_lambda lismacro = afirst as plt_lambda;
                if (lismacro != null && lismacro.ismacro)
                {
                    lismacro.mix_env(this);
                    return lismacro.eval();
                }
            }
            plt_list args_eval = new plt_list();
            args_eval.add(afirst);
            for (int i = 1; i < _member.Count; i++)
            {
                args_eval.add(plt_node.eval_node(_member[i]));
            }
            plt_lambda func = afirst as plt_lambda;
            if (func != null)
            {
                func.mix_env(args_eval);
            }
            return afirst.eval();
        }
        public override string ToString() { return "plt_list"; }

        //public IEnumerator GetEnumerator()
        //{
        //    foreach (plt_node i in _member)
        //    {
        //        yield return i;
        //    }
        //}
    }


    public class plt_lambda : plt_node {
        public virtual void mix_env(plt_list arg) { }
        public override string ToString() { return "plt_lambda"; }
        public static plt_lambda quote = new plt_lambda();
        public static plt_lambda make_lambda = new plt_lambda();
        public static plt_lambda cond = new plt_lambda();
        public bool ismacro = false;
    }


}
