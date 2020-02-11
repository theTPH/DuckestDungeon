class Log
{
	public static log4net.ILog log { get; private set; } = log4net.LogManager.GetLogger("main");
}