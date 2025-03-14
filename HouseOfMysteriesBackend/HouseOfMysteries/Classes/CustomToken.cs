using HouseOfMysteries.DTOs;

namespace HouseOfMysteries.Classes;
public class CustomToken
{
    #region Declarations
    public LoggedInUserDTO LoggedInUser {get; set; }
        
    int expirationTime;
    public int ExpirationTime
    {
        get { return expirationTime; }
        set { expirationTime = value; }
    }
    public int RemainingTime {  get; private set; }
    #endregion
    #region methods
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
    #endregion
    #region Construktors             
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
    #endregion
    #region overrides
    public override string ToString() { return $"{LoggedInUser}"; }
    #endregion
}
