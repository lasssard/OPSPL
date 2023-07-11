using Microsoft.EntityFrameworkCore;
using OPSPLReconEngineerTask.Data.DbContext;
using OPSPLReconEngineerTask.Data.Models;

namespace OPSPLReconEngineerTask.Services;

public class BookSearchService : IBookSearchService
{
    private readonly OPSPLTaskContext _dbContext;

    public BookSearchService(OPSPLTaskContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<Book>> SearchAsync(string author, string textToFind, string currentBookHolder, string combineCondition, CancellationToken cancellationToken)
    {
        if (combineCondition.ToLowerInvariant() == "or")
        {
            return await SingleTermSearchAsync(author, textToFind, currentBookHolder, cancellationToken);
        }
        return await MultiTermSearchAsync(author, textToFind, currentBookHolder, cancellationToken);
    }

    private async Task<IList<Book>> SingleTermSearchAsync(string author, string textToFind, string currentBookHolder, CancellationToken cancellationToken)
    {
        IQueryable<Book> books = null!;
        if (!string.IsNullOrWhiteSpace(author))
        {
            books = SearchByAuthor(author);
            if (books.Any())
            {
                return await books.ToListAsync(cancellationToken);
            }
        }

        if (!string.IsNullOrWhiteSpace(textToFind))
        {
            books = SearchBookByText(textToFind);
            if (books.Any())
            {
                return await books.ToListAsync(cancellationToken);
            }
        }

        if (!string.IsNullOrWhiteSpace(currentBookHolder))
        {
            books = SearchBookByCurrentBookHolder(currentBookHolder);
            if (books.Any())
            {
                return await books.ToListAsync(cancellationToken);
            }
        }

        return await books?.ToListAsync(cancellationToken)!;
    }

    private async Task<IList<Book>> MultiTermSearchAsync(string author, string textToFind, string currentBookHolder, CancellationToken cancellationToken)
    {
        IQueryable<Book> books = null!;
        if (!string.IsNullOrWhiteSpace(author))
        {
            books = SearchByAuthor(author);
        }

        if (!string.IsNullOrWhiteSpace(textToFind))
        {
            var foundByText = SearchBookByText(textToFind);
            books = (books != null ? books.Intersect(foundByText) : foundByText);
        }

        if (!string.IsNullOrWhiteSpace(currentBookHolder))
        {
            var foundByCurrentBookHolder = SearchBookByCurrentBookHolder(currentBookHolder);
            books = (books != null ? books.Intersect(foundByCurrentBookHolder) : foundByCurrentBookHolder);
        }

        return await books?.ToListAsync(cancellationToken)!;
    }

    private IQueryable<Book> SearchByAuthor(string author)
    {
        return _dbContext.Authors.Where(a =>
            a.FirstName.Contains(author)
            || a.LastName.Contains(author)
            || a.MiddleName != null && a.MiddleName.Contains(author)).SelectMany(x => x.Books);
    }

    private IQueryable<Book> SearchBookByText(string textToFind)
    {
        return _dbContext.Books.Where(a =>
            a.Title.Contains(textToFind)
            || a.Description != null && a.Description.Contains(textToFind));
    }

    private IQueryable<Book> SearchBookByCurrentBookHolder(string currentBookHolder)
    {
        return _dbContext.BooksTakens.Where(a =>
            a.User.FirstName.Contains(currentBookHolder)
            || a.User.LastName.Contains(currentBookHolder)
            || a.User.Email.Contains(currentBookHolder)).Select(x => x.Book);
    }
}