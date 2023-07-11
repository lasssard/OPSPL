using Microsoft.EntityFrameworkCore;
using Moq;
using OPSPLReconEngineerTask.Data.DbContext;
using OPSPLReconEngineerTask.Data.Models;

namespace OPSPLReconEngineerTask.Services.Tests;

[TestFixture]
public class BookSearchServiceTests
{
    [Test]
    public async Task SearchByAuthor_GivesExpectedResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var data = new List<Author>
        {
            new Author()
            {
                Id = 0, FirstName = "Bob", MiddleName = "Jackson", LastName = "Builder",
                Books = new[] { new Book { AuthorId = 0, Id = 0, Title = "LOR", Description = "LOR text" } }
            },
            new Author()
            {
                Id = 1, FirstName = "Bil", LastName = "Dow",
                Books = new[] { new Book { AuthorId = 1, Id = 1, Title = "BOR", Description = "BOR text" } }
            },
            new Author()
            {
                Id = 2, FirstName = "Lans", LastName = "Far",
                Books = new[] { new Book { AuthorId = 2, Id = 2, Title = "FAR", Description = "FAR text" } }
            },
        }.AsQueryable();

        var mockSet = GetDbSetMock(data);

        dbContext.Setup(x => x.Authors).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = await testable.SearchAsync("Bob", "", "", "or", CancellationToken.None);

        //Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.FirstOrDefault().AuthorId, Is.EqualTo(data.FirstOrDefault().Id));
    }

    [Test]
    public async Task SearchByDescription_GivesExpectedResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var data = new List<Book>
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
        }.AsQueryable();

        var mockSet = GetDbSetMock(data);

        dbContext.Setup(x => x.Books).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = await testable.SearchAsync("", "text", "", "or", CancellationToken.None);

        //Assert
        Assert.That(result.Count, Is.EqualTo(3));
        CollectionAssert.AreEquivalent(result, data.ToList());
    }

    [Test]
    public async Task SearchByTitle_GivesExpectedResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var data = new List<Book>
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
        }.AsQueryable();

        var mockSet = GetDbSetMock(data);

        dbContext.Setup(x => x.Books).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("", "OR", "", "or", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(data.ToList()[0].Id));
        Assert.That(result[1].Id, Is.EqualTo(data.ToList()[1].Id));
    }

    [Test]
    public async Task SearchByUserWhoHoldingBook_GivesExpectedResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var data = new List<BooksTaken>
        {
            new BooksTaken()
            {
                UserId = 0, BookId = 2,
                User = new User { Id = 0, FirstName = "Bil", LastName = "Dow", Email = "bd@g.com" },
                DateTaken = new DateTime(2020, 11, 11),
                Book = new Book()
                {
                    Id = 2, Title = "FAR", Description = "FAR text", AuthorId = 2
                }
            },
            new BooksTaken()
            {
                UserId = 1, BookId = 1,
                User = new User { Id = 1, FirstName = "Mac", LastName = "Jim", Email = "mj@g.com" },
                DateTaken = new DateTime(2023, 7, 23),
                Book = new Book()
                {
                    Id = 1, Title = "BOR", Description = "BOR text", AuthorId = 1
                }
            },
            new BooksTaken()
            {
                UserId = 2, BookId = 0,
                User = new User { Id = 2, FirstName = "Jimmy", LastName = "Baron", Email = "jb@g.com" },
                DateTaken = new DateTime(2023, 4, 15),
                Book = new Book()
                {
                    Id = 0, Title = "LOR", Description = "LOR text", AuthorId = 0
                }
            }
        }.AsQueryable();

        var mockSet = GetDbSetMock(data);

        dbContext.Setup(x => x.BooksTakens).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("", "", "Jim", "or", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(data.ToList()[1].BookId));
        Assert.That(result[1].Id, Is.EqualTo(data.ToList()[2].BookId));
    }

    [Test]
    public async Task SearchMultipleTermsOr_GivesExpectedResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var booksTakenData = new List<BooksTaken>
        {
            new BooksTaken()
            {
                UserId = 0, BookId = 2,
                User = new User { Id = 0, FirstName = "Bil", LastName = "Dow", Email = "bd@g.com" },
                DateTaken = new DateTime(2020, 11, 11),
                Book = new Book()
                {
                    Id = 2, Title = "FAR", Description = "FAR text", AuthorId = 2
                }
            },
            new BooksTaken()
            {
                UserId = 1, BookId = 1,
                User = new User { Id = 1, FirstName = "Mac", LastName = "Jim", Email = "mj@g.com" },
                DateTaken = new DateTime(2023, 7, 23),
                Book = new Book()
                {
                    Id = 1, Title = "BOR", Description = "BOR text", AuthorId = 1
                }
            },
            new BooksTaken()
            {
                UserId = 2, BookId = 0,
                User = new User { Id = 2, FirstName = "Jimmy", LastName = "Baron", Email = "jb@g.com" },
                DateTaken = new DateTime(2023, 4, 15),
                Book = new Book()
                {
                    Id = 0, Title = "LOR", Description = "LOR text", AuthorId = 0
                }
            }
        }.AsQueryable();

        var booksData = new List<Book>
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
        }.AsQueryable();

        var authorsData = new List<Author>
        {
            new Author()
            {
                Id = 0, FirstName = "Bob", MiddleName = "Jackson", LastName = "Builder",
                Books = new[] { new Book { AuthorId = 0, Id = 0, Title = "LOR", Description = String.Empty } }
            },
            new Author()
            {
                Id = 1, FirstName = "Bil", LastName = "Dow",
                Books = new[] { new Book { AuthorId = 1, Id = 1, Title = "BOR", Description = String.Empty } }
            },
            new Author()
            {
                Id = 2, FirstName = "Lans", LastName = "Far",
                Books = new[] { new Book { AuthorId = 2, Id = 2, Title = "FAR", Description = "Description2" } }
            },
        }.AsQueryable();


        var mockBooksTakenSet = GetDbSetMock(booksTakenData);
        var mockBooksSet = GetDbSetMock(booksData);
        var mockAuthorsSet = GetDbSetMock(authorsData);

        dbContext.Setup(x => x.Books).Returns(mockBooksSet);
        dbContext.Setup(x => x.Authors).Returns(mockAuthorsSet);
        dbContext.Setup(x => x.BooksTakens).Returns(mockBooksTakenSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("Frank", "LOR", "Dirk", "or", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo(booksData.ToList()[0].Id));
    }
    
    private DbSet<T> GetDbSetMock<T>(IQueryable<T> data) where T : class
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
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        return mockSet.Object;
    }
}