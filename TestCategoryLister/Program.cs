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
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                // Must be an [NUnit.Framework.Test] method
                var testAttr = method.GetCustomAttribute<TestAttribute>();
                if (testAttr is null)
                    continue;

                // Collect [Category] from method and class
                var methodCategories = method.GetCustomAttributes<CategoryAttribute>().Select(a => a.Name);
                var classCategories = type.GetCustomAttributes<CategoryAttribute>().Select(a => a.Name);

                var allCategories = methodCategories.Concat(classCategories).Distinct().ToList();

                Console.WriteLine($"{type.FullName}.{method.Name} - Categories: {(allCategories.Count == 0 ? "None" : string.Join(", ", allCategories))}");
            }
        }
    }
}