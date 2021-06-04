
using LetsChess_GameService.Messages;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsChess_GameService.Logic
{
	public class MQConnector:MQConnectorBase
	{
		public MQConnector(IOptions<ConnectionStrings> connectionStrings, ILogger<MQConnector> logger, IOptions<Credentials> mqCredentials) : base(connectionStrings, logger, mqCredentials)
		{
		}

		
		public void TakeMove(TakeMoveMessage message)
		{
			var connected = Connection != default || Connection.IsOpen || Channel.IsOpen;
			if (!connected)
			{
				connected = Connect();
			}
			if (connected)
			{
				logger.LogDebug($"publishing takeMove for match {message.MatchId} of user {message.UserId} with move {message.From}->{message.To}");
				var body = JsonConvert.SerializeObject(message);
				Channel.BasicPublish(exchange: "game", routingKey: "game", body: Encoding.UTF8.GetBytes(body));
			}
		}
	}
}
