using ZumaService.Models;

namespace ZumaService.Services.Interface
{
    public interface IElectionReportService
    {
        /// <summary>
        /// Gets presidential election report
        /// </summary>
        Task<ElectionReport> GetPresidentialReportAsync();

        /// <summary>
        /// Gets Senatorial election report
        /// </summary>
        Task<ElectionReport> GetSenatorialReportAsync(string district);

        /// <summary>
        /// Gets Representative election report
        /// </summary>
        Task<ElectionReport> GetRepresentativeReportAsync(string constituency);

        /// <summary>
        /// Gets Gubernatorial election report
        /// </summary>
        Task<ElectionReport> GetGubernatorialReportAsync(string state);

        /// <summary>
        /// Gets state Assembly election report
        /// </summary>
        Task<ElectionReport> GetAssemblyReportAsync(string constituency);
    }
}
