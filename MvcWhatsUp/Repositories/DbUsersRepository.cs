using MvcWhatsUp.Models;
using Microsoft.Data.SqlClient;
using System.Net.Mail;

namespace MvcWhatsUp.Repositories
{
	public class DbUsersRepository : IUsersRepository
	{
		private readonly string? _connectionString;

		public DbUsersRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("WhatsUpDatabase");
		}

		private User ReadUser(SqlDataReader reader)
		{
			int id = (int)reader["UserId"];
			string name = (string)reader["UserName"];
			string mobileNumber = (string)reader["MobileNumber"];
			string emailAddress = (string)reader["EmailAddress"];
			string password = (string)reader["Password"];

			return new User(id, name, mobileNumber, emailAddress, password);
		}

		public List<User> GetAll()
		{
			// Crear una lista local para evitar acumulación de registros.
			List<User> users = new List<User>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE IsDeleted = 0;";
				SqlCommand command = new SqlCommand(query, connection);

				command.Connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					User user = ReadUser(reader);
					users.Add(user);
				}
				reader.Close();
			}
			return users;
		}


		public User? GetById(int userId)
		{
			//Este codifgo busca un usuario segun el id directamente en la base de datos y lo devuelve al final
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE " +
					"UserId = @Id AND IsDeleted = 0;";

				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Id", userId);

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					User user = ReadUser(reader); //convierte la fila de datos SQL en un user de vuelta 
					reader.Close();
					return user;
				}
				else
				{
					reader.Close();
					return null;
				}
			}
			/* Esto estaba buscando un usuario en la lista de memoria
			List<User> users = GetAll();
			return users.FirstOrDefault(u => u.UserId == userId);
			*/
		}

		public User? GetByLoginCredentials(string userName, string password)
		{
			//Este codifgo busca un usuario segun el id directamente en la base de datos y lo devuelve al final
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE UserName = @UserName AND Password = @Password";

				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@UserName", userName);
				command.Parameters.AddWithValue("@Password", password);

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					User user = ReadUser(reader); //convierte la fila de datos SQL en un user de vuelta 
					reader.Close();
					return user;
				}
				else
				{
					reader.Close();
					return null;
				}
			}
		}

		public void Add(User user)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = $"INSERT INTO Users (UserName, MobileNumber, EmailAddress, Password, IsDeleted) " +
								$"VALUES (@Name, @MobileNumber, @EmailAddress, @Password, 0); " +
								"SELECT SCOPE_IDENTITY();";  // Obtiene el último ID insertado
				SqlCommand command = new SqlCommand(query, connection);

				//for the injection thingy
				command.Parameters.AddWithValue("@Name", user.UserName);
				command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
				command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
				command.Parameters.AddWithValue("@Password", user.Password);

				command.Connection.Open();
				//to get the identity value of the scope 
				object result = command.ExecuteScalar(); //no tengo idea de que es esto de aqui hasta abajo
				if (result != null)
				{
					user.UserId = Convert.ToInt32(result);
				}
				else
				{
					throw new Exception("Adding user failed");
				}

				//esto me esta poniendo el usuario doble si lo dejo en el codigo, hace lo mismo que xevute scalar
				//user.UserId = Convert.ToInt32(command.ExecuteScalar()); //no tengo idea de loq eu dive aqui pero es para coger el user id y guardarlo en el usuario
				//int nrOfRowsAffected = command.ExecuteNonQuery();
				//if (nrOfRowsAffected != 1)
				//throw new Exception("Adding user failed");
			}
		}

		public void Update(User user)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = $"UPDATE Users SET UserName = @Name, MobileNumber = @Mobilenumber, " +
								"EmailAddress = @EmailAddress WHERE UserId = @Id";

				SqlCommand command = new SqlCommand(query, connection);

				//for the injection thingy
				command.Parameters.AddWithValue("@Id", user.UserId);
				command.Parameters.AddWithValue("@Name", user.UserName);
				command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
				command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

				//!POR QUE NO PONGO PASSWORD
				//command.Parameters.AddWithValue("@Password", user.Password);

				command.Connection.Open();

				int nrOfRowsAffected = command.ExecuteNonQuery();
				if (nrOfRowsAffected == 0)
					throw new Exception("No records updated");
			}
		}

		public void HardDelete(User user)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = $"DELETE FROM Users WHERE UserId = @Id";

				SqlCommand command = new SqlCommand(query, connection);

				//for the injection thingy
				command.Parameters.AddWithValue("@Id", user.UserId);

				//!POR QUE NO PONGO TODOS LOS DEMAS

				command.Connection.Open();

				int nrOfRowsAffected = command.ExecuteNonQuery();
				if (nrOfRowsAffected == 0)
					throw new Exception("No records HARD deleted");

			}
		}

		public void Delete(User user)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "UPDATE Users SET IsDeleted = 1 WHERE UserId = @Id";
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Id", user.UserId);

				connection.Open();
				int nrOfRowsAffected = command.ExecuteNonQuery();
				if (nrOfRowsAffected == 0)
					throw new Exception("No records deleted");
			}
		}

	}

}