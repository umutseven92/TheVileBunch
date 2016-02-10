﻿using UnityEngine;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

public class global : MonoBehaviour
{
    static global()
    {
        ConfigureAllLogging();
    }

    public static string GameVersion = "0.3.0";
    public static int FrameRateLimit = 60;

    // Player Pref values
    public static string Music = "Music";
    public static string SoundEffects = "SoundEffects";
    public static string PlayerName = "PlayerName";
    public static string PlayerId = "PlayerId";

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
}
