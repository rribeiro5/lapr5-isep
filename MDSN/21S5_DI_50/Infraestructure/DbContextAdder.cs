using DDDSample1.Infrastructure.Provider;
using System;

namespace DDDSample1.Infrastructure
{
    public class DbContextAdder
    {
        private DbContextAdder() {}

        public static IDbProvider GetDbContextAdder(string className)
        {
            try
            {
                return (IDbProvider)Activator.CreateInstance(null, className).Unwrap();
            }
            catch (Exception)
            {
                // Default provider
                return (IDbProvider)new DbInMemory();
            }
            
        }
    }
}