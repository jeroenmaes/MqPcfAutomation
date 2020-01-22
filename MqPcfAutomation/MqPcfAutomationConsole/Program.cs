using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqPcfAutomation;

namespace MqClientTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Queue Demo
                var connection = "localhost";
                var queueManager = "QM1";
                var channel = "DEV.ADMIN.SVRCONN";
                var port = 1414;
                
                var qMgr = new MqQueueManager(connection, queueManager, channel, port);

                qMgr.Create("QL.Q1.AUTO_DEMO1");
                qMgr.Create("Q1.AUTO_DEMO1", "QL.Q1.AUTO_DEMO1", "CLUSTER1");

                var qResult = qMgr.Inquire("QL.Q1.AUTO");
                foreach (var q in qResult)
                {
                    Console.WriteLine($"Name: {q.Name} - BaseObject: {q.BaseObject} - ClusterName: {q.ClusterName}");
                }
                
                //qMgr.Delete("Q1.AUTO_DEMO1");
                //qMgr.Delete("QL.Q1.AUTO_DEMO1");
                
                // Subscription Demo
                var connection2 = "localhost";
                var queueManager2 = "QM1";
                
                var sMgr = new MqSubscriptionManager(connection2, queueManager2, channel, port);

                sMgr.Create("SUB.AUTODEMO", "/SU/AUTO_DEMO", "Q1.AUTO_DEMO1");

                var sResult = sMgr.Inquire("SUB.AUTODEMO");
                foreach (var sub in sResult)
                {
                    Console.WriteLine(sub.Name);
                }

                //sMgr.Delete("SUB.AUTODEMO");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
