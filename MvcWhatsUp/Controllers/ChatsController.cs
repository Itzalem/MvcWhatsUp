using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;

namespace MvcWhatsUp.Controllers
{
	public class ChatsController : Controller
	{
		public IActionResult IndexChats()
		{
			return View();
		}

		[HttpGet]
		public IActionResult SendMessage()
		{
			return View();
		}

		[HttpPost]
		public string SendMessage(Message message)
		{
			return $"Message {message.Text} has been sent by {message.Name}";
		}
	}
}
