using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            ReflLab.ShowMethodsWithString("Lab7.Test");

            Console.ReadLine();
        }
    }

    class ReflLab
    {
        public static void ShowMethodsWithString(String className)
        { 
            try
            {
                List<MethodInfo> methods = getMembersWithStringParam(Type.GetType(className, true));
                if (methods.Count == 0)
                {
                    Console.WriteLine("Методов с параметром String не найдено!");
                }
                foreach (MethodInfo method in methods)
                {
                    Console.WriteLine(constructFullMethodName(method));
                }
            }
            catch (TypeLoadException exception)
            {
                Console.WriteLine("Класс с именем \"" + className + "\" не найден! " + exception.Message);
            }
        
        }

        private static bool hasStringParam(MethodInfo method)
        {
            foreach (ParameterInfo parameterInfo in method.GetParameters())
            {
                if (parameterInfo.ParameterType == typeof(string))
                {
                    return true;
                }
            }
            return false;
        }

        private static List<MethodInfo> getMembersWithStringParam(Type type)
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (MethodInfo method in type.GetMethods())
            {
                if (hasStringParam(method))
                {
                    methods.Add(method);
                }
            }
            return methods;
        }

        private static string constructFullMethodName(MethodInfo method)
        {
            string fullMethodName = "";
            if (method.IsStatic)
                fullMethodName += "static ";
            if (method.IsVirtual)
                fullMethodName += "virtual ";
            fullMethodName += method.ReturnType.Name + " " + method.Name + " (";
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                fullMethodName += parameters[i].ParameterType.Name + " " + parameters[i].Name;
                if (i < parameters.Length - 1)
                    fullMethodName += ", ";
            }
            fullMethodName += ")";
            return fullMethodName;
        }
    }




    class Test
    {
        public void showString(String s)
        {
            
        }

        public void str0(int a, string s)
        {
            
        }

        public void tes2(bool a)
        {
            
        }

        public void str(string s1, string s2)
        {
        }
    }


    class Test2
    {
       

        public void tes2(bool a)
        {

        }

        public void str(string s1, string s2)
        {
        }
    }
}
