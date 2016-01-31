using System;
using UnityEngine;
using log4net.Appender;
using log4net.Core;

public class UnityAppender : AppenderSkeleton
{
    protected override void Append(LoggingEvent loggingEvent)
    {
        string main = string.Format("{0} {1} {2} - {3}", DateTime.Now, loggingEvent.Level.Name, loggingEvent.LoggerName, RenderLoggingEvent(loggingEvent));

        if (Level.Compare(loggingEvent.Level, Level.Error) >= 0)
        {
            // everything above or equal to error is an error
            Debug.LogError(main);
        }
        else if (Level.Compare(loggingEvent.Level, Level.Warn) >= 0)
        {
            // everything that is a warning up to error is logged as warning
            Debug.LogWarning(main);
        }
        else
        {
            // everything else we'll just log normally
            Debug.Log(main);
        }
    }
}
