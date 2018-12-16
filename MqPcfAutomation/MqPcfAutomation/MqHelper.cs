using com.ibm.mq;
using com.ibm.mq.pcf;
using java.util;

namespace MqPcfAutomation
{
    public static class MqHelper
    {
        public static PCFMessageAgent CreatePcfAgent(string connection, string queueManager, string channel)
        {
            var qm = CreateMqQueueManager(connection, queueManager, channel);

            return new PCFMessageAgent(qm);
        }

        private static MQQueueManager CreateMqQueueManager(string connection, string queueManager, string channel)
        {
            var properties = new Hashtable();
            
            properties.put(MQC.CHANNEL_PROPERTY, channel);
            properties.put(MQC.HOST_NAME_PROPERTY, connection);

            return new MQQueueManager(queueManager, properties);
        }
    }
}
