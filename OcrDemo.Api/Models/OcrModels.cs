namespace OcrDemo.Api.Models;

public record OfflineOcrEngine(
    string Id,
    string Name,
    string Architecture,
    string DotNetIntegration,
    string License,
    bool SupportsHandwriting,
    bool SupportsPrintedText,
    string StrengthSummary,
    List<string> Highlights
);

public class CertificateFormData
{
    public string PersonName { get; set; } = string.Empty;
    public string CertificateTitle { get; set; } = string.Empty;
    public string AuthorityName { get; set; } = string.Empty;
    public string CertificateId { get; set; } = string.Empty;
    public string IssueDate { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string GradeOrScore { get; set; } = string.Empty;
    public string HandwrittenSignatureOrNote { get; set; } = string.Empty;
}

public class OcrRunResult
{
    public string EngineId { get; set; } = string.Empty;
    public string EngineName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long LatencyMs { get; set; }
    public double AccuracyConfidence { get; set; }
    public string ExecutionMode { get; set; } = string.Empty;
    public CertificateFormData FormFields { get; set; } = new();
    public string RawExtractedText { get; set; } = string.Empty;
    public List<string> Highlights { get; set; } = new();
}
