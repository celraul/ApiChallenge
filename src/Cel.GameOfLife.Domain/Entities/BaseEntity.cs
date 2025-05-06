using System.ComponentModel.DataAnnotations;

namespace Cel.GameOfLife.Domain.Entities;

public class BaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
}
