using MicroHelper.Common.Emuns;

namespace MicroHelper.Common.Dtos
{
    public class PlatformPublishedToBusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MessageBusEventType EventType { get; set; }
    }
}
