namespace OPSPLReconEngineerTask.Services;

/// <summary>
/// Custom implementation. 
/// </summary>
public class WordCustomInverter : IWordInverter
{
    public string InvertWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            throw new ArgumentException("The word should not be null or empty string");
        }

        var result = new char[word.Length];
        var initialIndex = word.Length - 1;
        for (var i = initialIndex; i >= 0 && i < word.Length; i--)
        {
            result[initialIndex - i] = word[i];
        }

        return new string(result);
    }
}