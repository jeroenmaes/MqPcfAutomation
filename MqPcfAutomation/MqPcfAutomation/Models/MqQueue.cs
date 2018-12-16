namespace MqPcfAutomation.Models
{
    public class MqQueue
    {
        public string Name { get; set; }
        public string BaseObject { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string ClusterName { get; set; }
        public int Depth { get; set; }
        public int MaxDepth { get; set; }
    }
}
