using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using Fleck;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StateServer
{
    public class Program
    {
        private static readonly List<IWebSocketConnection> Sockets = new List<IWebSocketConnection>();
        private static readonly WebSocketServer Server = new WebSocketServer("ws://127.0.0.1:8181/socket");
        private static ConcurrentDictionary<int, Score> _state = new ConcurrentDictionary<int, Score>();

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var connectionString =  configuration.GetConnectionString("DefaultConnection");

            using (var con = new SqlConnection(connectionString))
            {
                var scores = con.Query<Score>(@"SELECT *
                FROM (
                    SELECT *,
                    ROW_NUMBER() OVER(PARTITION BY [EventId] ORDER BY Id DESC) AS RowNum
                    FROM [Scores]
                ) t
                WHERE t.RowNum = 1", commandType: CommandType.Text);
                _state = new ConcurrentDictionary<int, Score>(scores.ToDictionary(s => s.EventId)); 
            }

            Server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Connection open. {socket.ConnectionInfo.Id}");
                    Sockets.Add(socket);
                    var currentState = JsonConvert.SerializeObject(_state.Values);
                    socket.Send(currentState);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine($"Connection closed. {socket.ConnectionInfo.Id}");
                    Sockets.Remove(socket);
                };
            });


            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("Received: {0}", message);
                        Sockets.ForEach(socket => socket.Send(message));
                        
                        var scores = JsonConvert.DeserializeObject<List<Score>>(message);
                        scores.ForEach(score => _state[score.EventId] = score);
                    };
                    channel.BasicConsume("hello", true, consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }


        }
    }
}

