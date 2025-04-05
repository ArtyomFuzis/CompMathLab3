using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotAWitchMat3
{
    internal static class Comp
    {
        public static void calc_int(IntMethodsVariants meth, IntFuncVariants func, (double, double, double) intervals)
        {
            var mathFunc = VariantsMaps.integralVariantsFuncs[func];
            var execFunc = Type.GetType("NotAWitchMat3.Comp")?.GetMethod($"calc_int_{meth}");
            if (execFunc == null)
            {
                throw new NotImplementedException();
            }
            int n = 4;
            int iter_cnt = 1;
            double? lastRes = null;
            while (true)
            {
                var res = (double?)execFunc.Invoke(null, [mathFunc, intervals.Item1, intervals.Item2, n]);
                if (res == null) throw new NotImplementedException();
                if (lastRes == null) lastRes = res;
                else if (Math.Abs((double)(lastRes - res)) >= intervals.Item3 / 15) lastRes = res;
                else { lastRes = res; break; }
                n *= 2;
                iter_cnt++;
            }
            Console.WriteLine($"Значение интеграла: {lastRes}, для его вычисления потребовалось {iter_cnt} итераций");
        }
        private static IEnumerable<(double, double)> make_partion(double a, double b, int n)
        {
            double partition_width = (b - a) / n;
            for (double i = a; i < b; i += partition_width)
            {
                yield return (i, i + partition_width);
            }
        }
        public static double calc_int_RectangeLeft(MathFunc func, double a, double b, int n)
        {
            var partiotion = make_partion(a, b, n);
            double result = 0;
            foreach ((double l, double r) in partiotion)
            {
                result += func(l) * (r - l);
            }
            return result;
        }
        public static double calc_int_RectangeRight(MathFunc func, double a, double b, int n)
        {
            var partiotion = make_partion(a, b, n);
            double result = 0;
            foreach ((double l, double r) in partiotion)
            {
                result += func(r) * (r - l);
            }
            return result;
        }
        public static double calc_int_RectangeMid(MathFunc func, double a, double b, int n)
        {
            var partiotion = make_partion(a, b, n);
            double result = 0;
            foreach ((double l, double r) in partiotion)
            {
                result += func((r + l) / 2) * (r - l);
            }
            return result;
        }
        public static double calc_int_Trapezoid(MathFunc func, double a, double b, int n)
        {
            var partiotion = make_partion(a, b, n);
            double result = 0;
            foreach ((double l, double r) in partiotion)
            {
                result += (func(r) + func(l)) / 2 * (r - l);
            }
            return result;
        }
        public static double calc_int_Simpson(MathFunc func, double a, double b, int n)
        {
            var partiotion = make_partion(a, b, n);
            double last = 0;
            List<double> partition_decomposed = new List<double>();
            int cnt = 0;
            double h = 0;
            foreach ((double l, double r) in partiotion)
            {
                partition_decomposed.Add(func(l));
                last = func(r);
                h += r - l;
                cnt++;
            }
            h /= cnt * 3;
            double result = (partition_decomposed[0] + last) * h;
            for (int i = 1; i < partition_decomposed.Count; i++)
            {
                if (i % 2 == 0) result += 2 * partition_decomposed[i] * h;
                else result += 4 * partition_decomposed[i] * h;
            }

            return result;
        }
        public static void calc_impop(IntMethodsVariants meth, ImpopFuncVariants func, (double, double, double) intervals) 
        {
            (double a, double b, double precision) = intervals;
            var break_points_func = VariantsMaps.impopVariantsBreaks[func];
            if(break_points_func == null) throw new NotImplementedException();
            var break_points = break_points_func(a, b);
            break_points.Sort();
            double eps = 0.1;
            bool found = false;
            double? last_res = 0;
            int iters = 0;
            for (int i = 0; i < 20; i++) 
            {
                double last_border = a;
                double res = 0;
                foreach (double break_point in break_points)
                {
                    if (break_point == last_border) continue;
                    (double? cur_int, int cur_iters) = calc_with_epsilon(meth, func, (last_border+eps, break_point-eps, precision));
                    if (cur_int == null) throw new NotImplementedException();
                    res += (double) cur_int;
                    iters = iters > cur_iters ? iters : cur_iters;
                    last_border = break_point;
                }
                if (last_border != b)
                {
                    (double? cur_int_last, int cur_iters_last) = calc_with_epsilon(meth, func, (last_border + eps, b - eps, precision));
                    if (cur_int_last == null) throw new NotImplementedException();
                    res += (double)cur_int_last;
                    iters = iters > cur_iters_last ? iters : cur_iters_last;
                }
                if (last_res == null) last_res = res;
                else if (Math.Abs((double)last_res - res) < precision / 15) 
                {
                    last_res = res;
                    found = true;
                    break;
                }
                else last_res = res;
                //Console.WriteLine($"{(last_border + eps, b - eps, precision)}, {last_res}");
                eps /= 10;
            }
            if (!found)
            {
                Console.WriteLine("Интеграл рассходится");
            }
            else
            {
                Console.WriteLine($"Значение интеграла: {last_res}, для его вычисления потребовалось {iters} итераций");
            }
        }
        private static (double?, int) calc_with_epsilon(IntMethodsVariants meth, ImpopFuncVariants func, (double, double, double) intervals)
        {
            var mathFunc = VariantsMaps.impopVariantsFuncs[func];
            var execFunc = Type.GetType("NotAWitchMat3.Comp")?.GetMethod($"calc_int_{meth}");
            if (execFunc == null)
            {
                throw new NotImplementedException();
            }
            int n = 4;
            int iter_cnt = 1;
            double? lastRes = null;
            for(int i = 0 ; i < 20; i++)
            {
                var res = (double?)execFunc.Invoke(null, [mathFunc, intervals.Item1, intervals.Item2, n]);
                if (res == null) throw new NotImplementedException();
                if (lastRes == null) lastRes = res;
                else if (Math.Abs((double)(lastRes - res)) >= intervals.Item3/15) lastRes = res;
                else { lastRes = res; break; }
                n *= 2;
                iter_cnt++;
            }
            return (lastRes, iter_cnt);
        }
    }
}
