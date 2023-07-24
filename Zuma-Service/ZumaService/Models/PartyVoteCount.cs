namespace ZumaService.Models
{
    public record PartyVoteCount
    {
        /// <summary>
        /// Gets or sets the party 
        /// </summary>
        public string? Party { get; set; }

        /// <summary>
        /// Gets or sets the total vote count for party
        /// </summary>
        public int TotalVote { get; set; }

        /// <summary>
        /// Gets or sets the total Female Vote count for party
        /// </summary>
        public int FemaleVote { get; set; }

        /// <summary>
        /// Gets or sets the total male Vote count for party
        /// </summary>
        public int MaleVote { get; set; }
    }
}
