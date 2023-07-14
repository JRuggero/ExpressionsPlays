using System;
using System.Linq.Expressions;
using System.Xml.Linq;

// See https://code-maze.com/dotnet-collections-ienumerable-iqueryable-icollection/
//     https://www.c-sharpcorner.com/UploadFile/tirthacs/expressionfunc-vs-func/
public class Program
{
    public static void Main()
    {
        Expression<Func<Student, bool>> isTeenagerExpr = s => s.Age > 12 && s.Age < 20;
        Func<Student, bool> isTeenagerFunc = s => s.Age > 12 && s.Age < 20;
        Predicate<Student> isTeenagerPredicate = s => s.Age > 12 && s.Age < 20;

        List<Student> students = new List<Student>()
        {
        new Student() { ID = 1, Name = "Steve", Age = 19 },
        new Student() { ID = 1, Name = "William", Age = 25 }
        };

        bool isTeenager;

        foreach(var student in students)
        {
            ShowResult(student, Condition(student, isTeenagerExpr));

            //ShowResult(student, Condition(student, isTeenAgerPredicate)); // Predicate is no castable as Funk<T,bool> neither as Expression<Funk<T,bool>>

            ShowResult(student, Condition(student, isTeenagerFunc));

            // ... Condition(student, s => s.Age > 12 && s.Age < 20); // Compiler error CS0121: Ambiguous method call
            ShowResult(student, Condition(student, (Expression<Func<Student, bool>>)(s => s.Age > 12 && s.Age < 20)));

            // ... Condition(student, s => s.Age > 12 && s.Age < 20); // Compiler error CS0121: Ambiguous method call
            ShowResult(student, Condition(student, (Func<Student, bool>)(s => s.Age > 12 && s.Age < 20)));
        }
    }

    private static void ShowResult(Student student, bool isTeenager)
    {
        Console.WriteLine($"{student} is {(isTeenager ? string.Empty : "not ")}teenager.");
    }

    public static bool Condition<T>(T @object, Expression<Func<T, bool>> expression)
    {
        Func<T, bool> function = expression.Compile();
        return Condition(@object, function);
    }

    public static bool Condition<T>(T @object, Func<T, bool> function)
    {
        bool result = function(@object);
        return result;

    }

void Prueba (IEnumerable<Student> students)
    {
        // IEnumerable.Where(Func<T,bool>)
        var studentsE = students.Where(x => x.ID > 10);
        // IQueryable.Where(Expression<Func<T,bool>>)
        var studentsQ = students.AsQueryable().Where(x => x.ID > 10);

    }
}

public class Student
{

    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Age})";
    }
}