using Moq;
using NUnit.Framework;
using OPSPLReconEngineerTask.Data.DbContext;

namespace OPSPLReconEngineerTask.Services.Tests;

[TestFixture]
public class ReportBuildServiceTests
{
    [Test]
    public async Task GetReport_ReturnsCalculatedReportBasedOnTestData()
    {
        //Arrange
        var dbContext = new Mock<OPSPLTaskContext>();
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        
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

        dateTimeProvider.Setup(x => x.GetUtcNow()).Returns(new DateTime(2023, 07, 9));
        
        var testable = new ReportBuildService(dbContext.Object, dateTimeProvider.Object);
        
        //Act
        var result = await testable.GetUserReportsAsync(CancellationToken.None);
        
        //Assert
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].TotalBooksHolds, Is.EqualTo(1));
        Assert.That(result[0].TotalDaysHolds, Is.EqualTo(970));
        Assert.That(result[0].User.Id, Is.EqualTo(booksTaken[0].UserId));
        
        Assert.That(result[1].TotalBooksHolds, Is.EqualTo(1));
        Assert.That(result[1].TotalDaysHolds, Is.EqualTo(16));
        Assert.That(result[1].User.Id, Is.EqualTo(booksTaken[1].UserId));
        
        Assert.That(result[2].TotalBooksHolds, Is.EqualTo(1));
        Assert.That(result[2].TotalDaysHolds, Is.EqualTo(85));
        Assert.That(result[2].User.Id, Is.EqualTo(booksTaken[2].UserId));
    }
}