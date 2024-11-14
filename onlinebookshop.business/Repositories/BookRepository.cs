using Helper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace onlinebookshop.business
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;
        SqlDataReader reader = null;
        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Book> getBookLists()
        {
            List<Book> lstBooks = null;            
            try
            {                
                using (connection = new SqlConnection(
                   _configuration.GetConnectionString("LocalDBAppCon").ToString()))
                {
                    connection.Open();
                    string queryString = "select * from books;";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    using (reader = command.ExecuteReader())
                    {
                        lstBooks = new List<Book>();
                        while (reader.Read())
                        {
                            lstBooks.Add(new Book
                            {
                                Title = DatabaseHelper.GetString(reader["Title"].ToString()),
                                Author = DatabaseHelper.GetString(reader["Author"].ToString()),
                                ISBN = DatabaseHelper.GetString(reader["ISBN"].ToString()),
                                Genre = DatabaseHelper.GetString(reader["Genre"].ToString()),
                                IsOnRent = DatabaseHelper.GetBoolean(reader["IsOnRent"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }

            return lstBooks;
        }
    }
}