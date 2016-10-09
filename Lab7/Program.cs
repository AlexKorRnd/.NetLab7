using System;
using System.Collections.Generic;
using System.IO;
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
            //ReflLab.ShowMethodsWithString("Lab7.Test");
            ReflLab.Execute("Lab7.Test", "Test2", "test.txt");

            Console.ReadLine();
        }
    }

    class ReflLab
    {
        public static void ShowMethodsWithString(string className)
        {
            try
            {
                List<MethodInfo> methods = GetMembersWithStringParam(Type.GetType(className, true));
                if (methods.Count == 0)
                {
                    Console.WriteLine("Методов с параметром String не найдено!");
                }
                foreach (MethodInfo method in methods)
                {
                    Console.WriteLine(ConstructFullMethodName(method));
                }
            }
            catch (TypeLoadException exception)
            {
                Console.WriteLine("Класс с именем \"" + className + "\" не найден! " + exception.Message);
            }
        }

        public static void Execute(string className, string methodName, string paramFile)
        {
            // Read the file and display it line by line.
            StreamReader file = null;
            try
            {
                Type type = Type.GetType(className);
                MethodInfo method = findMethod(type, methodName);
                file = new StreamReader("test.txt");
                Object[] paramsValues = new Object[method.GetParameters().Length];
                string line;
                int index = 0;
                while ((line = file.ReadLine()) != null)
                {
                    ParameterInfo parameterInfo = method.GetParameters()[index];
                    paramsValues[index++] = parseString(parameterInfo, line);
                }
                Object obj = type.InvokeMember(null,
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);

                type.InvokeMember(methodName, BindingFlags.Public
                                              | BindingFlags.Instance
                                              | BindingFlags.InvokeMethod, null, obj, paramsValues);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                file?.Close();
            }
        }

        private static Object parseString(ParameterInfo parameterInfo, String line)
        {
            Object value = null;
            if (parameterInfo.ParameterType == typeof(bool))
            {
                value = Boolean.Parse(line);
            }
            else if (parameterInfo.ParameterType == typeof(int))
            {
                value = Int32.Parse(line);
            }
            else if (parameterInfo.ParameterType == typeof(Double))
            {
                value = Double.Parse(line);
            }
            return value;
        }

        private static MethodInfo findMethod(Type type, string methodName)
        {
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                if (methodInfo.Name.Equals(methodName))
                {
                    return methodInfo;
                }
            }
            return null;
        }

        private static bool HasStringParam(MethodInfo method)
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

        private static List<MethodInfo> GetMembersWithStringParam(Type type)
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (MethodInfo method in type.GetMethods())
            {
                if (HasStringParam(method))
                {
                    methods.Add(method);
                }
            }
            return methods;
        }

        private static string ConstructFullMethodName(MethodInfo method)
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
        public void test4()
        {
            Console.WriteLine("test4");
        }

        public void Test2(bool value, int a, double b)
        {
            if (value)
            {
                Console.WriteLine("a = " + a);
                Console.WriteLine("a * 10 = " + (a*10));
            }
            Console.WriteLine("b = " + b);
            Console.WriteLine("b + 1.2 = " + (b+1.2));
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