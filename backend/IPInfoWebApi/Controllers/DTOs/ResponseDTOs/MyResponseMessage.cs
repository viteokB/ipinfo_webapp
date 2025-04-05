namespace IPInfoWebApi.Controllers.DTOs.ResponseDTOs;

public class MyResponseMessage
{
    public string Message { get; set; }
    
    public List<string> Errors { get; set; }

    public MyResponseMessage(string message) : this(message, new List<string>())
    {
    }

    public MyResponseMessage(string message, List<string> errors)
    {
        Message = message;
        Errors = errors;
    }
}