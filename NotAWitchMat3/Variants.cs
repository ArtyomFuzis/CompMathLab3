using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotAWitchMat3
{
    internal class Cancellable<T>
    {
        T? obj;
        bool present;
        public Cancellable(T obj) { this.obj = obj; this.present = true; }
        public Cancellable() { this.obj = default; this.present = false; }
        public bool get(out T? res)
        {
            res = obj;
            return present;
        }
    }
    internal enum IntMethodsVariants
    {
        RectangeLeft,
        RectangeRight,
        RectangeMid,
        Simpson,
        Trapezoid
    }
    internal enum IntFuncVariants
    {
        Equ1,
        Equ2,
        Equ3,
        Equ4
    }
    internal enum ImpopFuncVariants
    {
        EquImpop1,
        EquImpop2,
        EquImpop3
    }
    delegate double MathFunc(double x);
    delegate bool IntervalValidator(double a, double b);
    delegate List<double> BreakPointFinder(double a, double b);
    internal static class VariantsMaps
    {
        public static Dictionary<IntMethodsVariants, string> integralMethodsVariants = new Dictionary<IntMethodsVariants, string>
        {
            {IntMethodsVariants.RectangeLeft, "Прямоугольников (левый)" },
            {IntMethodsVariants.RectangeRight, "Прямоугольников (правый)" },
            {IntMethodsVariants.RectangeMid, "Прямоугольников (средний)" },
            {IntMethodsVariants.Trapezoid, "Трапеций" },
            {IntMethodsVariants.Simpson, "Симпсона" }
        };
        public static Dictionary<IntFuncVariants, string> integralVariants = new Dictionary<IntFuncVariants, string>
        {
            {IntFuncVariants.Equ1, "f(x) = 2x^3-9x^2-7x+11" },
            {IntFuncVariants.Equ2, "f(x) = sin(x)" },
            {IntFuncVariants.Equ3, "f(x) = tan(x)*ln(10x)*x" },
            {IntFuncVariants.Equ4, "f(x) = e^(-x^2/2)" }
        };
        public static Dictionary<IntFuncVariants, MathFunc> integralVariantsFuncs = new Dictionary<IntFuncVariants, MathFunc>
        {
            {IntFuncVariants.Equ1, (x) => 2*x*x*x-9*x*x-7*x+11},
            {IntFuncVariants.Equ2, Math.Sin },
            {IntFuncVariants.Equ3, (x)=>Math.Tan(x)*Math.Log(10*x)*x },
            {IntFuncVariants.Equ4, (x)=>Math.Exp(-x*x/2) }
        };
        public static Dictionary<IntFuncVariants, IntervalValidator> integralVariantsIntervalsValidators = new Dictionary<IntFuncVariants, IntervalValidator>
        {
            {IntFuncVariants.Equ1, (a,b)=>true },
            {IntFuncVariants.Equ2, (a,b)=>true },
            {IntFuncVariants.Equ3, (a,b)=>a>0 },
            {IntFuncVariants.Equ4, (a,b)=>true }
        };
        public static Dictionary<ImpopFuncVariants, string> impopVariants = new Dictionary<ImpopFuncVariants, string>
        {
            {ImpopFuncVariants.EquImpop1, "f(x) = 1/(x(x-1))" },
            {ImpopFuncVariants.EquImpop2, "f(x) = ln(x)" },
            {ImpopFuncVariants.EquImpop3, "f(x) = 1/(x-10)^2" }
        };
        public static Dictionary<ImpopFuncVariants, MathFunc> impopVariantsFuncs = new Dictionary<ImpopFuncVariants, MathFunc>
        {
            {ImpopFuncVariants.EquImpop1, (x)=> 1/(x*(x-1))},
            {ImpopFuncVariants.EquImpop2, Math.Log },
            {ImpopFuncVariants.EquImpop3, (x)=>1/(x-10)/(x-10) }
        };
        public static Dictionary<ImpopFuncVariants, IntervalValidator> impopVariantsIntervalsValidators = new Dictionary<ImpopFuncVariants, IntervalValidator>
        {
            {ImpopFuncVariants.EquImpop1, (a,b)=>true},
            {ImpopFuncVariants.EquImpop2, (a,b)=>a>=0 },
            {ImpopFuncVariants.EquImpop3, (a,b)=>true }
        };
        public static Dictionary<ImpopFuncVariants, BreakPointFinder> impopVariantsBreaks = new Dictionary<ImpopFuncVariants, BreakPointFinder>
        {
            {ImpopFuncVariants.EquImpop1, (a,b)=>{
                List<double> breakPoints = new List<double>();
                if(a <= 0 && b >= 0) breakPoints.Add(0);
                if(a <= 1 && b >= 1)breakPoints.Add(1);
                return breakPoints;
            } },
            {ImpopFuncVariants.EquImpop2, (a,b)=>{
                List<double> breakPoints = new List<double>();
                if(a <= 0 && b >= 0) breakPoints.Add(0);
                return breakPoints;
            } },
            {ImpopFuncVariants.EquImpop3, (a,b)=>{
                List<double> breakPoints = new List<double>();
                if(a <= 10 && b >= 10) breakPoints.Add(10);
                return breakPoints;
            } }
        };

    }
}
