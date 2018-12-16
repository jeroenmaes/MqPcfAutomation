using System.Collections.Generic;
using System.Diagnostics;
using com.ibm.mq;
using com.ibm.mq.pcf;
using MqPcfAutomation.Models;

namespace MqPcfAutomation
{
    public class MqQueueManager : MqPcfManager
    {
        public MqQueueManager(string connection, string queueManager, string channel) : base(connection, queueManager, channel)
        {
        }
        /// <summary>
        /// Creates a local queue
        /// </summary>
        /// <param name="name">The name of the local queue</param>
        /// <param name="description">Option description of the queue</param>
        public void Create(string name, string description = "")
        {
            Trace.WriteLine($"Creating local queue {name} ...");

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_CREATE_Q);

                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_Q_NAME, name);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQIA_Q_TYPE, MQC.MQQT_LOCAL);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_Q_DESC, description);

                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQIA_MAX_Q_DEPTH, 100000);

                _agent.send(pcfCmd);
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }
        }

        /// <summary>
        /// Creates an alias queue
        /// </summary>
        /// <param name="name">The name of the alias queue</param>
        /// <param name="baseObject">The name of the local queue to use as the baseObject</param>
        /// <param name="clusterName">The name of the cluster this alias queue will be visible in</param>
        /// <param name="description">Option description of the queue</param>
        public void Create(string name, string baseObject, string clusterName, string description = "")
        {
            Trace.WriteLine($"Creating alias queue {name} for base object {baseObject} in cluster {clusterName} ...");

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_CREATE_Q);

                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_Q_NAME, name);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQIA_Q_TYPE, MQC.MQQT_ALIAS);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_BASE_OBJECT_NAME, baseObject);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_CLUSTER_NAME, clusterName);
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_Q_DESC, description);
                
                _agent.send(pcfCmd);
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }
        }

        public void Delete(string name)
        {
            Trace.WriteLine($"Deleting queue {name} ...");

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_DELETE_Q);

                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_Q_NAME, name);
                
                _agent.send(pcfCmd);
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }
        }

        public List<MqQueue> Inquire(string name)
        {
            var collection = new List<MqQueue>();

            try
            {
                PCFMessage pcfCmd = new PCFMessage(com.ibm.mq.constants.CMQCFC.MQCMD_INQUIRE_Q);

                
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQCA_Q_NAME, $"{name}*");
                pcfCmd.addParameter(com.ibm.mq.constants.CMQC.MQIA_Q_TYPE, MQC.MQQT_LOCAL);

                PCFMessage[] pcfResponse = _agent.send(pcfCmd);

                var names = new List<string>();

                for (int i = 0; i < pcfResponse.Length; i++)
                {
                    //https://www.ibm.com/support/knowledgecenter/en/SSFKSJ_7.5.0/com.ibm.mq.ref.adm.doc/q087800_.htm

                    string qName = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQC.MQCA_Q_NAME);
                    int depth = (int)pcfResponse[i].getIntParameterValue(com.ibm.mq.constants.CMQC.MQIA_CURRENT_Q_DEPTH);
                    int maxDepth = (int)pcfResponse[i].getIntParameterValue(com.ibm.mq.constants.CMQC.MQIA_MAX_Q_DEPTH);
                    int type = (int)pcfResponse[i].getIntParameterValue(com.ibm.mq.constants.CMQC.MQIA_Q_TYPE);
                    string baseObject = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQC.MQCA_BASE_OBJECT_NAME);
                    string description = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQC.MQCA_Q_DESC);
                    string clusterName = (string)pcfResponse[i].getParameterValue(com.ibm.mq.constants.CMQC.MQCA_CLUSTER_NAME);

                    collection.Add(new MqQueue { Name = qName, Depth = depth, MaxDepth = maxDepth, BaseObject = baseObject, Type = type, ClusterName = clusterName, Description = description});
                }

                return collection;
            }
            catch (MQException e)
            {
                Trace.WriteLine($"MQException::ReasonCode: {e.reasonCode} | {e.getMessage()}");
            }

            return collection;
        }
    }
}
