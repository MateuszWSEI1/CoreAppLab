using CoreApp.Entities;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Context;

public class ContactsDbContext : IdentityDbContext<CrmUser, CrmRole, string>
{
    public DbSet<Person> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=contacts.db");
    }

    public ContactsDbContext()
    {
    }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CrmUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<CrmRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });

        builder.Entity<Contact>()
            .HasDiscriminator<string>("ContactType")
            .HasValue<Person>("Person")
            .HasValue<Company>("Company")
            .HasValue<Organization>("Organization");

        builder.Entity<Contact>(entity =>
        {
            entity.Property(p => p.Mail).HasMaxLength(200);
            entity.Property(p => p.Phone).HasMaxLength(20);
        });

        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.BirthDate).HasColumnType("date");
            entity.Property(p => p.Gender).HasConversion<string>();
            entity.Property(p => p.Status).HasConversion<string>();
            entity.Property(p => p.FirstName).HasMaxLength(100);
            entity.Property(p => p.LastName).HasMaxLength(100);
            entity.Property(p => p.Position).HasMaxLength(100);
        });

        builder.Entity<Person>()
            .HasOne(p => p.Employer)
            .WithMany(e => e.Employees);

        builder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(p => p.Organization);

        builder.Entity<Company>(entity =>
        {
            entity.HasData(
                new Company
                {
                    Id = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
                    Name = "WSEI",
                    Industry = "edukacja",
                    Phone = "123567123",
                    Mail = "biuro@wsei.edu.pl",
                    Website = "https://wsei.edu.pl"
                }
            );
        });

        builder.Entity<Person>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Gender = Gender.Male,
                    Status = ContactStatus.Active,
                    Email = "adam@wsei.edu.pl",
                    Phone = "123456789",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Programista",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new
                {
                    Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    Gender = Gender.Female,
                    Status = ContactStatus.Blocked,
                    Email = "ewa@wsei.edu.pl",
                    Phone = "123123123",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Tester",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );
        });

        var address = new
        {
            City = "Kraków",
            Country = "Poland",
            PostalCode = "25-009",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence,
            ContactId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f")
        };

        builder.Entity<Contact>()
            .OwnsOne(c => c.Address)
            .HasData(address);

        var adminRoleId = "8F50D6D8-4E73-4D0D-AE62-5C8D11000001";
        var readOnlyRoleId = "8F50D6D8-4E73-4D0D-AE62-5C8D11000002";

        builder.Entity<CrmRole>().HasData(
            new CrmRole
            {
                Id = adminRoleId,
                Name = UserRole.Administrator.ToString(),
                NormalizedName = UserRole.Administrator.ToString().ToUpper(),
                Description = "Administrator systemu"
            },
            new CrmRole
            {
                Id = readOnlyRoleId,
                Name = UserRole.ReadOnly.ToString(),
                NormalizedName = UserRole.ReadOnly.ToString().ToUpper(),
                Description = "Tylko odczyt"
            }
        );

        var user1 = new CrmUser
        {
            Id = "9C0A5A66-98E4-4E84-9C8A-110000000001",
            UserName = "admin@crm.local",
            NormalizedUserName = "ADMIN@CRM.LOCAL",
            Email = "admin@crm.local",
            NormalizedEmail = "ADMIN@CRM.LOCAL",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Admin",
            FullName = "System Admin",
            Department = "IT",
            Status = SystemUserStatus.Active,
            CreatedAt = DateTime.Now,
            SecurityStamp = "ADMIN-STAMP-0001",
            ConcurrencyStamp = "ADMIN-CONCURRENCY-0001"
        };

        var user2 = new CrmUser
        {
            Id = "9C0A5A66-98E4-4E84-9C8A-110000000002",
            UserName = "readonly@crm.local",
            NormalizedUserName = "READONLY@CRM.LOCAL",
            Email = "readonly@crm.local",
            NormalizedEmail = "READONLY@CRM.LOCAL",
            EmailConfirmed = true,
            FirstName = "Read",
            LastName = "Only",
            FullName = "Read Only",
            Department = "Support",
            Status = SystemUserStatus.Active,
            CreatedAt = DateTime.Now,
            SecurityStamp = "READONLY-STAMP-0001",
            ConcurrencyStamp = "READONLY-CONCURRENCY-0001"
        };

        var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<CrmUser>();
        user1.PasswordHash = passwordHasher.HashPassword(user1, "Admin123!");
        user2.PasswordHash = passwordHasher.HashPassword(user2, "ReadOnly123!");

        builder.Entity<CrmUser>().HasData(user1, user2);

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().HasData(
            new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
            {
                UserId = user1.Id,
                RoleId = adminRoleId
            },
            new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
            {
                UserId = user2.Id,
                RoleId = readOnlyRoleId
            }
        );
    }
}