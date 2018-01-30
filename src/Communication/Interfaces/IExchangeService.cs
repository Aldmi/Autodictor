using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.SerialPort;

namespace Communication.Interfaces
{
    public interface IExchangeService
    {
        void RunService(MasterSerialPort port);
    }
}
