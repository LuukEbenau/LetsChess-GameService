using LetsChess_GameService.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService.Logic
{
	public class Game
	{
		private readonly MQConnector mQClient;
		private readonly IMoveStore moveStore;

		public Game(MQConnector mQClient, IMoveStore moveStore) {
			this.mQClient = mQClient;
			this.moveStore = moveStore;
		}
		public void TakeMove(string matchId, string userId, string from, string to) {
			//TODO: rabbitmq etc
			moveStore.Add(matchId, from, to);
			mQClient.TakeMove(new TakeMoveMessage { MatchId = matchId, UserId = userId, From = from, To = to });
		}
	}
}
