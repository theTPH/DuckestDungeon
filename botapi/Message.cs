public class Message
{
	public string user {get; set;}
	public int xp {get; set;}
	public int coins_used {get; set;}

	override public string ToString()
	{
		return "user: " + user + ", xp: " + xp + ", coins_used: " + coins_used;
	}
}