using log4net;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SampleLINQ
{
	public class MethodLog : IDisposable
	{
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly Action<string> _logAction;
		private readonly bool _noLogging;
		private readonly string _message;

		public ILog Logger { get; private set; }

		public enum LogLevel
		{
			None,
			Debug,
			Info,
		}

		public MethodLog(LogLevel logLevel = LogLevel.Debug, [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0) :
			this(LogManager.GetLogger(new StackTrace().GetFrame(1).GetMethod().DeclaringType), "", logLevel, methodName, lineNumber)
		{
		}

		public MethodLog(string extraInformation, LogLevel logLevel = LogLevel.Debug, [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0) :
			this(LogManager.GetLogger(new StackTrace().GetFrame(1).GetMethod().DeclaringType), extraInformation, logLevel, methodName, lineNumber)
		{
		}

		public MethodLog(ILog logger, LogLevel logLevel = LogLevel.Debug, [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0) :
			this(logger, "", logLevel, methodName, lineNumber)
		{
		}

		public MethodLog(ILog logger, string extraInformation, LogLevel logLevel = LogLevel.Debug,
			[CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
		{
			Logger = logger;
			if (logLevel == LogLevel.Debug && logger.IsDebugEnabled)
				_logAction = logger.Debug;
			else if (logLevel == LogLevel.Info && logger.IsInfoEnabled)
				_logAction = logger.Info;
			else
			{
				_noLogging = true;
				return;
			}
			_message = $"MethodLog:{methodName} Line:{lineNumber}";
			if (extraInformation.Length > 0)
				_message += $" ExtraInfo:{extraInformation}";
			var logMessage = $"{_message} (starting)";
			_logAction(logMessage);
			_stopwatch.Start();
		}

		public void Dispose()
		{
			if (_noLogging)
				return;
			_stopwatch.Stop();
			var logMessage = $"{_message} (complete) elapsed seconds:{_stopwatch.ElapsedMilliseconds / 1000D}";
			_logAction(logMessage);
		}
	}
}