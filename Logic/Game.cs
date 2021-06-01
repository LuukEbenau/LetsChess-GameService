
using LetsChess_Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService.Logic
{
	public class Game
	{
		private readonly MQConnector mQClient;

		public Game(MQConnector mQClient) {
			this.mQClient = mQClient;
		}
		public void TakeMove(string matchId, string userId, string from, string to) {
			//TODO: rabbitmq etc
			mQClient.TakeMove(new TakeMoveMessage { MatchId = matchId, UserId = userId, From = from, To = to });
		}
	}
}
