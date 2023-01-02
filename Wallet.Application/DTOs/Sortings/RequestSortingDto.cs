using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.DTOs.Sortings
{
    public class RequestSortingDto
    {
        public bool IsDateAscending { get; set; } = false;
        public bool IsDateDescending { get; set; } = false;
        public bool IsIDDescending { get; set; } = false;
    }
}
