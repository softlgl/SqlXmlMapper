using EFMigrate;
using EFMigrate.Models;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;
using SqlXmlMapper;
using System.Threading;
using SqlXmlTest.Models;
using EntityFramework;

namespace CoreMysql
{
    class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSqlXmlMapper("./SqlMapper");
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            using (StudentSystemContext studentContext = new StudentSystemContext())
            {
                studentContext.Database.Log = Console.Write;

                var groupListResult = studentContext.Database.Connection.Query<Program, StudentGroupDo>(serviceProvider, "GetStudentCount", new { Deleted = 0, NameCount = 1 });
                foreach (var item in groupListResult)
                {
                    Console.WriteLine($"{item.Id} {item.Name}");
                }

                var studentResult = studentContext.Database.Connection.Query<Program, StudentGroupDo>(serviceProvider, "GetStudents", new { Deleted = 0 });
                foreach (var item in studentResult)
                {
                    Console.WriteLine($"GetStudents:{item.Id} {item.Name}");
                }

                studentResult = studentContext.Database.Connection.Query<Program, StudentGroupDo>(serviceProvider, "GetStudents", new { Deleted = 0, Name = "小A" });
                foreach (var item in studentResult)
                {
                    Console.WriteLine($"GetStudentsWithName:{item.Id} {item.Name}");
                }

                var groupResult = studentContext.SqlQuery<Program, StudentGroupDo>(serviceProvider, "GetStudentCount", new { Deleted = 0, NameCount = 1 });
                foreach (var item in groupResult)
                {
                    Console.WriteLine($"{item.Id} {item.Name}");
                }
                Console.WriteLine();

                var studentResult2 = studentContext.SqlQuery<Program, StudentGroupDo>(serviceProvider, "GetStudents", new { Deleted = 0 });
                foreach (var item in studentResult2)
                {
                    Console.WriteLine($"GetStudents:{item.Id} {item.Name}");
                }
                Console.WriteLine();

                studentResult2 = studentContext.SqlQuery<Program, StudentGroupDo>(serviceProvider, "GetStudents", new { Deleted = 0, Name = "小A" });
                foreach (var item in studentResult2)
                {
                    Console.WriteLine($"GetStudentsWithName:{item.Id} {item.Name}");
                }
                Console.WriteLine();

                var grades = studentContext.SqlQuery<Grade>(serviceProvider, "CoreMysql.Program.Grade", "GetGrades", new { Deleted = 1, Ids = new[] { 1, 3 } });
                foreach (var item in grades)
                {
                    Console.WriteLine($"GetGrade:{item.Id} {item.Name}");
                }
                grades = studentContext.SqlQuery<Grade>(serviceProvider, "CoreMysql.Program.Grade", "GetGrades2", new { Deleted = 0 });
                foreach (var item in grades)
                {
                    Console.WriteLine($"GetGrade2:{item.Id} {item.Name}");
                }

                var stuGradeClasses = studentContext.SqlQuery<StudentGradeClassDo>(serviceProvider, "CoreMysql.Program.Grade", "GetGradeStudent", new { Deleted = 0, ClassId = 1, GradeId = 1 });
                foreach (var item in stuGradeClasses)
                {
                    Console.WriteLine(item);
                }

            }
            Console.ReadLine();
        }
    }
}
