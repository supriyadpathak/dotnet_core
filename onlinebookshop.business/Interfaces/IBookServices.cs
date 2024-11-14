using Microsoft.Extensions.Logging;

namespace onlinebookshop.business
{
    public interface IBookServices
    {
        void SetDependencies(IBookRepository bookRepository, ILogger logger);
        List<Book> getBookLists();
    }
}