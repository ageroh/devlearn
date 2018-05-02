using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain
{
    public class DatabaseContext : DbContext
    {
        private readonly string _connectionString;
        private static object _lock = new object();
        private static bool _tablesExist = false;

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            lock(_lock)
            {
                var databaseCreator = (RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>();
                if (!databaseCreator.Exists())
                {
                    databaseCreator.Create();
                }
                if (!TablesExists())
                {
                    databaseCreator.CreateTables();
                }
            }
        }
       
        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Score>()
                .HasOne(p => p.Event)
                .WithMany(b => b.Scores)
                .HasForeignKey(p => p.EventId)
                .HasConstraintName("ForeignKey_Score_Event")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Score>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Event>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("getdate()");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Event> Event { get; set; }
        public DbSet<Score> Score { get; set; }

      
        private bool TablesExists()
        {
            if(_tablesExist)
            {
                return true;
            }
            var p = new SqlParameter
            {
                ParameterName = "cnt",
                DbType = System.Data.DbType.Int32,
                Direction = System.Data.ParameterDirection.Output,
                SqlValue = 0
            };

            var sql = @"
                SELECT @cnt = 1 
                FROM sys.tables AS T
                WHERE T.Name = 'Event' OR T.Name = 'Score';
                SELECT @cnt;
            ";
            var resp = Database.ExecuteSqlCommand(sql, p);
            _tablesExist =  p?.Value != DBNull.Value && (int)p.Value == 1;
            return _tablesExist;
        }
    }
}
