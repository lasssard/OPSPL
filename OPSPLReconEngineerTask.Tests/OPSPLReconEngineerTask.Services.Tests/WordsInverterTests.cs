namespace OPSPLReconEngineerTask.Services.Tests;

[TestFixture]
public class WordsInverterTests
{
    [TestCase(typeof(WordCustomInverter))] 
    [TestCase(typeof(WordInverter))] 
    public void InvertWord_ReturnsInvertedWord(Type type)
    {
        //Arrange
        IWordInverter testable = (IWordInverter)Activator.CreateInstance(type)!;
        var word = "my word to test the logic";
        
        //Act
        var invertedWord = testable.InvertWord(word);
        
        //Assert
        for (var i = 0; i < word.Length; i++)
        {
            Assert.That(invertedWord[invertedWord.Length - 1 - i], Is.EqualTo(word[i]));
        }
    }
    
    [TestCase(typeof(WordCustomInverter))] 
    [TestCase(typeof(WordInverter))] 
    public void InvertWord_IfWordIsNullOrEmpty_ThrowException(Type type)
    {
        //Arrange
        IWordInverter testable = (IWordInverter)Activator.CreateInstance(type)!;

        //Act & assert
       Assert.Throws<ArgumentException>(() => testable.InvertWord(null));
       Assert.Throws<ArgumentException>(() => testable.InvertWord(string.Empty));
    }
}