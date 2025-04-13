namespace ServiceToolWPF.Classes
{
    public class RefreshEvent
    {
        public event EventHandler<string>? Refreshed;
        public void Refresh(string message)
        {
            Refreshed?.Invoke(this, message);
        }
    }
}
