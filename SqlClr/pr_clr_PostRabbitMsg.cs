using System.Text;
using RabbitMQ.Client;

namespace SqlClr
{
    public class RabbitMqSqlServer
    {
        public static void Pr_clr_PostRabbitMsg(string msg)
        {
            var factory = new ConnectionFactory { HostName = "localhost", Port = 32782, UserName = "admin", Password = "admin" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish("", "hello", null, body);
                }
            }
        }

    }
}
