using LiteEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroBattleApp
{
    public class SeriLogger : ILogger
    {
        private Serilog.Core.Logger _logger;
        public SeriLogger(Serilog.Core.Logger logger)
        {
            _logger = logger;
        }
        public void Log(string log)
        {
            _logger.Information(log);
        }

        public void LogError(string log)
        {
           _logger.Error(log);
        }

        public void LogWarning(string log)
        {
           _logger.Warning(log);
        }
    }
}
