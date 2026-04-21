namespace CoreApp.Exceptions;

public class ContactNotFoundException : Exception
{
    public ContactNotFoundException(string msg) : base(msg)
    {
    }
}