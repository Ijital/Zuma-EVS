namespace ZumaService.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ZumaService.Services.Interface;

    public class ElectionReportsController : Controller
    {
        private readonly IElectionReportService _reportService;


        public ElectionReportsController(IElectionReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Error()
        {
            return Json("ERROR");
        }

        /// <summary>
        /// Gets vote packs
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> President()
        {
            var votePacks = await _reportService.GetPresidentialReportAsync();
            return Json(votePacks);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Senate(string district = "BenueNorthEast")
        {

            var report = await _reportService.GetSenatorialReportAsync(district);

            return Json(report);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Representative(string constituency)
        {
            var votePacks = await _reportService.GetRepresentativeReportAsync(constituency);
            return Json(votePacks);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Governor(string state)
        {
            var votePacks = await _reportService.GetGubernatorialReportAsync(state);
            return Json(votePacks);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Assembly(string constituency)
        {
            var votePacks = await _reportService.GetAssemblyReportAsync(constituency);
            return Json(votePacks);
        }
    }
}
