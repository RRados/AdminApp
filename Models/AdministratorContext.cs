using Microsoft.EntityFrameworkCore;

namespace AdminApp.Models
{
    public partial class AdministratorContext : DbContext
    {
        public AdministratorContext()
        {
        }

        public AdministratorContext(DbContextOptions<AdministratorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LogIn> LogIns { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP\\SQLSERVER;Initial Catalog=Administrator;Integrated Security=SSPI;TrustServerCertificate=Yes;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<LogIn>(entity =>
            {
                entity.HasKey(e => e.IdLogin);
                entity.ToTable("LogIn");

                entity.Property(e => e.IdLogin)
                    .HasColumnName("idLogin");

                entity.Property(e => e.IdUser).HasColumnName("idUser");
                entity.Property(e => e.LoggedIn)
                    .HasColumnType("datetime")
                    .HasColumnName("loggedIn");

                entity.Property(e => e.LoggedOut)
                    .HasColumnType("datetime")
                    .HasColumnName("loggedOut");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.LogIns)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_LogIn_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.ToTable("Role");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("date")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateDismised)
                    .HasColumnType("date")
                    .HasColumnName("dateDismised");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasColumnName("confirmPassword")
                    .HasMaxLength(50)
                    .IsUnicode(false);


            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.IdRole, e.IdUser });
                entity.ToTable("UserRole");

                entity.Property(e => e.IdRole).HasColumnName("idRole");
                entity.Property(e => e.IdUser).HasColumnName("idUser");
                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany()
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_UserRole_User");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
