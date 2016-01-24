using UnityEngine;
using System.Text;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

public class global : MonoBehaviour
{
    static global()
    {
        ConfigureAllLogging();
    }

    public static string GameVersion = "0.2.9";
    public static int FrameRateLimit = 60;

    // Player Pref values
    public static string Music = "Music";
    public static string SoundEffects = "SoundEffects";
    public static string PlayerName = "PlayerName";
    public static string PlayerId = "PlayerId";

    private static void ConfigureAllLogging()
    {
        /*
        var xmlLayout = new XmlLayoutSchemaLog4j(true);

        xmlLayout.ActivateOptions();

        var fileAppender = new RollingFileAppender
        {
            LockingModel = new FileAppender.MinimalLock(),
            AppendToFile = true,
            File = @"logs\",
            DatePattern = "yyyyMMdd'.log'",
            Layout = xmlLayout,
            Encoding = Encoding.UTF8,
            MaxSizeRollBackups = 10,
            MaximumFileSize = "10MB",
            RollingStyle = RollingFileAppender.RollingMode.Date,
            StaticLogFileName = false,
        };

        fileAppender.ActivateOptions();
        */

        var patternLayout = new PatternLayout
        {
            ConversionPattern = "%date %-5level %logger - %message%newline"
        };
        patternLayout.ActivateOptions();

        // setup the appender that writes to Log\EventLog.txt
        var fileAppender = new RollingFileAppender
        {
            AppendToFile = false,
            File = @"logs\log.txt",
            Layout = patternLayout,
            MaxSizeRollBackups = 5,
            MaximumFileSize = "1GB",
            RollingStyle = RollingFileAppender.RollingMode.Size,
            StaticLogFileName = true
        };
        fileAppender.ActivateOptions();

        var unityLogger = new UnityAppender
        {
            Layout = new PatternLayout()
        };
        unityLogger.ActivateOptions();

        BasicConfigurator.Configure(unityLogger, fileAppender);
    }
}
