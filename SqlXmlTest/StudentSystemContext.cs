using EFMigrate.Models;
using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMigrate
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class StudentSystemContext : DbContext
    {
        const string connStr = "server=127.0.0.1;user=root;database=StudentSystem;port=3306;password=123456;SslMode=None";
        public StudentSystemContext()
            :base(connStr)
        {

        }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<ClassGrade> ClassGrades { get; set; }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<StudentSystemContext>(null);
            DbInterception.Add(new CommandInterceptor());
            base.OnModelCreating(modelBuilder);
        }
    }
}
