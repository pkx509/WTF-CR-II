using log4net.Core;
using log4net.Layout;
using System;
using System.Text;
using System.Xml;

namespace DITS.HILI.Framework
{
    public enum LogLevelEnum
    {
        Critical,
        Error,
        Information,
        Warning
    }

    public class WMSXmlLayout : XmlLayoutBase
    {
        protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
        {
            writer.WriteStartElement("LogEntry");
            writer.WriteStartElement("Message");
            writer.WriteString(loggingEvent.RenderedMessage);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }

    public class Logging
    {
        private static log4net.ILog logger;
        public static void Log(Type e, LogLevelEnum logLevel, string methodName, Exception ex, string message = "")
        {
            Exception _ex = ExceptionHelper.ExceptionMessage(ex);
            StringBuilder _message = new StringBuilder();
            _message.AppendLine("Level : " + logLevel.ToString());
            _message.AppendLine("Time : " + DateTime.Now.ToString());
            _message.AppendLine("Assembly : " + e.Assembly);
            _message.AppendLine("Method : " + methodName);
            _message.AppendLine("System Message : " + _ex.Message);

            logger = log4net.LogManager.GetLogger(e);
            log4net.Config.XmlConfigurator.Configure();
            switch (logLevel)
            {
                case LogLevelEnum.Critical:
                    {
                        logger.Fatal(_message.ToString());
                        break;
                    }
                case LogLevelEnum.Error:
                    {
                        logger.Error(_message.ToString());
                        break;
                    }
                case LogLevelEnum.Information:
                    {
                        logger.Info(_message.ToString());
                        break;
                    }
                case LogLevelEnum.Warning:
                    {
                        logger.Warn(_message.ToString());
                        break;
                    }
                default:
                    break;
            }

        }

    }
}
