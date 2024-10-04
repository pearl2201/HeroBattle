using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Options;

namespace MasterServer.Infrastructure.Services
{
    public class GoogleSheetService : IGoogleSheetsService
    {
        public SheetsService Service { get; private set; }
        const string APPLICATION_NAME = "MasterServer";
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly ServerSetting _settings;
        public GoogleSheetService(IOptions<ServerSetting> appSettings)
        {
            _settings = appSettings.Value;
            var credential = GetCredentialsFromFile();
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }

        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(_settings.GCloudServiceAccountKeyPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            return credential;
        }

        public async Task<IList<ValueRange>> GetBatchSheetDataAsync(string spreadSheetId, List<string> ranges)
        {

            var batchGetRequest = Service.Spreadsheets.Values.BatchGet(spreadSheetId);
            batchGetRequest.Ranges = new Google.Apis.Util.Repeatable<string>(ranges);
            var batchValueRange = await batchGetRequest.ExecuteAsync();
            return batchValueRange.ValueRanges;
        }

        public ValueRange GetSheetData(string spreadSheetId, string range)
        {
            var appendRequest = Service.Spreadsheets.Values.Get(spreadSheetId, range);
            var valueRange = appendRequest.Execute();
            return valueRange;
        }
    }
}
