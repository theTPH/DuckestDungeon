public class MessageVote
{
	public string option1 {get; set;}
	public string option2 {get; set;}
	public bool option1Chosen {get; set;}

	override public string ToString()
	{
		return "option1: " + option1 + ", option2: " + option2 + ", option1Chosen: " + option1Chosen;
	}
}