using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FuryTechs.WebApi.Example.Models.Database;

namespace FuryTechs.WebApi.Example.Models.Dto
{
    public class MessageDto
    {
        [Key]
        public Guid MessageId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public UserDto Sender { get; set; }

        [ForeignKey(nameof(Sender))]
        public Guid SenderId { get; set; }
    }
}
