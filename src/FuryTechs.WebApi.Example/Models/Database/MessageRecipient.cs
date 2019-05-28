using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryTechs.WebApi.Example.Models.Database
{
    public class MessageRecipient
    {
        public Guid Id { get; set; }
        public User To { get; set; }
        public DateTimeOffset? Opened { get; set; }
        public Message Message { get; set; }
    }
}