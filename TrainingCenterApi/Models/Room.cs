using System.ComponentModel.DataAnnotations;

namespace TrainingCenterApi.Models;

public class Room
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string BuildingCode { get; set; }

    public int Floor { get; set; }

    [Range(1, 1000)]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }

    public bool IsActive { get; set; }
}