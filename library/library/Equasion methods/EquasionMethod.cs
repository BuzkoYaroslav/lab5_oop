using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.Equasion_methods
{
    public interface EquasionMethod
    {
        double Solve(Equasion eq, double a, double b);
    }
}
