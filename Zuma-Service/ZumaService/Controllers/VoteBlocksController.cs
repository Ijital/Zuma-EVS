namespace ZumaService.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ZumaService.Models;
    using ZumaService.Services.Interface;

    public class VoteBlocksController : Controller
    {
        private readonly IVoteBlockService _voteBlockService;
      
        /// <summary>
        ///  Initialises a new instance of mined VoteBlocks controller
        /// </summary>
        public VoteBlocksController(IVoteBlockService voteBlockService)
        {
            _voteBlockService = voteBlockService;
        }

        /// <summary>
        ///  Saves a pending voteBlockToMine for mining
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> MineVoteBlock([FromBody] VoteBlock voteBlockToMine)
        {
            return Ok(await _voteBlockService.SaveMinedBlock(voteBlockToMine));
        }

        /// <summary>
        /// Saves a pending pendingVoteBlock for mining
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SavePendingVoteBlock([FromBody] PendingVoteBlock pendingVoteBlock)
        {
            await _voteBlockService.SavePendingBlock(pendingVoteBlock);
            return Ok(true);
        }
    }
}