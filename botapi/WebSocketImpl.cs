using System;
using WebSocketSharp;

public class WebSocketImpl
{

	public WebSocketImpl(string adress = "ws://localhost:9080")
	{
		WebSocket ws = new WebSocket (adress);
		ws.OnMessage += (sender, e) =>
			Log.log.Info("message: " + e.Data);
		ws.OnClose += (sender, e) =>
			Log.log.Info("WS-Verbindung zu Server geschlossen");

		ws.Connect ();
		ws.Send ("BALUS");

		System.Threading.Thread.Sleep(500);
		ws.Close();
	}

}
