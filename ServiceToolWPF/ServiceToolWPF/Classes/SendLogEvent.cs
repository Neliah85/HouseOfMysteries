namespace ServiceToolWPF.Classes
{
    public class SendLogEvent
    {        
        public event EventHandler<string>? LogSent;        
        public void SendLog(string message)
        { 
            LogSent?.Invoke(this, message);
        }
    }
}
