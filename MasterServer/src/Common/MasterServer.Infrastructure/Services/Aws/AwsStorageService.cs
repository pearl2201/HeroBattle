using Amazon.CloudFront;
using Amazon.S3;
using Amazon.S3.Model;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Web;

namespace MasterServer.Infrastructure.Services.Aws
{
    public class AwsStorageService : IStorageService
    {
        private readonly IAmazonS3 _client;
        private readonly string bucketName;
        private readonly ServerSetting _serverSetting;
        private readonly ILogger<AwsStorageService> _logger;
        private readonly IAmazonCloudFront _cloudfrontClient;

        public AwsStorageService(IAmazonS3 client, IAmazonCloudFront cloudfrontClient, IOptions<ServerSetting> mkpSettings, ILogger<AwsStorageService> logger)
        {
            _client = client;
            _serverSetting = mkpSettings.Value;
            bucketName = _serverSetting.BucketAwsStorage;
            _logger = logger;
            _cloudfrontClient = cloudfrontClient;
        }

        public Task<UploadSimpleResult> GetUrlFromPath(string path)
        {
            return Task.FromResult(new UploadSimpleResult()
            {
                IsSuccess = true,
                Path = _serverSetting.GetBucketDistributeUrl(path)
            });
        }

        public async Task<UploadSimpleResult> IsFileExisted(string path)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {
                var response1 = await _client.GetObjectMetadataAsync(bucketName, path);
                ret.IsSuccess = true;
                ret.Path = _serverSetting.GetBucketDistributeUrl(path);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task<UploadSimpleResult> UploadFormFile(string path, IFormFile file)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = path,
                    InputStream = file.OpenReadStream(),
                    CannedACL = S3CannedACL.PublicRead,

                };

                PutObjectResponse response1 = await _client.PutObjectAsync(putRequest1);
                ret.IsSuccess = true;
                ret.Path = _serverSetting.GetBucketDistributeUrl(path);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public Task<UploadSimpleResult> GetUrlFromPrivatePath(string bucket, string path)
        {

            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {

                var response1 = _client.GeneratePreSignedURL(bucket, path, DateTime.UtcNow.AddMinutes(15), new Dictionary<string, object>());
                ret.IsSuccess = true;
                ret.Path = response1;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }
            return Task.FromResult(ret);
        }

        public async Task<UploadSimpleResult> UploadPrivateFormFile(string bucket, string path, IFormFile file)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = path,
                    InputStream = file.OpenReadStream(),
                    CannedACL = S3CannedACL.Private,

                };

                PutObjectResponse response1 = await _client.PutObjectAsync(putRequest1);
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }


        public async Task<UploadSimpleResult> UploadPrivateText(string bucket, string path, string json)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            try
            {
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = path,
                    InputStream = stream,
                    CannedACL = S3CannedACL.Private,

                };

                PutObjectResponse response1 = await _client.PutObjectAsync(putRequest1);
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task<UploadSimpleResult> UploadPubtext(string bucket, string path, string json)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            try
            {
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = path,
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead,

                };

                PutObjectResponse response1 = await _client.PutObjectAsync(putRequest1);
                ret.IsSuccess = true;
                ret.Path = _serverSetting.GetBucketDistributeUrl(path);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task RemoveFileFromPath(string bucket, string path)
        {

            try
            {

                var response1 = await _client.RestoreObjectAsync(bucket, path);

            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));




            }
            catch (Exception)
            {


            }

        }

        public async Task<UploadSimpleResult> MovePath(string srcPath, string dstPath)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {


                CopyObjectResponse response1 = await _client.CopyObjectAsync(bucketName, srcPath, bucketName, dstPath);
                //     a//wait _client.DeleteAsync(bucketName, srcPath, new Dictionary<string,object>());
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task<UploadSimpleResult> ReloadBucket(string bucketDistributionId)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {


                var response1 = await _cloudfrontClient.CreateInvalidationAsync(new Amazon.CloudFront.Model.CreateInvalidationRequest()
                {
                    DistributionId = bucketDistributionId,
                    InvalidationBatch = new Amazon.CloudFront.Model.InvalidationBatch(new Amazon.CloudFront.Model.Paths() { Items = new List<string>() { "*" }, Quantity = 1 }, DateTime.UtcNow.ToString())
                });
                //     a//wait _client.DeleteAsync(bucketName, srcPath, new Dictionary<string,object>());
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task<UploadSimpleResult> CreateUploadUrl(string bucket, string path, double duration = 1)
        {


            UploadSimpleResult ret = new UploadSimpleResult();
            var additionProperties = new Dictionary<string, object>();

            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = path,
                    Verb = HttpVerb.PUT,
                    Expires = DateTime.UtcNow.AddHours(duration),
                };
                request.Headers["x-amz-acl"] = "public-read";


                var presignedUrl = _client.GetPreSignedURL(request);
                ret.Path = presignedUrl;
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task<UploadSimpleResult> ReloadBucket(string bucketDistributionId, string distributionPath)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {


                var response1 = await _cloudfrontClient.CreateInvalidationAsync(new Amazon.CloudFront.Model.CreateInvalidationRequest()
                {
                    DistributionId = bucketDistributionId,
                    InvalidationBatch = new Amazon.CloudFront.Model.InvalidationBatch(new Amazon.CloudFront.Model.Paths() { Items = new List<string>() { "/" + string.Join("/", distributionPath.Split("/").Select(HttpUtility.UrlEncode)) }, Quantity = 1 }, DateTime.UtcNow.ToString())
                });
                //     a//wait _client.DeleteAsync(bucketName, srcPath, new Dictionary<string,object>());
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }

        public async Task<UploadSimpleResult> ReloadBucket(string bucketDistributionId, List<string> distributionPaths)
        {
            UploadSimpleResult ret = new UploadSimpleResult();
            try
            {
                var temp = distributionPaths.Select(distributionPath => "/" + string.Join("/", distributionPath.Split("/").Select(y => HttpUtility.UrlEncode(y)))).ToList();

                var response1 = await _cloudfrontClient.CreateInvalidationAsync(new Amazon.CloudFront.Model.CreateInvalidationRequest()
                {
                    DistributionId = bucketDistributionId,
                    InvalidationBatch = new Amazon.CloudFront.Model.InvalidationBatch(new Amazon.CloudFront.Model.Paths() { Items = temp, Quantity = distributionPaths.Count }, DateTime.UtcNow.ToString())
                });
                //     a//wait _client.DeleteAsync(bucketName, srcPath, new Dictionary<string,object>());
                ret.IsSuccess = true;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, String.Format("Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message));


                ret.Error = ServiceError.S3Error.Message;
                ret.ErrorCode = ServiceError.S3Error.Code;

            }
            catch (Exception)
            {

                ret.Error = ServiceError.IntergrationError.Message;
                ret.ErrorCode = ServiceError.IntergrationError.Code;
            }

            return ret;
        }
    }

}
