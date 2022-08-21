using System;
using System.Text.RegularExpressions;
using log4net;

namespace AxGrid
{
    public class Log
    {
        public static ILog logger = LogManager.GetLogger("Ax");

        public static void Debug(string message)
        {
            if (logger.IsDebugEnabled)
                logger.Debug(Regex.Escape(message));
        }
        
        public static void Info(string message)
        {
            if (logger.IsInfoEnabled)
                logger.Info(Regex.Escape(message));
        }
        
        public static void Warn(string message)
        {
            if (logger.IsWarnEnabled)
                logger.Warn(Regex.Escape(message));
        }
        
        public static void Error(string message)
        {
            if (logger.IsErrorEnabled)
                logger.Error(Regex.Escape(message));
        }
        
        public static void Error(Exception ex)
        {
            if (logger.IsErrorEnabled)
                logger.Error(ex);
        }
        
        public static void Error(Exception ex, string message)
        {
            if (logger.IsErrorEnabled)
                logger.Error(Regex.Escape($"{message}: {ex.Message}\n{ex.StackTrace}"));
        }
    }
}