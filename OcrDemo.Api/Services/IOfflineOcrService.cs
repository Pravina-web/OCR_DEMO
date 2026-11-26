using OcrDemo.Api.Models;

namespace OcrDemo.Api.Services;

public interface IOfflineOcrService
{
    IReadOnlyList<OfflineOcrEngine> GetOfflineEngines();
    Task<OcrRunResult> ExtractSingleAsync(string engineId, IFormFile file, CancellationToken ct = default);
    Task<List<OcrRunResult>> CompareAllAsync(IFormFile file, CancellationToken ct = default);
}
