using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using StackExchange.Redis;

namespace LetsChess_GameService.Logic
{
	public class GameMoveStoreRedis: IMoveStore
	{
		readonly ConnectionMultiplexer muxer;
		private readonly IDatabase conn;
		private readonly ILogger<GameMoveStoreRedis> logger;

		public GameMoveStoreRedis(IOptions<ConnectionStrings> connectionStrings, ILogger<GameMoveStoreRedis> logger)
		{
			muxer = ConnectionMultiplexer.Connect(connectionStrings.Value.Redis);
			conn = muxer.GetDatabase();

			this.logger = logger;
		}

		public Task<bool> Add(string matchId, string from, string to) {
			if (string.IsNullOrEmpty(matchId)) {
				throw new ArgumentNullException(nameof(matchId));
			}
			if (string.IsNullOrEmpty(from))
			{
				throw new ArgumentNullException(nameof(from));
			}
			if (string.IsNullOrEmpty(to))
			{
				throw new ArgumentNullException(nameof(to));
			}
			var move = $"{from}-{to}";
			if (!muxer.IsConnected)
			{
				logger.LogWarning($"tried to add move '{move}', but the connection to redis could not been found");
				return Task.FromResult(false);
			}
			
			var success = conn.SetAddAsync(new RedisKey(matchId), new RedisValue(move));
			success.ContinueWith(s => {	
				logger.LogInformation(s.Result ? $"move stored in redis: '{move}'" : $"failed to store move in redis: '{move}'");
			});
			

			return success;
		}
	}
}
