using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);
        Task<SearchResponse<Book>> SearchBooksAsync(SearchRequest req);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public virtual async Task<SearchResponse<Book>> SearchBooksAsync(SearchRequest req)
        {
            // Default query for all books
            var baseQuery = _dataContext
                .Books
                .AsQueryable();

            // If text given, filter against text value
            if (!string.IsNullOrWhiteSpace(req.Text))
            {
                baseQuery = baseQuery
                    .Where(x =>
                        EF.Functions.Like(x.Title, $"%{req.Text}%") ||
                        EF.Functions.Like(x.ISBN, $"%{req.Text}%") ||
                        EF.Functions.Like(x.PublishedDate, $"%{req.Text}%") ||
                        EF.Functions.Like(x.ShortDescr, $"%{req.Text}%") ||
                        EF.Functions.Like(x.LongDescr, $"%{req.Text}%"));
            }

            // Run query to get full item count
            var totalItemsTask = baseQuery
                .CountAsync();

            var pagedQuery =
                (req.ItemsPerPage > 0)
                ? baseQuery
                    .Skip((req.Page - 1) * req.ItemsPerPage)
                    .Take(req.ItemsPerPage)
                : baseQuery; // Ignore paging if page size not specified

            // Run query to get paged count
            var itemsTask = pagedQuery
                .ToListAsync();

            // Wait for item count
            var totalItems = await totalItemsTask;

            return new SearchResponse<Book>
            {
                Items = await itemsTask,
                Page = Math.Max(req.Page, 1),
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / Math.Max(req.ItemsPerPage, 1))
            };
        }
    }
}
