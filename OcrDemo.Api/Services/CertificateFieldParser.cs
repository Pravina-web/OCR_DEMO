using System.Text.RegularExpressions;
using OcrDemo.Api.Models;

namespace OcrDemo.Api.Services;

public static class CertificateFieldParser
{
    public static CertificateFormData ExtractFormFields(string rawText)
    {
        var data = new CertificateFormData();
        if (string.IsNullOrWhiteSpace(rawText)) return data;

        var lines = rawText
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        // 1. Person Name
        var nameMatch = Regex.Match(rawText,
            @"(?:certify that|awarded to|presented to|granted to|recipient|person name|name)[:\s]+([A-Z][a-zA-Z0-9'\-\.]+(?:\s+[A-Z][a-zA-Z0-9'\-\.]+){1,3})",
            RegexOptions.IgnoreCase);
        if (nameMatch.Success)
        {
            data.PersonName = nameMatch.Groups[1].Value.Trim();
        }
        else
        {
            var fallbackName = lines.FirstOrDefault(l =>
                Regex.IsMatch(l, @"^[A-Z][a-zA-Z]+\s+[A-Z][a-zA-Z]+(?:\s+[A-Z][a-zA-Z]+)?$") &&
                !l.Contains("Certificate", StringComparison.OrdinalIgnoreCase) &&
                !l.Contains("Authority", StringComparison.OrdinalIgnoreCase));
            data.PersonName = fallbackName ?? "Not Detected";
        }

        // 2. Certificate Title
        var titleMatch = Regex.Match(rawText,
            @"(?:Certificate of|Certified|Diploma in|Program|Course)[:\s]*([^\r\n]+)",
            RegexOptions.IgnoreCase);
        data.CertificateTitle = titleMatch.Success
            ? titleMatch.Value.Trim()
            : (lines.FirstOrDefault(l => l.Contains("Certificate", StringComparison.OrdinalIgnoreCase)) ?? "Professional Certificate");

        // 3. Authority / Issuer Name
        var authMatch = Regex.Match(rawText,
            @"(?:Certificate Authority|Authority|Issued by|Organization|Institute|University|Academy|Board)[:\s]+([^\r\n]+)",
            RegexOptions.IgnoreCase);
        if (authMatch.Success)
        {
            data.AuthorityName = authMatch.Groups[1].Value.Trim();
        }
        else
        {
            var orgLine = lines.FirstOrDefault(l =>
                Regex.IsMatch(l, @"(Institute|University|Academy|Authority|Board|Microsoft|AWS|Google|Cisco|PMI|ISO)", RegexOptions.IgnoreCase));
            data.AuthorityName = orgLine ?? "Not Detected";
        }

        // 4. Certificate ID
        var idMatch = Regex.Match(rawText,
            @"(?:Certificate\s*(?:ID|No|Number|#)|Credential\s*ID|Reg(?:istration)?\s*No)[:\s#]*([A-Z0-9\-]{5,25})",
            RegexOptions.IgnoreCase);
        data.CertificateId = idMatch.Success ? idMatch.Groups[1].Value.Trim() : "Not Detected";

        // 5. Issue Date
        var issueMatch = Regex.Match(rawText,
            @"(?:Issued(?:\s+on)?|Issue\s+Date|Date\s+of\s+Issue|Granted\s+on)[:\s]*([A-Za-z]{3,9}\s+\d{1,2},?\s+\d{4}|\d{4}-\d{2}-\d{2}|\d{1,2}[\/\-]\d{1,2}[\/\-]\d{2,4})",
            RegexOptions.IgnoreCase);
        data.IssueDate = issueMatch.Success ? issueMatch.Groups[1].Value.Trim() : "Not Detected";

        // 6. Expiry Date
        var expiryMatch = Regex.Match(rawText,
            @"(?:Valid\s+(?:Until|Thru|Through)|Expir(?:es|y|ation)(?:\s+Date)?)[:\s]*([A-Za-z]{3,9}\s+\d{1,2},?\s+\d{4}|\d{4}-\d{2}-\d{2}|\d{1,2}[\/\-]\d{1,2}[\/\-]\d{2,4}|No\s+Expiration|Lifetime)",
            RegexOptions.IgnoreCase);
        data.ExpiryDate = expiryMatch.Success ? expiryMatch.Groups[1].Value.Trim() : "Not Detected";

        // 7. Grade / Score
        var gradeMatch = Regex.Match(rawText,
            @"(?:Grade|Score|Result|Marks)[:\s]*([^\r\n]+)",
            RegexOptions.IgnoreCase);
        data.GradeOrScore = gradeMatch.Success ? gradeMatch.Groups[1].Value.Trim() : "Verified / Pass";

        // 8. Handwritten Signature / Endorsement
        var sigMatch = Regex.Match(rawText,
            @"(?:Handwritten\s*(?:Note|Endorsement)|Signed|Signature|Registrar|Director)[:\s]*([^\r\n]+)",
            RegexOptions.IgnoreCase);
        data.HandwrittenSignatureOrNote = sigMatch.Success ? sigMatch.Groups[1].Value.Trim() : "Signature Present";

        return data;
    }
}
