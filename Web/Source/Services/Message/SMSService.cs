namespace Home.Source.Services.Message
{
    public class SMSService : IMessageService
    {
        public MessageServiceType MessageServiceType => MessageServiceType.SMS;

        public void SendMessage()
        {
            Console.WriteLine("Hi from SMS");
        }
    }
}
