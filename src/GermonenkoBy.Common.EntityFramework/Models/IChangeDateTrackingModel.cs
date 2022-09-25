namespace GermonenkoBy.Common.EntityFramework.Models;

public interface IChangeDateTrackingModel
{
    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}