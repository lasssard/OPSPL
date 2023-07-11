using System.Text.Json.Serialization;

namespace OPSPLReconEngineerTask.Data.Models;

public partial class Author
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
