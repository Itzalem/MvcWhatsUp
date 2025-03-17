using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories
{
	public interface IUsersRepository
	{
		List<User> GetAll();
		User GetById(int userId);
		User GetByLoginCredentials(string userName, string password);
		void Add(User user);
		void Update(User user);
		void Delete(User user);
	}
}
