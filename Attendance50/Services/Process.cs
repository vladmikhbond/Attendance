using Attendance50.Data;
using Attendance50.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Attendance50.Services
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
        public string[] GetPresentNames(IFormFile uploadedFile)
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

    }
}
