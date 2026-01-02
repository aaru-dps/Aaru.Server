using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Aaru.Server.Components.Admin.Pages.Reports;

public partial class Upload : ComponentBase
{
    private string _errorMessage;
    private byte[] _fileContent;

    private InputFile _fileInput;
    private bool      _fileSelected;
    private bool      _isUploading;
    private string    _selectedFileName;
    private long      _selectedFileSize;
    private string    _successMessage;
    [Inject]
    private HttpClient Http { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        _errorMessage   = null;
        _successMessage = null;
        _fileSelected   = false;

        try
        {
            IBrowserFile? file = e.File;

            if(file == null)
            {
                _errorMessage = "No file selected.";

                return;
            }

            if(!file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                _errorMessage = "Please select a valid JSON file (.json)";

                return;
            }

            const long maxFileSize = 10 * 1024 * 1024; // 10 MB

            if(file.Size > maxFileSize)
            {
                _errorMessage = $"File size exceeds the maximum allowed size of {FormatFileSize(maxFileSize)}";

                return;
            }

            _selectedFileName = file.Name;
            _selectedFileSize = file.Size;

            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);
            _fileContent = memoryStream.ToArray();

            _fileSelected = true;
        }
        catch(Exception ex)
        {
            _errorMessage = $"Error reading file: {ex.Message}";
        }
    }

    private async Task UploadFile()
    {
        if(!_fileSelected)
        {
            _errorMessage = "Please select a file first.";

            return;
        }

        _isUploading    = true;
        _errorMessage   = null;
        _successMessage = null;

        try
        {
            string jsonContent = Encoding.UTF8.GetString(_fileContent);

            using var                 content   = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var                       uploadUrl = NavigationManager.ToAbsoluteUri("/api/uploadreportv2").ToString();
            using HttpResponseMessage response  = await Http.PostAsync(uploadUrl, content);

            string? responseContent = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode && responseContent == "ok")
            {
                _successMessage   = "Report uploaded successfully!";
                _fileSelected     = false;
                _selectedFileName = null;
                _selectedFileSize = 0;
                _fileContent      = null;

                // Reset file input
                if(_fileInput != null)
                {
                    // Navigate away after a short delay to show success message
                    await Task.Delay(1500);
                    NavigationManager.NavigateTo("/admin/reports");
                }
            }
            else if(responseContent == "notstats")
                _errorMessage = "Invalid report format: The uploaded file is not a valid device report.";
            else
                _errorMessage = $"Upload failed: {responseContent ?? response.ReasonPhrase}";
        }
        catch(Exception ex)
        {
            _errorMessage = $"Error uploading file: {ex.Message}";
        }
        finally
        {
            _isUploading = false;
        }
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB"];
        double   len   = bytes;
        var      order = 0;

        while(len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}