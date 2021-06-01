
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService.Logic
{
	public abstract class MQConnectorBase: IDisposable
	{
		private readonly ConnectionFactory _factory;
		protected IConnection Connection { get; private set; }
		protected IModel Channel { get; private set; }
		protected readonly ILogger<MQConnector> logger;

		public MQConnectorBase(IOptions<ConnectionStrings> connectionStrings, ILogger<MQConnector> logger, IOptions<Credentials> mqCredentials)
		{
			this.logger = logger;

			_factory = new ConnectionFactory()
			{
				Endpoint = new AmqpTcpEndpoint(new Uri(connectionStrings.Value.MQ)),
				UserName = mqCredentials.Value.Username,
				Password = mqCredentials.Value.Password,
			};

			Connect();
		}

		protected bool Connect()
		{
			logger.LogDebug("connecting to MQ");
			try
			{
				Connection = _factory.CreateConnection();
				Connection.ConnectionShutdown += Connection_ConnectionShutdown;
				Channel = Connection.CreateModel();

				Channel.ExchangeDeclare("game", ExchangeType.Direct);
				var args = new Dictionary<string, object>
				{
					{ "x-message-ttl", 30000 }
				};
				Channel.QueueDeclare("game", durable: false, exclusive: false, autoDelete: false, arguments: args);
				Channel.QueueBind("game", "game", "game");
				logger.LogDebug("succesfully connected to MQ");

				return true;
			}
			catch (BrokerUnreachableException ex)
			{
				logger.LogError(ex, $"Could not connect to the service, '{ex.Message}' see {ex.HelpLink} for more details");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"An error occured while connecting to the service {ex.Message}");
			}
			return false;
		}

		private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
		{
			logger.LogInformation($"Connection to the MQ has been shutdown with reason: <{e.ReplyCode}> '{e.ReplyText}'");
			Connection.ConnectionShutdown -= Connection_ConnectionShutdown;

			Connect();
		}

		public void Dispose()
		{
			Connection.Dispose();
			Channel.Dispose();
		}
	}
}
