using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService.Messages
{
	public class TakeMoveMessage
	{
		public string MatchId { get; set; }
		public string UserId { get; set; }
		public string From { get; set; }
		public string To { get; set; }
	}
}
