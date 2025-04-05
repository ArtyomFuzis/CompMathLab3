using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotAWitchMat3
{
    internal static class SubCommands
    {
        public static int chooseNum(int size)
        {
            while (true)
            {
                string? ln = Console.ReadLine();
                if (ln == "") continue;
                if (ln == null || ln[0] == 0x04)
                {
                    throw new IOException("Console input closed");
                }
                int enteredValue;
                if (!int.TryParse(ln, out enteredValue)) Console.WriteLine("Введено не число");
                else if (enteredValue <= 0 || enteredValue > size)
                {
                    Console.WriteLine("Введено некорректое число");
                }
                else
                {
                    return enteredValue - 1;
                }
            }
        }
        public static double chooseDouble()
        {
            while (true)
            {
                string? ln = Console.ReadLine();
                if (ln == "") continue;
                if (ln == null || ln[0] == 0x04)
                {
                    throw new IOException("Console input closed");
                }
                ln = ln.Replace('.', ',');
                double enteredValue;
                if (!double.TryParse(ln, out enteredValue)) Console.WriteLine("Введено не число");
                else
                {
                    return enteredValue;
                }
            }
        }
        public static Cancellable<T> chooseVariant<T>(Dictionary<T, string> variants, string groupName) where T : Enum
        {
            StringBuilder sb = new StringBuilder();
            int cnt = 1;
            List<T> lst = new List<T>();
            foreach (var method in variants)
            {
                sb.AppendLine($"({cnt++}) {method.Value}");
                lst.Add(method.Key);
            }
            Console.WriteLine($"""

                Выберите {groupName} из предложенных: 
                -----------------------
                {sb}({cnt}) Отмена
                -----------------------

                """);
            try
            {
                int num = chooseNum(cnt);
                if (num == cnt - 1) return new Cancellable<T>();
                return new Cancellable<T>(lst[num]);
            }
            catch (IOException)
            {
                return new Cancellable<T>();
            }
        }
        private static Cancellable<double> choosePartameter(string parametrName)
        {
            Console.WriteLine($"\nВведите параметр {parametrName}:\n");
            try
            {
                return new Cancellable<double>(chooseDouble());
            }
            catch (IOException)
            {
                return new Cancellable<double>();
            }
        }
        public static Cancellable<(double,double,double)> chooseIntervals()
        {
            double a,b,precision;
            var aCancellable = choosePartameter("a");
            bool present = aCancellable.get(out a);
            if(!present) return new Cancellable<(double,double,double)>();
            var bCancellable = choosePartameter("b");
            present = bCancellable.get(out b);
            if (!present) return new Cancellable<(double, double, double)>();
            var precisionCancellable = choosePartameter("точность");
            present = precisionCancellable.get(out precision);
            if (!present) return new Cancellable<(double, double, double)>();
            return new Cancellable<(double, double, double)>((a, b, precision));
        }
    }
}
