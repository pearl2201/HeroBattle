using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Application.Common.Interfaces
{
    public interface IStorageService
    {
        Task<UploadSimpleResult> IsFileExisted(string path);
        Task<UploadSimpleResult> MovePath(string srcPath, string dstPath);
        Task<UploadSimpleResult> UploadFormFile(string path, IFormFile file);

        Task<UploadSimpleResult> GetUrlFromPath(string path);

        Task RemoveFileFromPath(string bucket, string path);

        Task<UploadSimpleResult> UploadPrivateText(string bucket, string path, string text);

        Task<UploadSimpleResult> UploadPubtext(string bucket, string path, string text);

        Task<UploadSimpleResult> UploadPrivateFormFile(string bucket, string path, IFormFile file);

        Task<UploadSimpleResult> GetUrlFromPrivatePath(string bucket, string path);

        Task<UploadSimpleResult> ReloadBucket(string bucketDistributionId);
        Task<UploadSimpleResult> ReloadBucket(string bucketDistributionId, string distributionPath);

        Task<UploadSimpleResult> CreateUploadUrl(string bucket, string path, double duration = 1);

        Task<UploadSimpleResult> ReloadBucket(string bucketDistributionId, List<string> distributionPaths);
    }

    public class UploadSimpleResult
    {
        public string Path { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public int ErrorCode { get; set; }
    }
}
