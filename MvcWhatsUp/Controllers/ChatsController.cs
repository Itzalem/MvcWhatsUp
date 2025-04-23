using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.ViewModels;
using MvcWhatsUp.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace MvcWhatsUp.Controllers
{
	public class ChatsController : Controller
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IChatsRepository _chatsRepository;


		public ChatsController(IUsersRepository usersRepository, IChatsRepository chatsRepository)
		{
			_usersRepository = usersRepository;
			_chatsRepository = chatsRepository;
		}

		public IActionResult IndexChats()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AddMessage(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("IndexUsers", "Users");
			}

			//get logged in user id via cookie
			string? loggedInUserId = Request.Cookies["UserId"];

			if (loggedInUserId == null)
			{
				return RedirectToAction("IndexUsers", "Users");
			}

			User? receiverUser = _usersRepository.GetById((int) id);
			ViewData["ReceiverUser"] = receiverUser;

			Message message = new Message();
			message.SenderUserId = int.Parse(loggedInUserId);
			message.ReceiverUserId = (int)id;

			return View(message);
		}

		[HttpPost]
		public IActionResult AddMessage(Message message)
		{
			try
			{
				message.SendAt = DateTime.Now;
				_chatsRepository.AddMessage(message);

				TempData["ConfirmMessage"] = "Message has been sent correctly";

				return RedirectToAction("DisplayChats", new { id = message.ReceiverUserId }); 
			}
			catch (Exception ex)
			{
				return View(message);
			}

		}

		[HttpGet]
		public IActionResult DisplayChats(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("IndexUsers", "Users");
			}

			//get logged in user id via cookie
			string? loggedInUserId = Request.Cookies["UserId"];
			if (loggedInUserId == null)
			{
				return RedirectToAction("IndexUsers", "Users");
			}

			//get users via repository
			User? receivingUser = _usersRepository.GetById((int.Parse(loggedInUserId)));
			User? sendingUser = _usersRepository.GetById((int)id);

			if ((sendingUser == null) || (receivingUser == null))
			{
				return RedirectToAction("IndexUsers", "Users");
			}

			List <Message> chatMessages = _chatsRepository.GetAllMessages(sendingUser.UserId, 
																	receivingUser.UserId);

			ChatViewModel chatViewModel = new ChatViewModel(chatMessages, sendingUser, receivingUser);

			return View(chatViewModel);
		}
	}
}
