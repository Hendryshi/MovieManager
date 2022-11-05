namespace MovieManager.Core.Interfaces
{
	public interface IEmailSender
	{
		void SendEmail(string from, string to, string subject, string body);
	}
}