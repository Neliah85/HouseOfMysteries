using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseOfMysteries.DTOs;
using HouseOfMysteries.Models;

namespace HouseOfMysteries.Classes;

public class CustomToken
{
    public LoggedInUserDTO LoggedInUser {get; set; }
        
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
             
    public CustomToken(int expirationTime,LoggedInUserDTO loggedInUser) {
        LoggedInUser = loggedInUser;
        LoggedInUser.Token = Guid.NewGuid().ToString();
        ExpirationTime = expirationTime;
        RemainingTime = expirationTime;
        LoggedInUser = loggedInUser;
    }

    public CustomToken(Guid masterToken, int expirationTime, LoggedInUserDTO loggedInUser)
    {
        LoggedInUser = loggedInUser;
        LoggedInUser.Token = masterToken.ToString();
        ExpirationTime = expirationTime;
        RemainingTime = expirationTime;
        LoggedInUser = loggedInUser;
    }

    public override string ToString() { return $"{LoggedInUser}"; }


}
