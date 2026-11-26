# 100% Pure .NET 8 + React Offline Certificate OCR Demo (Zero Python)

A clean, self-contained demo project built strictly with **ASP.NET Core 8 (.NET 8)** and **React 18**—with **no Python dependency**—to demonstrate and compare offline OCR engines on uploaded certificate images or PDFs.

## Included Offline OCR Engines in Dropdown
1. **PaddleOCR (PP-OCRv4 Offline)** — Best all-rounder for stamps, seals, and mixed printed + handwritten text in .NET.
2. **Microsoft TrOCR (Offline ONNX Transformer)** — Best for cursive names and handwritten signatures via `Microsoft.ML.OnnxRuntime`.
3. **Tesseract OCR v5 (LSTM Offline)** — Ultra-fast CPU printed-text extraction via native .NET `Tesseract` NuGet (`PdfPig` included for PDFs).
4. **Windows Native Media.Ocr** — Built-in Windows 10/11 offline OCR engine.
5. **EasyOCR (CRAFT + CRNN Offline ONNX)** — Strong on skewed mobile photos and multi-language certificates.
6. **Mindee docTR (DBNet + ViT Offline ONNX)** — Sharp bounding-box localization for structured certificates.

## How to Run in 30 Seconds

### Option A: Run Full .NET 8 Web API + React UI
```bash
cd OcrDemo.Api
dotnet restore
dotnet run --urls "http://localhost:5000"
```
Open **http://localhost:5000** in your browser (or **http://localhost:5000/swagger** for Swagger API docs).

### Option B: Open React UI Directly (Instant Preview)
Double-click `react-ui/index.html` in Chrome or Edge.
