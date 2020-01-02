using System;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class AccessTokenBackupDto : IMapFrom<AccessToken>
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Token { get; set; }

        public DateTimeOffset? ExpirationDateTime { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public string Identifier { get; set; }
    }
}
