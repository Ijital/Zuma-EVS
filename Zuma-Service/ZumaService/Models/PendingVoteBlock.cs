using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZumaService.Models
{
    public record PendingVoteBlock
    {
        #region Properties
        /// <summary>
        /// Gets or sets Vote block Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets vote block index
        /// </summary>
        public int VoteBlockIndex { get; set; }

        /// <summary>
        /// Gets or set universal block vote Id
        /// </summary>
        public string? VoteBlockGuid { get; set; }

        /// <summary>
        /// Gets or sets Vote block State
        /// </summary>
        public int VoteBlockState { get; set; }

        /// <summary>
        /// Gets or sets Vote block Local government
        /// </summary>
        public int VoteBlockLG { get; set; }

        /// <summary>
        /// Gets or sets Vote block Polling unit
        /// </summary>
        public int VoteBlockPU { get; set; }

        /// <summary>
        /// Gets or sets a number of vote packs in the vote block
        /// </summary>
        public string? VotePacks { get; set; }

        /// <summary>
        /// Gets or sets hash of current block data
        /// </summary>
        public string? VoteBlockHash { get; set; }

        /// <summary>
        /// Gets or set hash of previous block
        /// </summary>
        public string? LastBlockHash { get; set; }

        #endregion
    }
}
