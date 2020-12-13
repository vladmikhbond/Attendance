using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class Process
    {
        public static Student[] Students { set; get; }

        public static void LoadData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            Students = lines.Select(l =>
            {
                var a = l.Split('\t');
                var groupName = new string(a[4].TakeWhile(c => c != '(').ToArray());
                return new Student { Name = a[0], Surname = a[1],  GroupName = groupName};
            }).ToArray();
        }

        string[] GetPresentNames(IFormFile uploadedFile)
        {
            //
            byte[] buffer = new byte[uploadedFile.Length];
            using (var f = uploadedFile.OpenReadStream())
            {
                f.Read(buffer);
            }
            string text = System.Text.Encoding.UTF8.GetString(buffer);

            //
            string sample = "<span class=\"ZjFb7c\">(.*?)<\\/span>";
            Regex regex = new Regex(sample, RegexOptions.Multiline);
            
            return regex.Matches(text)
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToArray();
        }

        public Student[] DoCheck(IFormFile uploadedFile)
        {
            var names = GetPresentNames(uploadedFile);

            var presentStudents = Students
                .Where(s => names.Contains($"{s.Name} {s.Surname}"))                
                .ToArray();

            var presentGroups = presentStudents
                .Select(s => s.GroupName)
                .Distinct().ToArray();

            var allStudents = Students
                .Where(s => presentGroups.Contains(s.GroupName))
                .Select(s => new Student
                {
                    Surname = s.Surname,
                    Name = s.Name,
                    GroupName = s.GroupName,
                    IsPresent = presentStudents.Contains(s)
                })
                .OrderBy(s => s.GroupName)
                .ThenBy(s => s.Surname)
                .ThenBy(s => s.Name)
                .ToArray();                

            return allStudents;            
        }

    }
}
