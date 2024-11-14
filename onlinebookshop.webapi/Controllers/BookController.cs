using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlinebookshop.business;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace onlinebookshop.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    { 
        private readonly ILogger<BookController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBookServices _bookServices;
        private readonly IBookRepository _bookRepository;

        public BookController(ILogger<BookController> logger, IConfiguration configuration,
            IBookServices bookServices, IBookRepository bookRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _bookServices = bookServices;
            _bookRepository = bookRepository;
        }

        private void SetDependencies()
        {
            _bookServices.SetDependencies(_bookRepository, _logger);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult getBookLists()
        {
            List<Book> lstBooks = null;
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                SetDependencies();
                lstBooks = _bookServices.getBookLists();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Error in executing API");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }

            return Ok(lstBooks);
        }

        [HttpGet]
        [Route("byName")]
        public IActionResult getBookByName(string name)
        {
            Book book = new Book();
            SqlConnection connection = null;
            SqlDataReader reader = null;
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Bad Request");
            }
            try
            {
                using (connection = new SqlConnection(
                   _configuration.GetConnectionString("LocalDBAppCon").ToString()))
                {
                    connection.Open();
                    string queryString = "select * from books where title like @bookName";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@bookName", "%" + name + "%");
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            book.Title = DatabaseHelper.GetString(reader["Title"].ToString());
                            book.Author = DatabaseHelper.GetString(reader["Author"].ToString());
                            book.ISBN = DatabaseHelper.GetString(reader["ISBN"].ToString());
                            book.Genre = DatabaseHelper.GetString(reader["Genre"].ToString());
                            book.IsOnRent = DatabaseHelper.GetBoolean(reader["IsOnRent"]);
                        }
                    }
                }         
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Error in executing API");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }
            
            return Ok(book);
        }

        [HttpPost]
        [Route("rentBook")]
        public IActionResult rentBook(RentBook rentBook)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            string message = string.Empty;
            if (rentBook == null)
            {
                return BadRequest("Bad Request");
            }
            try
            {
                if (!checkBookAlreadyOnRent(rentBook.BookID)) {
                    using (connection = new SqlConnection(
                       _configuration.GetConnectionString("LocalDBAppCon").ToString()))
                    {
                        connection.Open();
                        string queryString = "Insert into rentbooks values(@BookId, @CustomerName, @CustomerContactNumber, @CustomerAdress, @RentDate, @RentPeriodInweeks);" +
                            "update books set IsOnRent = 1 where BookId = @BookId;";
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Parameters.Add("@BookId", SqlDbType.Int);
                        command.Parameters["@BookId"].Value = rentBook.BookID;
                        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar);
                        command.Parameters["@CustomerName"].Value = rentBook.CustomerName;
                        command.Parameters.Add("@CustomerContactNumber", SqlDbType.NVarChar);
                        command.Parameters["@CustomerContactNumber"].Value = rentBook.CustomerContactNumber;
                        command.Parameters.Add("@CustomerAdress", SqlDbType.NVarChar);
                        command.Parameters["@CustomerAdress"].Value = rentBook.CustomerAdress;
                        command.Parameters.Add("@RentPeriodInweeks", SqlDbType.Int);
                        command.Parameters["@RentPeriodInweeks"].Value = rentBook.RentPeriodInweeks;
                        command.Parameters.Add("@RentDate", SqlDbType.DateTime);
                        command.Parameters["@RentDate"].Value = DateTime.Now;
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    message = "Book rented successfully";
                }
                else
                {
                    message = "Book already on rent";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Error in executing API");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }

            return Ok(message);
        }

        private bool checkBookAlreadyOnRent(int bookId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            object isOnRent = false;
            try
            {
                using (connection = new SqlConnection(
                   _configuration.GetConnectionString("LocalDBAppCon").ToString()))
                {
                    connection.Open();
                    string queryString = "select 1 from books where bookId = @BookId and IsOnRent = 1";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@BookId", SqlDbType.Int);
                    command.Parameters["@BookId"].Value = bookId;
                    isOnRent = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());                
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }
            return DatabaseHelper.GetBoolean(isOnRent);
        }

        [HttpPost]
        [Route("returnBook")]
        public IActionResult returnBook(int bookId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            if (bookId == 0 || bookId < 0)
            {
                return BadRequest("Bad Request");
            }
            try
            {
                using (connection = new SqlConnection(
                   _configuration.GetConnectionString("LocalDBAppCon").ToString()))
                {
                    connection.Open();
                    string queryString = "update books set IsOnRent = 0 where BookId = @BookId;";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@BookId", SqlDbType.Int);
                    command.Parameters["@BookId"].Value = bookId;                    
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Error in executing API");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }

            return Ok("Book returned successfully");
        }

        [HttpGet]
        [Route("rentBookHistory")]
        public IActionResult getBookRentHistory()
        {
            List<RentBookHistory> lstRentBooks = null;
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                using (connection = new SqlConnection(
                   _configuration.GetConnectionString("LocalDBAppCon").ToString()))
                {
                    connection.Open();
                    string queryString = "select b.BookId, Title, CustomerName, CustomerContactNumber, CustomerAdress, RentPeriodInweeks, RentDate from rentbooks rb "
                        + "inner join books b on b.BookId = rb.BookId";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    using (reader = command.ExecuteReader())
                    {
                        lstRentBooks = new List<RentBookHistory>();
                        while (reader.Read())
                        {
                            lstRentBooks.Add(new RentBookHistory
                            {
                                BookID = DatabaseHelper.GetInt(reader["BookId"].ToString()),
                                BookName = DatabaseHelper.GetString(reader["Title"].ToString()),
                                CustomerName = DatabaseHelper.GetString(reader["CustomerName"].ToString()),
                                CustomerContactNumber = DatabaseHelper.GetString(reader["CustomerContactNumber"].ToString()),
                                CustomerAdress = DatabaseHelper.GetString(reader["CustomerAdress"].ToString()),
                                RentPeriodInweeks = DatabaseHelper.GetInt(reader["RentPeriodInweeks"]),
                                RentDate = DatabaseHelper.GetDate(reader["RentDate"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Error in executing API");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (connection != null)
                    connection.Close();
            }

            return Ok(lstRentBooks);
        }
    }
}