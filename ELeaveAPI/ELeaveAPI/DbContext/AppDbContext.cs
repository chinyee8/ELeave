using ELeaveAPI.Models;
using Microsoft.EntityFrameworkCore;


    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Each DbSet = one table in the database
        public DbSet<User> Users => Set<User>();
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
        public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
        mb.Entity<User>().ToTable("M_Users");

        // Tell EF how LeaveRequest links to User (two foreign keys)
        mb.Entity<LeaveBalance>()
                .HasKey(lb => lb.BalanceID);
        mb.Entity<LeaveRequest>()
                .HasKey(r => r.RequestID);

        mb.Entity<LeaveRequest>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<LeaveRequest>()
                .HasOne(r => r.Approver)
                .WithMany()
                .HasForeignKey(r => r.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


