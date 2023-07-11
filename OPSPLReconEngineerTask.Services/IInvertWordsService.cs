using OPSPLReconEngineerTask.Data.Models;

namespace OPSPLReconEngineerTask.Services;

public interface IInvertWordsService
{
    Book InvertBookTitle(Book inputBook);
}

public class InvertWordsService : IInvertWordsService
{
    private readonly IWordInverter _wordInverter;
    public InvertWordsService(IWordInverter wordInverter)
    {
        _wordInverter = wordInverter ?? throw new ArgumentNullException(nameof(wordInverter));
    }


    public Book InvertBookTitle(Book inputBook)
    {
        var invertedTitle = _wordInverter.InvertWord(inputBook.Title);
        return new Book
        {
            Author = inputBook.Author,
            AuthorId = inputBook.AuthorId,
            Description = inputBook.Description,
            Id = inputBook.Id,
            Title = invertedTitle
        };
    }
}