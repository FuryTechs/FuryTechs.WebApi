using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuryTechs.WebApi.Example.Models.Database;

namespace FuryTechs.WebApi.Example.Models.Dto
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public User Sender { get; set; }
    }
}