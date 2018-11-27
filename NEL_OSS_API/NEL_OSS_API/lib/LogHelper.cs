using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using System.Threading;

namespace NEL.helper
{
    /// <summary>
    /// 日志输出帮助类
    /// 
    /// </summary>
    class LogHelper
    {
        private static string repositoryName;
        private static ILog log;
        public static void initLogger(string config)
        {
            ILoggerRepository repository = LogManager.CreateRepository("LogHelper");
            XmlConfigurator.Configure(repository, new FileInfo(config));
            repositoryName = repository.Name;
            log = getLogger(typeof(LogHelper));
        }

        public static ILog getLogger(Type t)
        {
            return LogManager.GetLogger(repositoryName, t);
        }

        public static void debug(string fmt, object[] args = null)
        {
            if (args == null)
            {
                log.Debug(fmt);
                return;
            }
            log.DebugFormat(fmt, args);
        }
        public static void warn(string fmt, object[] args = null)
        {
            if (args == null)
            {
                log.Warn(fmt);
                return;
            }
            log.WarnFormat(fmt, args);
        }
        public static void error(string fmt, object[] args = null)
        {
            if (args == null)
            {
                log.Error(fmt);
                return;
            }
            log.ErrorFormat(fmt, args);
        }
        
        public static void printEx(Exception ex, bool flag=false)
        {
            string threadName = Thread.CurrentThread.Name;

            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append("*************************************** \n");
            msg.AppendFormat(" 异常发生时间： {0} \n", DateTime.Now);
            msg.AppendFormat(" 异常类型： {0} \n", ex.HResult);
            msg.AppendFormat(" 导致当前异常的 Exception 实例： {0} \n", ex.InnerException);
            msg.AppendFormat(" 导致异常的应用程序或对象的名称： {0} \n", ex.Source);
            msg.AppendFormat(" 引发异常的方法： {0} \n", ex.TargetSite);
            msg.AppendFormat(" 异常堆栈信息： {0} \n", ex.StackTrace);
            msg.AppendFormat(" 异常消息： {0} \n", ex.Message);
            
            warn("{0} failed, {1}\n", new object[] { threadName, msg.ToString() });
            if(flag)
            {
                warn(threadName + " exit");
            } else
            {
                warn(threadName + " continue");
            }
        }
        
    }
}
