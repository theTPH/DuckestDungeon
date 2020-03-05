using Godot;
using log4net.Appender;

using log4net.Core;

namespace Logging
{
	public class GodotAppender : ConsoleAppender
	{
		override protected void Append(LoggingEvent loggingEvent) 
			{
	#if NETCF_1_0
				// Write to the output stream
				GD.Print(RenderLoggingEvent(loggingEvent));
	#else
				if (Target.Equals(ConsoleError))
				{
					// Write to the error stream
					GD.PrintErr(RenderLoggingEvent(loggingEvent));
				}
				else
				{
					// Write to the output stream
					GD.Print(RenderLoggingEvent(loggingEvent));
				}
	#endif
			}
	}

}