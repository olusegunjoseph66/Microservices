using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utilities.Helpers
{
    public static class NumberManipulator
    {
        public static int PageCountConverter(int totalCount, int pageSize)
        {
            if (totalCount == 0)
                return 0;
            if (totalCount <= pageSize)
                return 1;

            int totalPages = 0;
            if(totalCount > pageSize)
            {
                if((totalCount % pageSize) == 0)
                    totalPages = totalCount / pageSize;
                else 
                    totalPages = (totalCount / pageSize) + 1;
            }

            return totalPages;
        }
    }
}
