using Microsoft.EntityFrameworkCore;
using Moq;
using OPSPLReconEngineerTask.Data.Models;

namespace OPSPLReconEngineerTask.Services.Tests;

public static class TestDataProvider
{
    public static DbSet<T> GetDbSetMock<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);
        return mockSet.Object;
    }

    public static List<Book> GetBooks()
    {
        return new List<Book>
        {
            new Book()
            {
                Id = 0, Title = "LOR", Description = "LOR text", AuthorId = 0
            },
            new Book()
            {
                Id = 1, Title = "BOR", Description = "BOR text", AuthorId = 1
            },
            new Book()
            {
                Id = 2, Title = "FAR", Description = "FAR text", AuthorId = 2
            },
        };
    }

    public static List<Author> GetAuthors()
    {
        return new List<Author>
        {
            new Author()
            {
                Id = 0, FirstName = "Bob", LastName = "Jackson",
                Books = new List<Book>()
            },
            new Author()
            {
                Id = 1, FirstName = "Bil", LastName = "Dow",
                Books = new List<Book>()
            },
            new Author()
            {
                Id = 2, FirstName = "Lans", LastName = "Far",
                Books = new List<Book>()
            },
        };
    }

    public static List<BooksTaken> GetBooksTaken()
    {
        return new List<BooksTaken>
        {
            new BooksTaken()
            {
                UserId = 0,
                User = new User { Id = 0, FirstName = "Bil", LastName = "Dow", Email = "bd@g.com" },
                DateTaken = new DateTime(2020, 11, 11),
            },
            new BooksTaken()
            {
                UserId = 1,
                User = new User { Id = 1, FirstName = "Mac", LastName = "Jim", Email = "mj@g.com" },
                DateTaken = new DateTime(2023, 6, 23),
            },
            new BooksTaken()
            {
                UserId = 2,
                User = new User { Id = 2, FirstName = "Jimmy", LastName = "Baron", Email = "jb@g.com" },
                DateTaken = new DateTime(2023, 4, 15),
            }
        };
    }
}