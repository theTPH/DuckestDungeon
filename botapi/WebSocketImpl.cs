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
			logging.Logger.logger.Info("message: " + e.Data);
		this.OnClose += (sender, e) =>
			logging.Logger.logger.Info("WS-Verbindung zu Server geschlossen");

		this.Connect ();
		this.Send ("BALUS");
	}

	void send(Message m)
	{
		//var json = new JavaScriptSerializer().Serialize(m);
		var json = JsonConvert.SerializeObject(m);
		this.Send(json);
	}
}
