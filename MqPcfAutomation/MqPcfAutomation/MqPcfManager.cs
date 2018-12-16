using com.ibm.mq.pcf;

namespace MqPcfAutomation
{
    public class MqPcfManager
    {
        internal readonly PCFMessageAgent _agent;

        public MqPcfManager(string connection, string queueManager, string channel)
        {
            _agent = MqHelper.CreatePcfAgent(connection, queueManager, channel);
        }
    }
}