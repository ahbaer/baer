/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using log4net;
using System;

namespace NFine.Code
{
    public class Log
    {
        private ILog logger;

        public Log(ILog log)
        {
            this.logger = log;
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

        public void Warn(object message)
        {
            string msg = string.Concat(
                "-----------------Warn发生时间：" + DateTime.Now.ToString() + "-----------------",
                "\r\n",
                message,
                "\r\n\r\n\r\n");
            this.logger.Warn(msg);
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
    }
}
