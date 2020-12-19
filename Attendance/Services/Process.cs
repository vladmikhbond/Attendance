using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Attendance.Models
{
    public class Process
    {
        private readonly string _pattern;

        public Process(string pattern)
        {
            _pattern = pattern;
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

            
            //string sample = "<span class=\"ZjFb7c\">(.*?)<\\/span>";
            Regex regex = new Regex(_pattern, RegexOptions.Multiline);

            return regex.Matches(text)
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToArray();
        }

        public Student[] DoCheck(IFormFile uploadedFile, Student[] Students)
        {
            var names = GetPresentNames(uploadedFile);

            var presentStudents = Students
                .Where(s => names.Contains($"{s.Name} {s.Surname}"))
                .ToArray();

            var presentGroups = presentStudents
                .Select(s => s.Group)
                .Distinct().ToArray();

            var allStudents = Students
                .Where(s => presentGroups.Contains(s.Group))
                .Select(s => new Student
                {
                    Surname = s.Surname,
                    Name = s.Name,
                    Group = s.Group,
                    IsPresent = presentStudents.Contains(s),
                    Id = s.Id
                })
                .OrderBy(s => s.Group)
                .ThenBy(s => s.Surname)
                .ThenBy(s => s.Name)
                .ToArray();

            return allStudents;
        }

    }
}
