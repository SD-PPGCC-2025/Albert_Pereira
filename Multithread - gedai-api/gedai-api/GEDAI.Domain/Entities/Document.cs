using System;

namespace GEDAI.Domain.Entities;
public class Document
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public Document()
    {
        CreatedDate = DateTime.UtcNow;
    }
}