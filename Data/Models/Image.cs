using Data.Models;
using System.ComponentModel.DataAnnotations;

public class Image
{
    [Key]
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public Guid ProductId { get; set; }
    public required Product Product { get; set; }
}
