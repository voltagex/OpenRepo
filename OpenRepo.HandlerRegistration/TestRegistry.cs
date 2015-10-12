using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRepo.Handler
{
    public class TestRegistry : IRegistryProvider
    {
        public List<HandlerRegistration> handlers;
        public TestRegistry()
        {
            handlers = new List<HandlerRegistration>();
        }
        public bool SetHandlerForProtocol(HandlerRegistration handler)
        {
            Trace.WriteLine(string.Format("Attempting to register as {0}:// handler", handler.ProtocolName));
            handlers.Add(handler);
            return true;
        }

        public bool RemoveHandlerForProtocol(string ProtocolName)
        {
            var target = handlers.FirstOrDefault(h => h.ProtocolName == ProtocolName);
            if (target != null)
            {
                handlers.Remove(target);
                return true;
            }

            return false;
        }

        public bool OpenURL(string URL)
        {
            var uri = new Uri(URL);
            var target = handlers.FirstOrDefault(h => h.ProtocolName == uri.Scheme);
            if (target == null)
            {
                Trace.WriteLine(string.Format("Could not find a handler for {0}", URL));
                return false;
            }

            Trace.WriteLine(string.Format("Launching {0} with the handler for {1} at {2}", URL, target.ProtocolName, target.HandlerPath));
            return true;
        }
    }
}
