using Microsoft.EntityFrameworkCore;

namespace Pomelo.EntityFrameworkCore.Lolita
{
    public class LolitaValuing<TEntity, TProperty>
        where TEntity : class, new()
    {
        public LolitaSetting<TEntity> Inner { get; set; }

        public TService GetService<TService>() => Inner.Query.GetService<TService>();

        public string CurrentField { get; set; }
    }
}
