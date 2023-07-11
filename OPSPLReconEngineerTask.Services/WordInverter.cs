namespace OPSPLReconEngineerTask.Services;

/// <summary>
/// Built-in Linq implementation based on reversed iterator. 
/// </summary>
public class WordInverter : IWordInverter
{
    public string InvertWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            throw new ArgumentException("The word should not be null or empty string");
        }

        return new string(word.Reverse().ToArray());
    }
}