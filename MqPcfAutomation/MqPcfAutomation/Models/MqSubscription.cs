namespace MqPcfAutomation.Models
{
    public class MqSubscription
    {
        public string Name { get; set; }
        public string TopicName { get; set; }
        public string TopicString { get; set; }
        public string Destination { get; set; }
        public string Selector { get; set; }
    }
}
