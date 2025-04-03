using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
