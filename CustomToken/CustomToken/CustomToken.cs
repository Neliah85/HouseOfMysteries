using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTokenTest
{
    public class CustomToken
    {
        public Guid Token { get; set; }
        public Guid UserId {get; private set; }
        public int Permission { get; private set; }
        public string UserName {get; private set; }

        int expirationTime;
        public int ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }
        public int RemainingTime {  get; private set; }

        public void ResetRemainingTime() 
        { 
            RemainingTime = ExpirationTime;            
        }

        public void DecreaseRemainingTime()
        {
            if (RemainingTime > 0)
            {
                RemainingTime--;
            }
        }
                 
        public CustomToken(Guid userId, int permission, string userName, int expirationTime) {
            Token = Guid.NewGuid();
            UserId = userId;
            Permission = permission;
            UserName = userName;
            ExpirationTime = expirationTime;
            RemainingTime = expirationTime;
        }

        public CustomToken(Guid masterToken,Guid userId, int permission, string userName, int expirationTime)
        {
            Token = masterToken;
            UserId = userId;
            Permission = permission;
            UserName = userName;
            ExpirationTime = expirationTime;
            RemainingTime = expirationTime;
        }

        public override string ToString() { return $"{Token};{UserId};{Permission};{UserName}"; }
    }
}
