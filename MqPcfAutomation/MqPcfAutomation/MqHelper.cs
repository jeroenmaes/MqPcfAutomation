using com.ibm.mq;
using com.ibm.mq.pcf;
using java.util;

namespace MqPcfAutomation
{
    public static class MqHelper
    {
        public static PCFMessageAgent CreatePcfAgent(string connection, string queueManager, string channel, int port)
        {
            var qm = CreateMqQueueManager(connection, queueManager, channel, port);

            return new PCFMessageAgent(qm);
        }

        private static MQQueueManager CreateMqQueueManager(string connection, string queueManager, string channel, int port)
        {
            var properties = new Hashtable();
            
            properties.put(MQC.CHANNEL_PROPERTY, channel);
            properties.put(MQC.HOST_NAME_PROPERTY, connection);

            MQEnvironment.port = port;

            return new MQQueueManager(queueManager, properties);
        }
    }
}
