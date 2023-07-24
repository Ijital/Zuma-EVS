namespace ZumaService.Models
{
    public class VotePack
    {
        #region Properties

        /// <summary>
        /// Gets or sets Voter's age
        /// </summary>
        public int VoterAge { get; set; }

        /// <summary>
        /// Gets or sets voters gender
        /// </summary>
        public string? VoterGender { get; set; }

        /// <summary>
        /// Gets or sets voter's occupation
        /// </summary>
        public string? VoterOccupation { get; set; }

        /// <summary>
        /// Gets or sets voter's selection in presidential election
        /// </summary>
        public string? VoteForPresident { get; set; }

        /// <summary>
        /// Gets or sets voter's selection in Senatorial election
        /// </summary>
        public string? VoteForSenate { get; set; }

        /// <summary>
        /// Gets or sets voter's selection in House of reps election
        /// </summary>
        public string? VoteForReps { get; set; }

        /// <summary>
        /// Gets or sets voters selection in Gubernatorial election
        /// </summary>
        public string? VoteForGovernor { get; set; }

        /// <summary>
        /// Get or sets voter's selection in House of Assembly election
        /// </summary>
        public string? VoteForAssembly { get; set; }

        #endregion
    }
}
