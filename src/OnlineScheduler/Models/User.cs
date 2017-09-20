namespace OnlineScheduler.Models
{
    public class User
    {
        public int UserId { get; set;}
        public string UserName { get; set;}
        public DateTimeOffset RegisterDate { get; set;}
        public string EmailAddress { get; set;}
        public string PassWord { get; set;}
    }
}