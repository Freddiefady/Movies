using System;

public class GenreDto
{
    [MaxLength(100)]
    public string Name { get; set; }
}
