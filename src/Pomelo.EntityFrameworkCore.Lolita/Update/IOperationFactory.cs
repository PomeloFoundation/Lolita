using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public interface IOperationFactory
    {
        string TranslateToSql(OperationInfo operation);
    }
}
