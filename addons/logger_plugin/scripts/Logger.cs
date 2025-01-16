#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fallen.LoggerSystem;


public static class Logger
{
	//this value should not be changed manually
	public enum LogLevel
	{
		info,
		warning,
		error,
	}

	private static string GetCallerInfo()
	{
		var stack = new StackFrame(2, true);
		return " || CALLER INFO: " + stack.GetFileName() + " " + stack.GetFileLineNumber().ToString();
	}

	public static void Log(LogLevel p_logLevel,  string logTag, params object[] what)
	{
		string callerInfo = "";


		if(SLoggerSystemBuddy.Instance.s_jsonMap.ContainsKey(logTag))
		{
			if(!SLoggerSystemBuddy.Instance.s_jsonMap[logTag].enabled)
				return;

			if (SLoggerSystemBuddy.Instance.s_jsonMap[logTag].verbose)
				callerInfo = GetCallerInfo();

			if(SLoggerSystemBuddy.Instance.s_jsonMap[logTag].logToFile)
			{
				LogToFile(SLoggerSystemBuddy.Instance.s_jsonMap);
				return;
			}
		}
		else
		{
			GD.Print("non existant tag: " + logTag);
			return;
		}

		

		
		object[] message = new object[what.Length + 1];

		for (int i = 0; i < message.Length - 1; i++)
		{
			message[i] = what[i];
		}

		message[message.Length - 1] = callerInfo;

		switch (p_logLevel)
		{
			case LogLevel.info:
				GD.Print(message);
				break;
			case LogLevel.warning:
				GD.PushWarning(message);
				break;
			case LogLevel.error:
				GD.PrintErr(message);
				break;

			default:
				GD.Print(what);
				break;
		}
	}

	public static void Log(params object[] what)
	{
		string callerInfo = "";

		object[] message = new object[what.Length + 1];

		for (int i = 0; i < message.Length - 1; i++)
		{
			message[i] = what[i];
		}

		message[message.Length - 1] = callerInfo;

		GD.Print(what);
	}

	private static void LogToFile(Dictionary<string, TagProfile> tagMap)
	{
		GD.Print("Logging to file");
	}
}
#endif

