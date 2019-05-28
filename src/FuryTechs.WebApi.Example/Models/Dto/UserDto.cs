using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FuryTechs.WebApi.Example.Models.Database;

namespace FuryTechs.WebApi.Example.Models.Dto
{
    public class UserDto
    {
        [Key]
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public List<MessageDto> Outbox { get; set; }

    }
}