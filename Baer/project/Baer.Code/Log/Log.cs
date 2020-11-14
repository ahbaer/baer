using log4net;
using System;

namespace Baer.Code
{
    public class Log
    {
        private ILog logger;

        public Log(ILog log)
        {
            this.logger = log;
        }

        public void Info(object message)
        {
            string msg = string.Concat(
                "-----------------Info发生时间：" + DateTime.Now.ToString() + "-----------------",
                "\r\n",
                message,
                "\r\n\r\n\r\n");
            this.logger.Info(msg);
        }

        public void Debug(object message)
        {
            string msg = string.Concat(
                "-----------------Debug发生时间：" + DateTime.Now.ToString() + "-----------------",
                "\r\n",
                message,
                "\r\n\r\n\r\n");
            this.logger.Debug(msg);
        }

        public void Warn(object message)
        {
            string msg = string.Concat(
                "-----------------Warn发生时间：" + DateTime.Now.ToString() + "-----------------",
                "\r\n",
                message,
                "\r\n\r\n\r\n");
            this.logger.Warn(msg);
        }

        public void Error(object message)
        {
            string msg = string.Concat(
                "-----------------Error发生时间：" + DateTime.Now.ToString() + "-----------------",
                "\r\n",
                message,
                "\r\n\r\n\r\n");
            this.logger.Error(msg);
        }
    }
}
