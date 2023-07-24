namespace ZumaService.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Migrations;
    using ZumaService.Constants;
    using ZumaService.Models;

    public class ZumaDB : DbContext
    {
        protected readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes an instance of the data context
        /// </summary>
        public ZumaDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Configures the Database context
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataProvider = _configuration["dataProvider"];
            var connectionString = _configuration.GetConnectionString(dataProvider!);

            switch (dataProvider)
            {
                case DataProviders.Sqlite:
                    optionsBuilder.UseSqlite(connectionString);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the Block chain entities
        /// </summary>
        public DbSet<VoteBlock> VoteBlocks { get; set; }

        /// <summary>
        /// Gets or sets Voteblock Dtos entities
        /// </summary>
        public DbSet<PendingVoteBlock> PendingVoteBlocks { get; set; }
    }
}