using HaloStats.Web.Server.Domain.Services;
using HaloStats.Web.Shared.Contracts.CarnageReport;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarnageReportController : ControllerBase
{
    private readonly IPostCarnageReportService postCarnageReportService;

    public CarnageReportController(IPostCarnageReportService postCarnageReportService)
    {
        this.postCarnageReportService = postCarnageReportService;
    }


    [HttpPost]
    public async Task<PostCarnageReportResponse> PostCarnageReport([FromBody] PostCarnageReportRequest request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        return await postCarnageReportService.SavePostCarnageReport(request, ip ?? "unknown");
    }
}
