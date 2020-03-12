public class MessageCoins
{
	public string User {get; set;}
	public int Xp {get; set;}
	public int CoinsUsed {get; set;}

	override public string ToString()
	{
		return "user: " + User + ", xp: " + Xp + ", coins_used: " + CoinsUsed;
	}
}