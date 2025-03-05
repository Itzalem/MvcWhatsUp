namespace MvcWhatsUp.Models
{
	public class Message
	{
		public string Name { get; set; }
		public string Text { get; set; }

		public Message()
		{
			Name = "";
			Text = "";
		}

		public Message(string name, string message)
		{
				Name = name;
				Text = message;
		}
		
	}
}
