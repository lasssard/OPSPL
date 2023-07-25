using Moq;
using OPSPLReconEngineerTask.Data.DbContext;

namespace OPSPLReconEngineerTask.Services.Tests;

[TestFixture]
public class BookSearchServiceTests
{
    [Test]
    public async Task SearchByAuthorName_ReturnsOneMatchedBooksResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var authors = TestDataProvider.GetAuthors();
        var books = TestDataProvider.GetBooks();
        for (var i = 0; i < books.Count; i++)
        {
            books[i].Author = authors[i];
            books[i].AuthorId = authors[i].Id;
        }
        for (var i = 0; i < authors.Count; i++)
        {
            authors[i].Books.Add(books[i]);
        }

        var mockSet = TestDataProvider.GetDbSetMock(authors.AsQueryable());

        dbContext.Setup(x => x.Authors).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = await testable.SearchAsync("Bob", "", "", "or", CancellationToken.None);

        //Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.FirstOrDefault().Id, Is.EqualTo(books.FirstOrDefault().Id));
    }

    [Test]
    public async Task SearchByDescriptionPart_ReturnsMatchedBooksResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var books = TestDataProvider.GetBooks();

        var mockSet = TestDataProvider.GetDbSetMock(books.AsQueryable());

        dbContext.Setup(x => x.Books).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = await testable.SearchAsync("", "text", "", "or", CancellationToken.None);

        //Assert
        Assert.That(result.Count, Is.EqualTo(3));
        CollectionAssert.AreEquivalent(result, books);
    }

    [Test]
    public async Task SearchByTitlePart_ReturnsTwoMatchedBooksResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var books = TestDataProvider.GetBooks();

        var mockSet = TestDataProvider.GetDbSetMock(books.AsQueryable());

        dbContext.Setup(x => x.Books).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("", "OR", "", "or", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(books[0].Id));
        Assert.That(result[1].Id, Is.EqualTo(books[1].Id));
    }

    [Test]
    public async Task SearchByUserWhoHoldingBook_ReturnsTwoMatchedBooksResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();

        var books = TestDataProvider.GetBooks();
        var booksTaken = TestDataProvider.GetBooksTaken();
        for (var i = 0; i < booksTaken.Count; i++)
        {
            booksTaken[i].Book = books[i];
            booksTaken[i].BookId = books[i].Id;
        }
        
        var mockSet = TestDataProvider.GetDbSetMock(booksTaken.AsQueryable());

        dbContext.Setup(x => x.BooksTakens).Returns(mockSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("", "", "Jim", "or", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(booksTaken[1].BookId));
        Assert.That(result[1].Id, Is.EqualTo(booksTaken[2].BookId));
    }

    [Test]
    public async Task SearchMultipleButOneMatchedTerm_Or_ReturnsOneMatchedBookResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var booksTaken = TestDataProvider.GetBooksTaken();
        var authors = TestDataProvider.GetAuthors();
        var books = TestDataProvider.GetBooks();
        for (var i = 0; i < books.Count; i++)
        {
            books[i].Author = authors[i];
            books[i].AuthorId = authors[i].Id;
        }
        
        for (var i = 0; i < authors.Count; i++)
        {
            authors[i].Books.Add(books[i]);
        }

        for (var i = 0; i < booksTaken.Count; i++)
        {
            booksTaken[i].Book = books[i];
            booksTaken[i].BookId = books[i].Id;
        }


        var mockBooksTakenSet = TestDataProvider.GetDbSetMock(booksTaken.AsQueryable());
        var mockBooksSet = TestDataProvider.GetDbSetMock(books.AsQueryable());
        var mockAuthorsSet = TestDataProvider.GetDbSetMock(authors.AsQueryable());

        dbContext.Setup(x => x.Books).Returns(mockBooksSet);
        dbContext.Setup(x => x.Authors).Returns(mockAuthorsSet);
        dbContext.Setup(x => x.BooksTakens).Returns(mockBooksTakenSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("Frank", "LOR", "Dirk", "or", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo(books[0].Id));
    }
    
    [Test]
    public async Task Search2MatchTerms_And_ReturnsOneMatchedBookResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var booksTaken = TestDataProvider.GetBooksTaken();
        var authors = TestDataProvider.GetAuthors();
        var books = TestDataProvider.GetBooks();
        for (var i = 0; i < books.Count; i++)
        {
            books[i].Author = authors[i];
            books[i].AuthorId = authors[i].Id;
        }
        for (var i = 0; i < authors.Count; i++)
        {
            authors[i].Books.Add(books[i]);
        }
        for (var i = 0; i < booksTaken.Count; i++)
        {
            booksTaken[i].Book = books[i];
            booksTaken[i].BookId = books[i].Id;
        }

        var mockBooksTakenSet = TestDataProvider.GetDbSetMock(booksTaken.AsQueryable());
        var mockBooksSet = TestDataProvider.GetDbSetMock(books.AsQueryable());
        var mockAuthorsSet = TestDataProvider.GetDbSetMock(authors.AsQueryable());

        dbContext.Setup(x => x.Books).Returns(mockBooksSet);
        dbContext.Setup(x => x.Authors).Returns(mockAuthorsSet);
        dbContext.Setup(x => x.BooksTakens).Returns(mockBooksTakenSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("Dow", "", "Mac", "and", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo(books[1].Id));
    }
    
    [Test]
    public async Task Search2NotMatchTerms_And_ReturnsEmptyResult()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var booksTaken = TestDataProvider.GetBooksTaken();
        var authors = TestDataProvider.GetAuthors();
        var books = TestDataProvider.GetBooks();
        for (var i = 0; i < books.Count; i++)
        {
            books[i].Author = authors[i];
            books[i].AuthorId = authors[i].Id;
        }
        for (var i = 0; i < authors.Count; i++)
        {
            authors[i].Books.Add(books[i]);
        }
        for (var i = 0; i < booksTaken.Count; i++)
        {
            booksTaken[i].Book = books[i];
            booksTaken[i].BookId = books[i].Id;
        }

        var mockBooksTakenSet = TestDataProvider.GetDbSetMock(booksTaken.AsQueryable());
        var mockBooksSet = TestDataProvider.GetDbSetMock(books.AsQueryable());
        var mockAuthorsSet = TestDataProvider.GetDbSetMock(authors.AsQueryable());

        dbContext.Setup(x => x.Books).Returns(mockBooksSet);
        dbContext.Setup(x => x.Authors).Returns(mockAuthorsSet);
        dbContext.Setup(x => x.BooksTakens).Returns(mockBooksTakenSet);
        var testable = new BookSearchService(dbContext.Object);

        //Act
        var result = (await testable.SearchAsync("Dow", "", "Jimmy", "and", CancellationToken.None)).ToList();

        //Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }
}