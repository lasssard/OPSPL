using System.Text.Json.Serialization;

namespace OPSPLReconEngineerTask.Data.Models;

public partial class BooksTaken
{
    public long BookId { get; set; }

    public long UserId { get; set; }

    public DateTime DateTaken { get; set; }

    [JsonIgnore]
    public virtual Book Book { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
