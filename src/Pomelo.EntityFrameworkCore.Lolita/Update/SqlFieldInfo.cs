using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public class SqlFieldInfo
    {
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
    }
}
