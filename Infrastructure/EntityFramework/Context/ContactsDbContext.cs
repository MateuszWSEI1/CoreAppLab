using CoreApp.Entities;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
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
                new
                {
                    Id = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
                    Name = "WSEI",
                    Industry = "edukacja",
                    Phone = "123567123",
                    Mail = "biuro@wsei.edu.pl",
                    Website = "https://wsei.edu.pl",
                    EmployeeCount = 10,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                    Status = ContactStatus.Active
                }
            );
        });

        builder.Entity<Person>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("3D54091D-ABC8-49EC-9590-93AD3ED5458F"),
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Gender = Gender.Male,
                    Status = ContactStatus.Active,
                    Mail = "adam@wsei.edu.pl",
                    Email = "adam@wsei.edu.pl",
                    Phone = "123456789",
                    BirthDate = new DateTime(2001, 1, 11),
                    Position = "Programista",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0)
                },
                new
                {
                    Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    Gender = Gender.Female,
                    Status = ContactStatus.Blocked,
                    Mail = "ewa@wsei.edu.pl",
                    Email = "ewa@wsei.edu.pl",
                    Phone = "123123123",
                    BirthDate = new DateTime(2001, 1, 11),
                    Position = "Tester",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0)
                });
        });

        builder.Entity<Contact>()
            .OwnsOne(c => c.Address, address =>
            {
                address.HasData(
                    new
                    {
                        ContactId = Guid.Parse("3D54091D-ABC8-49EC-9590-93AD3ED5458F"),
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        City = "Kraków",
                        Country = "Poland",
                        PostalCode = "25-009",
                        Street = "ul. Św. Filipa 17",
                        Type = AddressType.Correspondence
                    },
                    new
                    {
                        ContactId = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        City = "Kraków",
                        Country = "Poland",
                        PostalCode = "30-001",
                        Street = "ul. Testowa 2",
                        Type = AddressType.Correspondence
                    },
                    new
                    {
                        ContactId = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        City = "Kraków",
                        Country = "Poland",
                        PostalCode = "31-150",
                        Street = "ul. Akademicka 1",
                        Type = AddressType.Correspondence
                    }
                );
            });

        var adminRoleId = "8F50D6D8-4E73-4D0D-AE62-5C8D11000001";
        var readOnlyRoleId = "8F50D6D8-4E73-4D0D-AE62-5C8D11000002";

        builder.Entity<CrmRole>().HasData(
            new
            {
                Id = adminRoleId,
                Name = UserRole.Administrator.ToString(),
                NormalizedName = UserRole.Administrator.ToString().ToUpper(),
                Description = "Administrator systemu",
                ConcurrencyStamp = "ROLE-CONCURRENCY-0001"
            },
            new
            {
                Id = readOnlyRoleId,
                Name = UserRole.ReadOnly.ToString(),
                NormalizedName = UserRole.ReadOnly.ToString().ToUpper(),
                Description = "Tylko odczyt",
                ConcurrencyStamp = "ROLE-CONCURRENCY-0002"
            }
        );

        builder.Entity<CrmUser>().HasData(
            new
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
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                SecurityStamp = "ADMIN-STAMP-0001",
                ConcurrencyStamp = "ADMIN-CONCURRENCY-0001",
                PasswordHash = "AQAAAAIAAYagAAAAEAAAAAAAAAAAAAAAAAAAAA==",
                PhoneNumber = (string?)null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = (DateTimeOffset?)null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                LastLoginAt = (DateTime?)null,
                DeactivatedAt = (DateTime?)null
            },
            new
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
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                SecurityStamp = "READONLY-STAMP-0001",
                ConcurrencyStamp = "READONLY-CONCURRENCY-0001",
                PasswordHash = "AQAAAAIAAYagAAAAEAAAAAAAAAAAAAAAAAAAAA==",
                PhoneNumber = (string?)null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = (DateTimeOffset?)null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                LastLoginAt = (DateTime?)null,
                DeactivatedAt = (DateTime?)null
            }
        );

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = "9C0A5A66-98E4-4E84-9C8A-110000000001",
                RoleId = adminRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = "9C0A5A66-98E4-4E84-9C8A-110000000002",
                RoleId = readOnlyRoleId
            }
        );
    }
}