using System.Collections.Generic;
using System.Diagnostics;
using com.ibm.mq;
using com.ibm.mq.pcf;
using MqPcfAutomation.Models;

namespace MqPcfAutomation
{
    public class MqSubscriptionManager : MqPcfManager
    {
        public MqSubscriptionManager(string connection, string queueManager, string channel) : base(connection, queueManager, channel)
        {
            
        }

        public void Create(string name, string topicString, string destination)
        {
            Trace.WriteLine($"Creating subscription {name} for topicString {topicString} and destination {destination} ...");

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_CREATE_SUBSCRIPTION);

                pcfCmd.addParameter(com.ibm.mq.constants.CMQCFC.MQCACF_SUB_NAME, name);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_TOPIC_STRING, topicString);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQCFC.MQCACF_DESTINATION, destination);

                _agent.send(pcfCmd);
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }
        }

        public void Delete(string name)
        {
            Trace.WriteLine($"Deleting subscription {name} ...");

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_DELETE_SUBSCRIPTION);

                pcfCmd.addParameter(com.ibm.mq.constants.CMQCFC.MQCACF_SUB_NAME, name);

                _agent.send(pcfCmd);
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }
        }

        public List<MqSubscription> Inquire(string name)
        {
            var collection = new List<MqSubscription>();

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_INQUIRE_SUBSCRIPTION);
                
                pcfCmd.addParameter(com.ibm.mq.constants.CMQCFC.MQCACF_SUB_NAME, $"{name}*");
                
                PCFMessage[] pcfResponse = _agent.send(pcfCmd);
                
                for (int i = 0; i < pcfResponse.Length; i++)
                {
                    //https://www.ibm.com/support/knowledgecenter/en/SSFKSJ_7.5.0/com.ibm.mq.ref.adm.doc/q088050_.htm

                    string subName = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQCFC.MQCACF_SUB_NAME);
                    string topicString = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQC.MQCA_TOPIC_STRING);
                    string topic = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQC.MQCA_TOPIC_NAME);
                    string selector = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQCFC.MQCACF_SUB_SELECTOR);
                    string dest = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQCFC.MQCACF_DESTINATION);

                    collection.Add(new MqSubscription { Name = subName, TopicName = topic, TopicString = topicString, Selector = selector, Destination = dest});
                }
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }

            return collection;
        }
    }
}
