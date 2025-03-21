using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
