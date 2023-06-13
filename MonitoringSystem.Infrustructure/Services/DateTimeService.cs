using MonitoringSystem.Application.Common.Interfaces;

namespace MonitoringSystem.Infrustructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;

    }
}
