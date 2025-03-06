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
				//loc 39-43 added to verify that the controller is recieving the data
				Console.WriteLine($"UserId: {user.UserId}");
				Console.WriteLine($"UserName: {user.UserName}");
				Console.WriteLine($"MobileNumber: {user.MobileNumber}");
				Console.WriteLine($"EmailAddress: {user.EmailAddress}");
				Console.WriteLine($"Password: {user.Password}");

				// add user via repository
				_usersRepository.Add(user);

				// loc 49-54 added to verify that the user is in the list 
				var usuarios = _usersRepository.GetAll();
				Console.WriteLine("Usuarios en la lista después de agregar:");
				foreach (var u in usuarios)
				{
					Console.WriteLine($"UserId: {u.UserId}, UserName: {u.UserName}, MobileNumber: {u.MobileNumber}, EmailAddress: {u.EmailAddress}, Password: {u.Password}");
				}

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
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


			var user = db.Users.Find(id);
			if (user == null)
				return HttpNotFound();


			return View(user);
		}

		// POST: Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone")] User user)
		{
			if (ModelState.IsValid)
			{
				db.Entry(user).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(user);
		}

		/* OLD CODE FOR EDIT!!!
		// POST: UsersController/Edit
		[HttpPost]
		public ActionResult Edit(User user)
		{
			try
			{
				// update user via repository
				_usersRepository.Update(user);

				// go back to users list (via Index)
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// something's wrong, go back to view with user
				return View(user);
			}
		}

		 */

		//NEW FEEDBACK CODE FOR DELETE!!!
		// GET: Users/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


			var user = db.Users.Find(id);
			if (user == null)
				return HttpNotFound();

			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var user = db.Users.Find(id);
			db.Users.Remove(user);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		/* OLD CODE BEFORE FEEDBACK FOR DELETE!!!
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
		public ActionResult Delete(User user)
		{
			try
			{
				// delete user via repository
				_usersRepository.Delete(user);

				// go back to user list (via Index)
				return RedirectToAction("IndexUsers");
			}
			catch (Exception ex)
			{
				// something's wrong, go back to view with user
				return View(user);
			}
		}
		*/
	}
}
