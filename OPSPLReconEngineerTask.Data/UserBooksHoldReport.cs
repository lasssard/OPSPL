using System.Text.Json.Serialization;
using OPSPLReconEngineerTask.Data.Models;

namespace OPSPLReconEngineerTask.Data;

public class UserBooksHoldReport
{
    public User User { get; set; }
    public int TotalBooksHolds { get; set; }
    public int TotalDaysHolds { get; set; }

}