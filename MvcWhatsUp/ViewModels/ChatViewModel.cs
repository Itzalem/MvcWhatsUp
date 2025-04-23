using MvcWhatsUp.Models;

namespace MvcWhatsUp.ViewModels
{

    public class ChatViewModel
    {
        public List<Message> Messages { get; set; }
        public User SendingUser { get; set; }
        public User ReceivingUser { get; set; }
        public ChatViewModel(List<Message> messages, User sendingUser, User receivingUser)
        {
            Messages = messages;
            SendingUser = sendingUser; ReceivingUser = receivingUser;
        }
    }
}
