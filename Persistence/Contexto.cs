using Microsoft.EntityFrameworkCore;

namespace Inveni.Persistence
{
    public class Contexto : DbContext
    {
        public Contexto() 
        {
        }
        public Contexto(DbContextOptions<Contexto> contextOptions) : base(contextOptions)
        {
        }
    }
}