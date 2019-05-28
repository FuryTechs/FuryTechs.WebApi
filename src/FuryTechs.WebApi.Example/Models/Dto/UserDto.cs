using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FuryTechs.WebApi.Example.Models.Database;

namespace FuryTechs.WebApi.Example.Models.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }
}