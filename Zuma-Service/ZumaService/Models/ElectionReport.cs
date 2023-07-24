namespace ZumaService.Models
{
    public record ElectionReport
    {
        /// <summary>
        /// Gets or sets delineation of election report
        /// </summary>
        public string? Delineation { get; set; }

        /// <summary>
        /// Get or sets the election being reported
        /// </summary>
        public string? ElectionType { get; set; }

        /// <summary>
        /// Gets or sets vote packs for election report
        /// </summary>
        public List<PartyVoteCount> PartyVoteCounts { get; set; } = new List<PartyVoteCount>();
    }
}
