using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FuryTechs.WebApi.Example.Models.Database
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid SenderId { get; set; }

        public User Sender { get; set; }

        public ICollection<MessageRecipient> Recipients { get; set; }
    }
}
