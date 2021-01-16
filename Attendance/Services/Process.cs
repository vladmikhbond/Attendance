using Attendance.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        // Extract student nicks from HTML file.
        //
        string[] GetPresentNames(IFormFile uploadedFile)
        {
            byte[] buffer = new byte[uploadedFile.Length];
            using (var f = uploadedFile.OpenReadStream())
            {
                f.Read(buffer);
            }
            string text = System.Text.Encoding.UTF8.GetString(buffer);

            Regex regex = new Regex(_pattern, RegexOptions.Multiline);

            return regex.Matches(text)
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToArray();
        }

        // Call when a user upploads html file
        //
        public Student[] DoCheck(IFormFile uploadedFile, ApplicationDbContext db, string groupFilter)
        {
            // select present students
            string[] presentStudentNames = GetPresentNames(uploadedFile);

            var presentStudents = db.Students.Include(s => s.Group)
                 .Where(s => presentStudentNames.Contains(s.Nick))
                 .ToArray();

            // select actual group names
            string[] presentGroups;
            if (string.IsNullOrWhiteSpace(groupFilter))
            {
                presentGroups = presentStudents
                    .Select(s => s.Group.Name)
                    .Distinct().ToArray();
            } 
            else
            {
                Regex regex = new Regex(groupFilter);
                var v = db.Students
                    .Select(s => s.Group).Distinct()
                    .ToArray();
                presentGroups = v.Where(g => regex.IsMatch(g.Name)).Select(g => g.Name)
                    .ToArray();                
            }

            // students from checked groups with isPresent prop
            var checkedStudents = db.Students
                .Where(s => presentGroups.Contains(s.Group.Name))
                .ToArray();

            for (int i = 0; i < checkedStudents.Length; i++)
            {
                checkedStudents[i].IsPresent = presentStudents.Contains(checkedStudents[i]);
            }

            return checkedStudents
                .OrderBy(s => s.Group.Name)
                .ThenBy(s => s.Nick)
                .ToArray();
        }

    }
}
