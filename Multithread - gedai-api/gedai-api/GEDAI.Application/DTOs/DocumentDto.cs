using System;
namespace GEDAI.Application.DTOs;
public class DocumentDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
}