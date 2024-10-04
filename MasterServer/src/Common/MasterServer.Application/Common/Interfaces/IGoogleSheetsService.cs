using Google.Apis.Sheets.v4.Data;

namespace MasterServer.Application.Common.Interfaces
{
    public interface IGoogleSheetsService
    {
        Task<IList<ValueRange>> GetBatchSheetDataAsync(string spreadSheetId, List<string> ranges);
    }
}
