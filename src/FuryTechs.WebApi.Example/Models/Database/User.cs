using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FuryTechs.WebApi.Example.Models.Dto;
using Microsoft.OData.Edm;

namespace FuryTechs.WebApi.Example.Models.Database
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<Message> SentMessages { get; set; }

        public ICollection<MessageRecipient> ReceivedMessages { get; set; }
    }
}
