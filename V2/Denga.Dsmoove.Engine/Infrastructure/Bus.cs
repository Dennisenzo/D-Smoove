using System;
using System.Collections.Generic;
using System.Text;
using MemBus;
using MemBus.Configurators;

namespace Denga.Dsmoove.Engine.Infrastructure
{
    public static class Bus
    {
        public static IBus Instance { get; private set; }
        static Bus()
        {
            Instance = BusSetup.StartWith<Conservative>().Construct();
        }
    }
}
