using log4net;
using log4net.Repository;
using System;
using System.Diagnostics;

namespace Sheng.Kernal
{
    /// <summary>
    /// Critical  Error  Warning  Information  Verbose  ActivityTracing 严重程度依次递减
    /// </summary>
    public class LogService
    {
        private static LogService _instance;
        ILog _log;

        public static LogService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LogService();

                return _instance;
            }
        }

        private LogService()
        {

        }

        public void SetRepository(ILoggerRepository repository)
        {
            _log = LogManager.GetLogger(repository.Name, typeof(LogService));
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        /// <summary>
        /// 记录错误消息日志
        /// </summary>
        /// <param name="message">错误消息</param>
        public void Error(object message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// 记录错误消息+异常+请求参数日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="ex">Exception</param>
        /// <param name="requestParameters">请求参数</param>
        public void Error(object message, Exception ex, string requestParameters)
        {
            _log.Error(message + GetExceptionInfo(ex) + "——请求参数为：{0}" + requestParameters);
        }

        /// <summary>
        /// 记录错误消息+异常日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="ex">Exception</param>
        public void Error(object message, Exception ex)
        {
            _log.Error(message + GetExceptionInfo(ex));
        }

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        private string GetExceptionInfo(Exception ex)
        {
            return "——异常类型：" + ex.GetType() + Environment.NewLine
                + "——异常消息：" + ex.Message + Environment.NewLine
                + "——异常实例：" + ex.InnerException + Environment.NewLine
                + "——堆栈信息：" + ex.StackTrace + Environment.NewLine;
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
    }
}
