using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl
{
    public class parsing
    {
        public static plt_node left_brackets = new plt_node();
        public static plt_node right_brackets = new plt_node();

        public static bool compare(ref string src, string dst)
        {
            if (src.StartsWith(dst)) {
                src = src.Substring(dst.Length);
                return true;
            }
            return false;
        }
        public static bool is_string(string src)
        {
            return src.StartsWith("\"");
        }
        public static string get_string(ref string src)
        {
            int index=1;
            for (; index < src.Length; index++)
            {
                if (src[index] == '\"')
                {
                    break;
                }
                if (src[index] == '\\') index++;
            }
            string ret = src.Substring(1, index-1);
            src = src.Substring(index+1);
            return ret;
        }
        public static plt_node make_node(ref string text)
        {
            text=text.TrimStart(" ".ToCharArray());
            if (compare(ref text, "(")) return left_brackets;
            if (compare(ref text, ")")) return right_brackets;
            if (is_string(text)) return new plt_symbol(get_string(ref text));
            if (compare(ref text, "'")) return plt_lambda.quote;

            //get first word
            string fst_word=text.Substring(0, text.IndexOf(' '));
            text = text.Substring(fst_word.Length);

            if (fst_word == "nil") return null;
            if (fst_word == "lambda") return symbol_node(plt_lambda.make_lambda);
            if (fst_word == "cond") return symbol_node(plt_lambda.cond);

            //is number?
            try
            {
                int ret = Convert.ToInt32(fst_word);
                return new plt_int(ret);
            }
            catch (FormatException) { }

            return new plt_symbol(fst_word);
        }
        public static plt_node make_exp(ref string exp) {
            plt_node ret  = make_node(ref exp);
            // do with ( 
            if (ReferenceEquals(ret, left_brackets))
            {
                plt_list r = new plt_list();
                while (true)
                {
                    if (exp.Length <= 0) break;
                    ret = make_exp(ref exp);
                    if (ReferenceEquals(ret, right_brackets)) { break; }
                    r.add(ret);
                }
                return r;
            }
            //do with '
            if (ReferenceEquals(ret, plt_lambda.quote))
            {
                plt_list r = new plt_list();
                r.add(plt_lambda.quote);
                r.add(make_exp(ref exp));
               return r;
            }

            //do nothing
            return ret;
        }
        public static string format_string(string s)
        {
            string ret = s;
            ret = ret.Replace("\n", " ");
            ret = ret.Replace("\r", " ");
            ret = ret.Replace("\t", " ");
            ret = ret.Replace(")", " )");
            return ret;
        }
        public static  plt_node quote_node(plt_node n)
        {
            plt_list ret = new plt_list();
            ret.add(plt_lambda.quote);
            ret.add(n);
            return ret;
        }
        public static plt_node symbol_node(plt_node n)
        {
            plt_symbol ret = new plt_symbol();
            ret.point_to = n;
            return ret;
        }
        public static void set_all_symbol_env(plt_list e,plt_list env) {
            if (e == null) { return; }
            foreach (plt_node i in e._member)
            {
                plt_symbol if_symbol = i as plt_symbol;
                if (if_symbol != null)
                {
                    if (env.name_map.ContainsKey(if_symbol.name))
                        if_symbol.point_to = env.get_member(if_symbol.name);
                    continue; ;
                }
                set_all_symbol_env(i as plt_list, env);
            }
        }

    }
}
