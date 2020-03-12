public class MessageVote
{
	public string Option1 {get; set;}
	public string Option2 {get; set;}
	public bool Option1Chosen {get; set;}

	override public string ToString()
	{
		return "option1: " + Option1 + ", option2: " + Option2 + ", option1Chosen: " + Option1Chosen;
	}
}