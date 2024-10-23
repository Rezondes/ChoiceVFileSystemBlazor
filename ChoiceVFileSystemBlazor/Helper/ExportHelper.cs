using System.Reflection;
using Microsoft.JSInterop;
using OfficeOpenXml;

namespace ChoiceVFileSystemBlazor.Helper;

public static class ExportHelper
{
    private static byte[] ToExcelData<T>(List<T>? data)
    {
        if (data == null || data.Count == 0) return Array.Empty<byte>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add(typeof(T).Name);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Header erstellen
        for (var i = 0; i < properties.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = properties[i].Name;
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        }

        // Datenreihen
        for (var i = 0; i < data.Count; i++)
        {
            for (var j = 0; j < properties.Length; j++)
            {
                worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(data[i])?.ToString() ?? string.Empty;
            }
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        
        worksheet.View.FreezePanes(2, 1);
        
        return package.GetAsByteArray();
    }
    
    public static async Task ExportToExcelAsync<T>(List<T>? data, string name, IJSRuntime jsRuntime)
    {
        var excelBytes = ToExcelData(data);
        var base64 = Convert.ToBase64String(excelBytes);
        var url = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}";
        await jsRuntime.InvokeVoidAsync("downloadFileFromUrl", $"{name}.xlsx", url);
    }
}