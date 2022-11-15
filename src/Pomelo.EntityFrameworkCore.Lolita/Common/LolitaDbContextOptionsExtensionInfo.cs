using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pomelo.EntityFrameworkCore.Lolita.Common
{
    public class LolitaDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        private readonly string logFragment;
        private int hashCode;

        public LolitaDbContextOptionsExtensionInfo(
            IDbContextOptionsExtension extension, 
            string logFragment,
            int hashCode) : base(extension)
        {
            this.logFragment = logFragment;
            this.hashCode = hashCode;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => logFragment;

        public override int GetServiceProviderHashCode() => hashCode;

        public override void PopulateDebugInfo([NotNull] IDictionary<string, string> debugInfo)
        {

        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => false;
    }
}
