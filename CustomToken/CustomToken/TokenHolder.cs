using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CustomTokenTest
{
    public class TokenHolder
    {
        public List<CustomToken> tokens = new List<CustomToken>();

        public static System.Timers.Timer? aTimer;

        public bool AutoDecrease { get; private set; }
        public bool SingleUseToken { get; private set; }
        int interval;
        public int Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                if (interval == 0)
                {
                    aTimer?.Close();
                }
                else
                {
                    SetTimer(Interval);
                }
            }
        }

        public void SetTimer(int interval)
        {
            aTimer = new System.Timers.Timer(interval);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void OnTimedEvent(Object? source, ElapsedEventArgs e)
        {
            DecreaseExpirationTime();
        }

        public Guid GetMasterToken() 
        {
            return tokens[0].Token;
        }
        public void DecreaseExpirationTime()
        {
            if (tokens.Count > 0)
            {
                for (int i = tokens.Count - 1; i > 0; i--)
                {
                    if (tokens[i].RemainingTime > 0)
                    {
                        tokens[i].DecreaseRemainingTime();
                    }
                    else
                    {
                        tokens.Remove(tokens[i]);
                    }
                }
            }
        }

        public void GenerateToken(Guid userId, int level, string userName, int expirationTime)
        {
            tokens.Add(new CustomToken(userId, level, userName, expirationTime));
        }

        public TokenHolder(bool singleUseToken, int interval)
        {
            SingleUseToken = singleUseToken;
            tokens.Add(new CustomToken(Guid.NewGuid(), 9, "Master", 0));
            Interval = interval;
        }

        public TokenHolder(Guid masterToken, bool autoChange, int interval)
        {
            SingleUseToken = false;
            tokens.Add(new CustomToken(masterToken, Guid.NewGuid(), 9, "Master", 0));
            Interval = interval;
        }

        public CustomToken CheckTokenValidity(Guid token)
        {
            int index = -1;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Token == token)
                {
                    index = i; break;
                }
            }
            if (index != -1)
            {
                tokens[index].ResetRemainingTime();
                if (SingleUseToken == true && index!=0) { tokens[index].Token = Guid.NewGuid(); }
                return tokens[index];
            }
            else
            {
                return new CustomToken(new Guid(), -1, "", 0);
            }
        }

        ~TokenHolder()
        {
            aTimer?.Close();
        }

    }
}
