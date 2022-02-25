using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.SeedHandler
{
    public interface ISeedInitializer
    {
        void ExecuteSeed();
    }
}
