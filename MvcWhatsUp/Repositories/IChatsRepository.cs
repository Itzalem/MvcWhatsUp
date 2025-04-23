using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories
{
    public interface IChatsRepository
    {
        void AddMessage (Message message);
        List <Message> GetAllMessages (int userId1, int userId2);   
    }
}
