using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.Repositories;
using System.Net;

namespace MvcWhatsUp.Controllers
{
	public class UsersController : Controller
	{
		private readonly IUsersRepository _usersRepository;

		public UsersController(IUsersRepository usersRepository)
		{
			_usersRepository = usersRepository;
		}

		public IActionResult IndexUsers()
		{
			//get logged in user id via cookie
			string? userId = Request.Cookies["UserId"];

			//pass the logged in user id to the view
			ViewData["UserId"] = userId;

			//get all users via repository
			List<User> users = _usersRepository.GetAll();
			// pass the list to the View
			return View(users);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}


		// POST: UsersController/Create
		[HttpPost]
		public ActionResult Create(User user)
		{
			try
			{
				// add user via repository
				_usersRepository.Add(user);


				// go back to user list (via Index)
				return RedirectToAction("IndexUsers");
			}
			catch (Exception ex)
			{
				// something's wrong, go back to view with user 

				return View(user);
			}
		}


		// GET: UsersController/Edit/5
		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			User? user = _usersRepository.GetById((int)id);
			return View(user);
		}

		// POST: Users/Edit/5
		[HttpPost]
		public IActionResult Edit(User user)
		{
			try
			{
				_usersRepository.Update(user);

				return RedirectToAction("IndexUsers");
			}
			catch (Exception ex)
			{
				return View(user);
			}
		}

				
		// GET: UsersController/Delete
		public ActionResult Delete(int? id)
		{
			if (id == null)
				return NotFound();

			// get user via repository
			User? user = _usersRepository.GetById((int)id);
			return View(user);
		}

		// POST: UsersController/Delete/
		[HttpPost]
		public IActionResult Delete(int UserId)
		{
			try
			{
				var user = _usersRepository.GetById(UserId);
				if (user == null)
				{
					return NotFound(); // Si el usuario no existe, devolver un error 404
				}

				_usersRepository.Delete(user); // Eliminar el usuario

				return RedirectToAction("IndexUsers"); // Volver a la lista
			}
			catch (Exception ex)
			{
				return BadRequest("Error eliminando usuario: " + ex.Message);
			}
		}


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
			//get user (from repository) matching username and password
			User? user = _usersRepository.GetByLoginCredentials(loginModel.UserName, loginModel.Password);
			if (user == null)
			{
				//bad login, go back to from
				ViewBag.ErrorMessage = "Bad User Name/Password Combination";
				return View(loginModel);
			}
			else
			{
				// use a cookie to remember logged in user
				Response.Cookies.Append("UserId", user.UserId.ToString());

				//redirect to list of users (via URL /Users/Index)
				return RedirectToAction("IndexUsers", "Users");
			}
        }

    }

}
