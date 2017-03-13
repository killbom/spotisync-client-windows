using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotiSync_Windows.Interfaces
{
    public interface IOptions
    {
        string serverUrl { get; }
        string socketUrl { get; }
    }
}
