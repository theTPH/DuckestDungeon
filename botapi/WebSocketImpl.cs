using System;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

public class WebSocketImpl : WebSocket
{
	private static WebSocketImpl instance = null;
	private SortedDictionary<int, Action<MessageVote>> voteMap = new SortedDictionary<int, Action<MessageVote>>();
	public static WebSocketImpl getInstance()
	{
		if (instance == null)
			instance = new WebSocketImpl();
		return instance;
	}
	public WebSocketImpl(string adress = "ws://localhost:9080") : base(adress)
	{
		this.OnMessage += (sender, e) =>
		{
			logging.Logger.logger.Debug("incoming Message: " + e.Data);
			string[] msg = System.Text.RegularExpressions.Regex.Split(e.Data, "##");
			switch(msg[0])
			{
				case "message_coins":
					MessageCoins messageCoins = JsonConvert.DeserializeObject<MessageCoins>(msg[1]);
				break;
				case "message_vote":
					MessageVote messageVote = JsonConvert.DeserializeObject<MessageVote>(msg[1]);

					int hash = (messageVote.option1 + messageVote.option2).GetHashCode();
					if (voteMap.ContainsKey(hash))
					{
						voteMap[hash].Invoke(messageVote);
						voteMap.Remove(hash);
					}
					else
						logging.Logger.logger.Warn("Konnte kein callback fuer vote " + messageVote.ToString() + " finden");
				break;
				default:
					logging.Logger.logger.Warn("Could not parse Message Object: " + e.Data);
				break;
			}
		};
		this.OnClose += (sender, e) =>
			logging.Logger.logger.Info("WS-Verbindung zu Server geschlossen");

		this.Connect ();

		// Beispiel send
		MessageVote message = new MessageVote();
		message.option1 = "erste option";
		message.option2 = "zweite option";
		this.send(message);

	}

	private void send(Object m)
	{
		string prefix = "";
		if (m is MessageCoins)
			prefix = "message_coins";
		else
			prefix = "message_vote";
		//var json = new JavaScriptSerializer().Serialize(m);
		var json = prefix + "##" + JsonConvert.SerializeObject(m);
		logging.Logger.logger.Info("Sending Message: " + json);
		this.Send(json);
	}

	public void send(MessageVote vote, Action<MessageVote> callback)
	{
		int hash = (vote.option1 + vote.option2).GetHashCode();
		this.voteMap[hash] = callback;
		this.send(vote);
	}
}
