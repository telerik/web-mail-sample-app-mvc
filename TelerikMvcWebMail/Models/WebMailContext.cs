using System;
using System.Data.Entity;
using System.Data.SQLite;

namespace TelerikMvcWebMail.Models
{

    public partial class WebMailEntities : DbContext
    {
        public WebMailEntities()
            : base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource = $"{AppDomain.CurrentDomain.GetData("DataDirectory").ToString()}\\demos.db",
                    ForeignKeys = true
                }.ConnectionString
            },
            true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Mail> Mails { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
    }
}