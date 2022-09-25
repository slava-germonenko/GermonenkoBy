using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Common.EntityFramework.Tests;

public class TestModel : IChangeDateTrackingModel
{
    [Key]
    public int Id { get; set; }

    public string EmailAddress { get; set; } = "test@test.com";

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}