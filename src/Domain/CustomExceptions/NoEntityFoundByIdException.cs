namespace Domain.CustomExceptions;

public class NoEntityFoundByIdException : Exception
{
	public NoEntityFoundByIdException(string message) : base(message)
	{
	}
}
