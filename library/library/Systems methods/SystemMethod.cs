using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.Systems_methods
{
    public interface SystemMethod
    {
        Vector Solve(SLAE system);
    }
}
