using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories
{
	public class DummyUsersRepository : IUsersRepository
	{
		List<User> users = [
			new User(1, "Peter Sauber", "06-12345678", "pete.sauber@gmail.com", "drs54j"),
			new User(2, "Bill Hodges", "06-44556677", "bill.hodges@gmail.com", "gdr58rf"),
			new User(3, "Morris Bellamy", "06-11228899", "morris.bellamy@gmail.com", "hyyfy"),
			new User(4, "Kira Shipkra", "06-2656653", "kira.shipkra@gmail.com", "udh5dkdj"),
			new User(5, "Paula Hops", "06-5591487", "paula.hops@gmail.com", "ks5dj8w8"),
		];

		public List<User> GetAll()
		{
			return users;
		}

		public User? GetById(int userId)
		{
			return users.FirstOrDefault(x => x.UserId == userId); //thats a LINQ query, you can also use a loop to fin the user //in final project we need to make the loop way
		}

		public void Add(User user)
		{
			users.Add(user);
		}

		public void Update(User user)
		{
			int index = 0;
			user.UserId = index;
			users[index] = new User(index, user.UserName, user.MobileNumber, user.EmailAddress, user.Password);
		}

		public void Delete(User user)
		{
			users.Remove(user);	
		}

	}
}
