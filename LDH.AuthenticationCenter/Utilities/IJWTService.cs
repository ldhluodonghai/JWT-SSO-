namespace LDH.AuthenticationCenter.Utilities
{
    public interface IJWTService
    {
        string GetToken(string name);
    }
}
