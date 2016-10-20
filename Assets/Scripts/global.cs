using log4net;
using UnityEngine;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

public class global : MonoBehaviour
{
	static global()
	{
		ConfigureAllLogging();
	}

	public static string GameVersion = "0.5.0";
	public static int FrameRateLimit = 60;

	// Player Pref values
	public static string Music = "Music";
	public static string SoundEffects = "SoundEffects";
	public static string PlayerName = "PlayerName";
	public static string PlayerId = "PlayerId";
	public static string FullScreen = "FullScreen";
	public static string Resolution = "Resolution";


	public enum Resolutions
	{
		r1920x1080 = 0,
		r1366x768 = 1,
		r1600x900 = 2,
		r1440x900 = 3,
		r1280x1024 = 4	
	}

	private static void ConfigureAllLogging()
	{
		var patternLayout = new PatternLayout
		{
			ConversionPattern = "%date %-5level %logger - %message%newline"
		};
		patternLayout.ActivateOptions();

		var fileAppender = new RollingFileAppender
		{
			AppendToFile = true,
			File = @"logs\log.log",
			Layout = patternLayout,
			MaxSizeRollBackups = 5,
			MaximumFileSize = "10MB",
			RollingStyle = RollingFileAppender.RollingMode.Size,
		};
		fileAppender.ActivateOptions();

		var unityLogger = new UnityAppender
		{
			Layout = new PatternLayout()
		};
		unityLogger.ActivateOptions();

		BasicConfigurator.Configure(unityLogger, fileAppender);
	}

	public static void LogDebug(ILog log, string message)
	{
		if (Debug.isDebugBuild)
		{
			log.Debug(message);
		}
	}

	public static void LogInfo(ILog log, string message)
	{
		if (Debug.isDebugBuild)
		{
			log.Info(message);
		}
	}

	public static void LogError(ILog log, string message)
	{
		if (Debug.isDebugBuild)
		{
			log.Error(message);
		}
	}
}
