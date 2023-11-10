namespace Home.Source.Services.Message
{
    public class EmailService : IMessageService
    {
        public MessageServiceType MessageServiceType => MessageServiceType.Email;

        public void SendMessage()
        {
            Console.WriteLine("Hi from Email");
        }
    }
}
