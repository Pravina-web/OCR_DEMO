using Microsoft.AspNetCore.Mvc;
using OcrDemo.Api.Models;
using OcrDemo.Api.Services;

namespace OcrDemo.Api.Controllers;

public class SingleExtractRequest
{
    public string EngineId { get; set; } = "windowsocr";
    public IFormFile File { get; set; } = default!;
}

public class CompareAllRequest
{
    public IFormFile File { get; set; } = default!;
}

[ApiController]
[Route("api/[controller]")]
public class OcrController : ControllerBase
{
    private readonly IOfflineOcrService _ocrService;

    public OcrController(IOfflineOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    [HttpGet("engines")]
    public ActionResult<IReadOnlyList<OfflineOcrEngine>> GetEngines()
    {
        return Ok(_ocrService.GetOfflineEngines());
    }

    [HttpPost("extract")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(25_000_000)]
    public async Task<ActionResult<OcrRunResult>> ExtractSingle(
        [FromForm] SingleExtractRequest request,
        CancellationToken ct)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest(new { error = "Please upload a PDF or image certificate file." });

        var result = await _ocrService.ExtractSingleAsync(request.EngineId, request.File, ct);
        return Ok(result);
    }

    [HttpPost("compare-all")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(25_000_000)]
    public async Task<ActionResult<List<OcrRunResult>>> CompareAll(
        [FromForm] CompareAllRequest request,
        CancellationToken ct)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest(new { error = "Please upload a PDF or image certificate file." });

        var results = await _ocrService.CompareAllAsync(request.File, ct);
        return Ok(results);
    }
}