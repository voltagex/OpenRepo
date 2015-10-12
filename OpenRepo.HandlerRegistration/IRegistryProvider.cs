using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OpenRepo.Handler
{
    public interface IRegistryProvider
    {
        bool SetHandlerForProtocol(HandlerRegistration handler);
        bool RemoveHandlerForProtocol(string ProtocolName);
    }
    
}
