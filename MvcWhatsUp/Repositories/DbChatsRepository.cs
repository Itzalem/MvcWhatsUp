using MvcWhatsUp.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace MvcWhatsUp.Repositories
{
	public class DbChatsRepository : IChatsRepository
	{
		private readonly string? _connectionString;

		public DbChatsRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("WhatsUpDatabase");
		}

		public void AddMessage(Message message)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = $"INSERT INTO Messages (SenderUserId, ReceiverUserId, MessageText, SendAt) " +
								$"VALUES (@SenderUserId, @ReceiverUserId, @MessageText, @SendAt); " +
								"SELECT SCOPE_IDENTITY();";  // Obtiene el último ID insertado

				SqlCommand command = new SqlCommand(query, connection);

				//for the injection thingy
				command.Parameters.AddWithValue("@SenderUserId", message.SenderUserId);
				command.Parameters.AddWithValue("@ReceiverUserId", message.ReceiverUserId);
				command.Parameters.AddWithValue("@MessageText", message.MessageText);
				command.Parameters.AddWithValue("@SendAt", message.SendAt);

				command.Connection.Open();
				//to get the identity value of the scope 
				object result = command.ExecuteScalar(); //no tengo idea de que es esto de aqui hasta abajo
				if (result != null)
				{
					message.MessageId = Convert.ToInt32(result);
				}
				else
				{
					throw new Exception("Adding message failed");
				}
			}

		}

		private Message ReadMessage(SqlDataReader reader)
		{
			
			int messageId = (int)reader["MessageId"];
			int senderUserId = (int)reader["SenderUserId"];
			int receiverUserId = (int)reader["ReceiverUserId"];
			string messageText = (string)reader["MessageText"];
			DateTime sendAt = (DateTime)reader["SendAt"];
			

			return new Message(messageId, senderUserId, receiverUserId, messageText, sendAt);
		}

		public List<Message> GetAllMessages(int userId1, int userId2)
		{
			List<Message> messages = new List<Message>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "SELECT MessageId, SenderUserId, ReceiverUserId, MessageText, SendAt " +
								"FROM Messages WHERE (SenderUserId = @UserId1 AND ReceiverUserId = @UserId2)" +
								"OR (SenderUserId = @UserId2 AND ReceiverUserId = @UserId1)" +
								"ORDER BY SendAt ASC";

				SqlCommand command = new SqlCommand(query, connection);

				command.Parameters.AddWithValue("@UserId1", userId1);
				command.Parameters.AddWithValue("@UserId2", userId2);

				command.Connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Message message = ReadMessage(reader);
					messages.Add(message);
				}
				reader.Close();
			}

			return messages;
		}

	}
}
