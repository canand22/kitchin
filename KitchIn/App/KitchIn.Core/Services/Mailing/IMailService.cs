namespace KitchIn.Core.Services.Mailing
{
    public interface IMailService
    {
        string Send(string to, string message); 
    }
}