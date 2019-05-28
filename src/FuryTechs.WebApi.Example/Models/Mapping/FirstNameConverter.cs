using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FuryTechs.WebApi.Example.Models.Mapping
{
    public class FirstNameConverter : IValueConverter<string, string>
    {
        /// <summary>
        /// Perform conversion from source member value to destination member value
        /// </summary>
        /// <param name="sourceMember">Source member object</param>
        /// <param name="context">Resolution context</param>
        /// <returns>Destination member value</returns>
        public string Convert(string sourceMember, ResolutionContext context)
        {
            var parts = sourceMember.Split(' ');
            return string.Join(" ", parts.Take(parts.Length - 1));
        }
    }
}