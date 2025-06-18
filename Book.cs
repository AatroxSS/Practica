using System;

namespace Practica
{
    public enum Genre
    {
        ScienceFiction,
        Fantasy,
        Mystery,
        Romance,
        Nonfiction,
        Biography
    }

    [Serializable]
    public class Book
    {
        public Genre Genre { get; set; }
        public int PublicationYear { get; set; }
        public bool Available { get; set; }

        public Book(Genre genre, int year, bool avaible)
        {
            Genre = genre;
            PublicationYear = year;
            Available = avaible;
        }

        public override string ToString()
        {
            return $"P{Genre,-15} {PublicationYear,-10} {Available,-10}";
        }
    }
}
