using MonitoringSystem.Application.Common.Interfaces;

namespace MonitoringSystem.Infrustructure.Services
{
    public class GuidGeneratorService : IGuidGenerator
    {
        public Guid Guid => Guid.NewGuid();

    }
}
