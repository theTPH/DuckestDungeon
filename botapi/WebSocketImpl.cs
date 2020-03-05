using System;
using WebSocketSharp;
using Newtonsoft.Json;

public class WebSocketImpl : WebSocket
{
	private static WebSocketImpl instance = null;
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
			Message m = JsonConvert.DeserializeObject<Message>(e.Data);
			logging.Logger.logger.Info("incoming Message: " + m.ToString());
		};
		this.OnClose += (sender, e) =>
			logging.Logger.logger.Info("WS-Verbindung zu Server geschlossen");

		this.Connect ();

		// Beispiel send
		Message message = new Message();
		message.coins_used = 5;
		message.user = "hallo";
		message.xp = 5664;
		this.send(message);
	}

	void send(Message m)
	{
		//var json = new JavaScriptSerializer().Serialize(m);
		var json = JsonConvert.SerializeObject(m);
		logging.Logger.logger.Info("Sending Message: " + json);
		this.Send(json);
	}
}
