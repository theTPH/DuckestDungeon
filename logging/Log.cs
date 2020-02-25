class Log
{
	public static log4net.ILog log { get; private set; } = log4net.LogManager.GetLogger("main");
}

// Wird nur für WebSocketImpl wegen Nameclash von WebSocket benötigt
namespace logging
{
	class Logger
	{
		public static log4net.ILog logger {
			get
			{
				return Log.log;
			}
		}
	}
}