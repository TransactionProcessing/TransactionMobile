using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: dotnet run <path-to-test-dll>");
            return;
        }

        var path = args[0];
        var asm = Assembly.LoadFrom(path);

        foreach (var type in asm.GetTypes())
        {
            foreach (var method in type.GetMethods())
            {
                var testAttr = method.GetCustomAttribute<TestAttribute>();
                if (testAttr != null)
                {
                    var categories = method.GetCustomAttributes<CategoryAttribute>()
                        .Select(a => a.Name)
                        .ToList();

                    Console.WriteLine($"{type.FullName}.{method.Name} - Categories: {(categories.Count == 0 ? "None" : string.Join(", ", categories))}");
                }
            }
        }
    }
}