// This DTO is used when creating a new external link, a link attached to a specific chapter

public class CreateExternalLinkDto
{
    public string Title { get; set; } = string.Empty; // Title or label of the link
    public string Url { get; set; } = string.Empty; // The actual URL of the external link
    public int ChapterId { get; set; } // ID of the chapter this link is associated with
}
