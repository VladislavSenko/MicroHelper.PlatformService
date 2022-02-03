using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroHelper.PlatformService.Configuration
{
    public interface IAppConfiguration
    {
        public string CommandsServiceBaseUrl { get; }
    }
}
