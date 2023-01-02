using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.DTOs.Events
{
    public class AccountsSapAccountCreatedMessage
    {
        public int SapAccountId { get; set; }
        public int UserId { get; set; }
        public string OldFriendlyName { get; set; }
        public string NewFriendlyName { get; set; }
        public string DistributorSapNumber { get; set; }
        public DateTime DateModified { get; set; }
        public string DistributorName { get; set; } = null!;
        public DateTime DateRefreshed { get; set; }
        public string CompanyCode { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public NameAndCode AccountType { get; set; }
    }
    public class NameAndCode
    {
        public NameAndCode(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}
