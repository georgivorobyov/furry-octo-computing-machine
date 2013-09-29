using Sumc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumc.Repositories
{
    public class AdminRepository : BaseRepository<Admin>
    {
        public Admin GetAdminByPassword(string cryptedPassword)
        {
            return this.dbSet.FirstOrDefault(x => x.Password == cryptedPassword);
        }
    }
}
