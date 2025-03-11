using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Org.BouncyCastle.Asn1.Icao;
using HouseOfMysteries.DTOs;
using HouseOfMysteries.Models;

namespace HouseOfMysteries.Classes;


public class TokenHolder
{
    public static List<CustomToken> tokens = new List<CustomToken>();
    public LoggedInUserDTO master = new LoggedInUserDTO();
        
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
    
    public string GetMasterToken() 
    {
        return tokens[0].LoggedInUser.Token;
    }

    public string GetUserToken(string userName)
    {
        int index = -1;
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].LoggedInUser.NickName.ToString() == userName)
            {
                index = i; break;
            }
        }
        if (index != -1)
        {
            return tokens[index].LoggedInUser.Token.ToString();
        }
        else 
        {
            return "";
        }
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

    public LoggedInUserDTO GenerateToken(int expirationTime, User loggedInUser)
    {
        LoggedInUserDTO newLoggedInUser = new LoggedInUserDTO();
        newLoggedInUser.UserId = loggedInUser.UserId;
        newLoggedInUser.RealName = loggedInUser.RealName;
        newLoggedInUser.NickName = loggedInUser.NickName;
        newLoggedInUser.Email = loggedInUser.Email;
        newLoggedInUser.Phone = loggedInUser.Phone;
        newLoggedInUser.TeamId = loggedInUser.TeamId;
        newLoggedInUser.RoleId = loggedInUser.RoleId;
        newLoggedInUser.Role = loggedInUser.Role;
        newLoggedInUser.Team = loggedInUser.Team;
        tokens.Add(new CustomToken(expirationTime, newLoggedInUser));
        return newLoggedInUser;   
    }

    public TokenHolder(bool singleUseToken, int interval)
    {
        SingleUseToken = singleUseToken;
        Interval = interval;        
        //master. = Guid.NewGuid().ToString();
        master.NickName = "master";
        master.RoleId = 4;
        master.RealName = "";
        master.Email = "";
        master.Phone = "";
        tokens.Add(new CustomToken(0,master));
    }

    public TokenHolder(Guid masterToken, bool singleUseToken, int interval)
    {
        SingleUseToken = singleUseToken;
        Interval = interval;
        //master.Id = Guid.NewGuid().ToString();
        master.NickName = "master";
        master.RoleId = 4;
        master.RealName = "";
        master.Email = "";
        master.Phone = "";
        tokens.Add(new CustomToken(masterToken,0,master));        
    }

    public CustomToken CheckTokenValidity(string token)
    {
        int index = -1;
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].LoggedInUser.Token.ToString() == token)
            {
                index = i; break;
            }
        }
        if (index != -1)
        {
            tokens[index].ResetRemainingTime();
            if (SingleUseToken == true && index!=0) { tokens[index].LoggedInUser.Token = Guid.NewGuid().ToString(); }
            return tokens[index];
        }
        else
        {
            return new CustomToken(new Guid(),-1,new LoggedInUserDTO());
        }
    }

    public void Logout(string userName) 
    {
        int index = -1;
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].LoggedInUser.NickName.ToString() == userName)
            {
                index = i; break;
            }
        }
        if (index != -1)
        {
            tokens.RemoveAt(index);
        }
    }

    ~TokenHolder()
    {
        aTimer?.Close();
    }

}
