using Microsoft.Extensions.Logging;

namespace onlinebookshop.business
{
    public class BookServices : IBookServices
    {
        private IBookRepository _bookRepository;
        private ILogger _logger;
        public void SetDependencies(IBookRepository bookRepository, ILogger logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        
        public List<Book> getBookLists()
        {
            List<Book> lstBooks = null;

            try
            { 
                lstBooks = _bookRepository.getBookLists();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return lstBooks;
        }
    }
}