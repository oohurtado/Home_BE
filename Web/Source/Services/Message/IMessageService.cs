namespace Home.Source.Services.Message
{
    public interface IMessageService
    {
        MessageServiceType MessageServiceType { get; }
        void SendMessage();
    }

    public enum MessageServiceType
    {
        Email,
        SMS,
    }
}
