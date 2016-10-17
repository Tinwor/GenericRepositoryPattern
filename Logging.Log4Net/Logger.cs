using System;
using System.Globalization;
using log4net;

namespace Logging.Log4Net
{
    public class Logger : ILogger
    {
        #region Datamembers

        private static ILog log = null;

        #endregion Datamembers

        #region Class Initializer

        public Logger()
        {
            log = LogManager.GetLogger(typeof(Logger));
            GlobalContext.Properties["host"] = Environment.MachineName;
        }

        #endregion Class Initializer

        #region ILogger Members

        /// <summary>
        /// Enters the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        public void EnterMethod(string methodName)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format(CultureInfo.InvariantCulture, "Entering Method {0}", methodName));
                return;
            }

            if (log.IsInfoEnabled)
            {
                log.Info(string.Format(CultureInfo.InvariantCulture, "Entering Method {0}", methodName));
                return;
            }
        }

        /// <summary>
        /// Leaves the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        public void LeaveMethod(string methodName)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format(CultureInfo.InvariantCulture, "Leaving Method {0}", methodName));
                return;
            }

            if (log.IsInfoEnabled)
            {
                log.Info(string.Format(CultureInfo.InvariantCulture, "Leaving Method {0}", methodName));
                return;
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void LogException(Exception exception)
        {
            if (log.IsErrorEnabled)
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", exception.Message), exception);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogError(string message)
        {
            if (log.IsErrorEnabled)
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarningMessage(string message)
        {
            if (log.IsWarnEnabled)
                log.Warn(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        /// <summary>
        /// Logs the information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInfoMessage(string message)
        {
            if (log.IsInfoEnabled)
                log.Info(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogDebugMessage(string message)
        {
            if (log.IsDebugEnabled)
                log.Debug(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        #endregion ILogger Members
    }
}
