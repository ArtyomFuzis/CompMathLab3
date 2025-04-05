using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace NotAWitchMat3
{
    internal static class Commands
    {
        public static void process_command(string command)
        {
            MethodInfo? cmd = Type.GetType("NotAWitchMat3.Commands")?.GetMethod($"cmd_{command}");
            if (cmd == null)
            {
                Console.WriteLine("К сожалению, выбранная операция не найдена, попробуйте help");
            }
            else
            {
                cmd.Invoke(null, null);
            }
        }
        public static void cmd_help()
        {
            Console.WriteLine("""

                Доступные команды:
                -----------------------
                calc_int - вычислить интеграл (с выбором метода и функции) 
                calc_impop - вычислить несобственный интеграл (с выбором метода и функции)
                help - вывести данную информацию
                -----------------------

                """);
        }
        public static void cmd_calc_int()
        {
            var method = SubCommands.chooseVariant(VariantsMaps.integralMethodsVariants, "метод");
            IntMethodsVariants methVar;
            bool method_present = method.get(out methVar);
            if (!method_present)
            {
                Console.WriteLine("Операция отменена!");
                return;
            }
            var func = SubCommands.chooseVariant(VariantsMaps.integralVariants, "функцию");
            IntFuncVariants funcVar;
            bool func_present = func.get(out funcVar);
            if (!func_present)
            {
                Console.WriteLine("Операция отменена!");
                return;
            }
            (double, double, double) interVar;
            while (true)
            {
                var intervals = SubCommands.chooseIntervals();
                bool intervals_present = intervals.get(out interVar);
                if (!intervals_present)
                {
                    Console.WriteLine("Операция отменена!");
                    return;
                }
                var intervalsValidator = VariantsMaps.integralVariantsIntervalsValidators[funcVar];
                bool internalValidation = intervalsValidator.Invoke(interVar.Item1, interVar.Item2);
                if (!internalValidation) Console.WriteLine("Введенные параметры не попадают в ОДЗ, введите заново:");
                else if (interVar.Item1 >= interVar.Item2) Console.WriteLine("a >= b, чего не должно быть. Введите парметры заново:");
                else break;
            }
            Comp.calc_int(methVar, funcVar, interVar);
        }
        public static void cmd_calc_impop()
        {
            var method = SubCommands.chooseVariant(VariantsMaps.integralMethodsVariants, "метод");
            IntMethodsVariants methVar;
            bool method_present = method.get(out methVar);
            if (!method_present)
            {
                Console.WriteLine("Операция отменена!");
                return;
            }
            var func = SubCommands.chooseVariant(VariantsMaps.impopVariants, "функцию");
            ImpopFuncVariants funcVar;
            bool func_present = func.get(out funcVar);
            if (!func_present)
            {
                Console.WriteLine("Операция отменена!");
                return;
            }
            (double, double, double) interVar;
            while (true)
            {
                var intervals = SubCommands.chooseIntervals();
                bool intervals_present = intervals.get(out interVar);
                if (!intervals_present)
                {
                    Console.WriteLine("Операция отменена!");
                    return;
                }
                var intervalsValidator = VariantsMaps.impopVariantsIntervalsValidators[funcVar];
                bool internalValidation = intervalsValidator.Invoke(interVar.Item1, interVar.Item2);
                if (!internalValidation) Console.WriteLine("Введенные параметры не попадают в ОДЗ, введите заново:");
                else if (interVar.Item1 >= interVar.Item2) Console.WriteLine("a >= b, чего не должно быть. Введите парметры заново:");
                else break;
            }
            Comp.calc_impop(methVar, funcVar, interVar);
        }
    }
}
