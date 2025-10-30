using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связи один-ко-многим между Author и Book
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка индексов
        modelBuilder.Entity<Author>()
            .HasIndex(a => a.Name);

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Title);

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.PublishedYear);

        // Seed данные
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed авторов
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "Лев Толстой", DateOfBirth = new DateTime(1828, 9, 9) },
            new Author { Id = 2, Name = "Фёдор Достоевский", DateOfBirth = new DateTime(1821, 11, 11) },
            new Author { Id = 3, Name = "Антон Чехов", DateOfBirth = new DateTime(1860, 1, 29) }
        );

        // Seed книг
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Война и мир", PublishedYear = 1869, AuthorId = 1 },
            new Book { Id = 2, Title = "Анна Каренина", PublishedYear = 1877, AuthorId = 1 },
            new Book { Id = 3, Title = "Преступление и наказание", PublishedYear = 1866, AuthorId = 2 },
            new Book { Id = 4, Title = "Идиот", PublishedYear = 1869, AuthorId = 2 },
            new Book { Id = 5, Title = "Вишнёвый сад", PublishedYear = 1904, AuthorId = 3 }
        );
    }
}
