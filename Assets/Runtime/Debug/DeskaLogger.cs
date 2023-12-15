using Unity.VisualScripting;
using UnityEngine;


	public class DeskaLogger
	{
		public void Log(string msg, LogLevel level)
		{
			Color logColor;

			switch (level)
			{
				case LogLevel.Verbose:
					logColor = Color.white;
					break;
				case LogLevel.Warn:
					logColor = Color.yellow;
					break;
				case LogLevel.Error:
					logColor = Color.red;
					break;
				case LogLevel.Critical:
					logColor = Color.magenta;
					break;
				default:
					logColor = Color.white;
					break;
			}
			Debug.Log($"<color=#{logColor.ToHexString()}>{msg}</color>");	
		}

		public void Log(string msg)
		{
			Log(msg, LogLevel.Verbose);
		}

		public enum LogLevel
		{
			Verbose,
			Warn,
			Error,
			Critical
		}
	}
