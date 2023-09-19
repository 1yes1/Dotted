using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotted
{
    public interface IObjectPool
    {
        void CreateObjects(int count);
    }
}
