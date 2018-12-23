using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models;
using sp_macro;

namespace Commands
{
    public class Utils
    {
        public static Regex validAddress = new Regex(@"^([0-9a-f]+)$");

    
        public static  Regex validByteC = new Regex(@"^[cC](['""])(.+)\1$");

        public static  Regex validByteX = new Regex(@"^[xX](['""])([a-fA-F0-9]+)\1$");

        public static  Regex validCommandR = new Regex(@"^([A-Z]+)(_RA)$");

        public static  Regex validCommand = new Regex(@"^([A-Z]+)$");

        public static  Regex validName = new Regex(@"^.?([a-zA-Z]*)([0-9_]*)$");

        public static  Regex validOperation = new Regex(@"^((>)|(<)|(>=)|(<=)|(!=)|(==))$");

        public static  Regex validArg = new Regex(@"^((ARG)([0-9]+))(=)([0-9]+)$");

        public static  Regex validArgKey = new Regex(@"^((ARG)([0-9]+))(=)([a-zA-Z0-9]+)$");

        public static Regex validMacroLable = new Regex(@"^.([A-Za-z]+[0-9]*)$");

        private static Config config = Config.getInstance();

        public static int ConvertTo16(string arg)
        {
            try
            {
                int res = Convert.ToInt32(arg, 16);
                return res;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int ConvertTo10(string arg)
        {
            try
            {
                int res = Convert.ToInt32(arg, 10);
                return res;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static string StringFormatAddress(int arg) => string.Format("{0:X}", arg).PadLeft(6, '0');

        public static string StringFormatCommand(int arg) => string.Format("{0:X}", arg).PadLeft(2, '0');

        public static string GetUniquePrefix(string arg = "") => $"{config.stack.Peek()}_{config.stack.Count()}_{arg}";

        public static bool Compare(string first, string second, string sign)
        {
            return Compare(ConvertTo10(first), ConvertTo10(second), sign);
        }

        public static bool Compare(int first, int second, string sign)
        {
            switch (sign)
            {
                case ">":
                    return first > second;
                case "<":
                    return first < second;
                case ">=":
                    return first >= second;
                case "<=":
                    return first <= second;
                case "==":
                    return first == second;
                case "!=":
                    return first != second;
                default:
                    return false;
            }
        }
        public static string GetUniqueLabel(string arg)
        {
            string lable = config.unigueLabel.ContainsKey(GetUniquePrefix(arg)) ? config.unigueLabel.First(i => i.Key == GetUniquePrefix(arg)).Value : "";
            if (lable.isEmpty()) lable = SetUniqueLabel(arg); 

            return lable;
        }
        
        private static string GenerateUniqueLabel(string arg) => arg.isNotEmpty() && validName.IsMatch(arg) ? $"{GetUniquePrefix(arg)}.{config.unigueLabel.Count()}" : "";

        public static string SetUniqueLabel(string arg)
        {
            if (arg.isEmpty()) return null;
            else
            {
                string keyArg = GetUniquePrefix(arg);
                if (!config.unigueLabel.Any(i => i.Key == keyArg))
                {
                    var lable = GenerateUniqueLabel(arg);
                    config.unigueLabel.Add(keyArg, lable);
                    return lable;
                }
            }
            return null;
        }


    }
}
