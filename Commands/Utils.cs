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


        public static Dictionary<string, int> regs = new Dictionary<string, int>()
            {
                { "R0", 00}, { "R1", 01}, { "R2", 02}, { "R3", 03}, { "R4", 04}, { "R5", 05}, { "R6", 06}, { "R7", 07},
                { "R8", 08}, { "R9", 09}, { "R10", 10}, { "R11", 11}, { "R12", 12}, { "R13", 13}, { "R14", 14}, { "R15", 15},
            };

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

        public static bool isReg(string arg) => regs.ContainsKey(arg);

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

        public static string GetUniquePrefix(string arg = "") => $"{config.stack.Peek()}_{arg}_{config.stack.Count()}";

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
