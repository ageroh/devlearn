using System.Text;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Admin.Services
{
    public class RabbitmqProvider
    {
        internal static Task<bool> Publish(Score score, string scoreQueue)
        {
            if(score == null)
            {
                return Task.FromResult(false);
            }

            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(scoreQueue, false, false, false, null);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new []{score}));
                    channel.BasicPublish("", scoreQueue, null, body);
                    return Task.FromResult(true);
                }
            }
            
        }
    }
}
