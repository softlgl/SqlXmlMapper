using System;
using System.Collections.Generic;
using System.Text;

namespace SqlXmlTest.Models
{
    public class StudentGradeClassDo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string GradeName { get; set; }
        public override string ToString()
        {
            return $"学号=[{Id}],姓名=[{Name}],班级=[{ClassName}],年级=[{GradeName}]";
        }
    }
}
