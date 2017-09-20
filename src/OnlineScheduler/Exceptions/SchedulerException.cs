using System;

namespace OnlineScheduler.Exceptions
{
    public class SchedulerException : Exception
    {
        public SchedulerException() { }
        public SchedulerException(string message) : base(message) { }
        public SchedulerException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class EmailAlreadyExistsException : SchedulerException
    {
        public EmailAlreadyExistsException()
        : base("电子邮箱地址已存在！")
        { }
        public EmailAlreadyExistsException(Exception innerException)
        : base("电子邮箱地址已存在！", innerException)
        { }
    }

    public class PlanNotExistException : SchedulerException
    {
        public PlanNotExistException()
        : base("计划不存在！")
        { }
        public PlanNotExistException(Exception innerException)
        : base("计划不存在！", innerException)
        { }
    }

}