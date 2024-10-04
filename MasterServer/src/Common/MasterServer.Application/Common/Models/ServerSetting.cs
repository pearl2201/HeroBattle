using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Application.Common.Models
{
    public class ServerSetting
    {
        public required string BucketAwsStorage { get; set; }

        public required string StorageDistributionUrl { get; set; }

        public required string StorageDistributionId { get; set; }

        public required string Secret { get; set; }

        public required string ValidIssuer { get; set; }

        public required string GCloudServiceAccountKeyPath { get; set; }

        public required string GoogleClientId { get; set; }

        public string GetBucketDistributeUrl(string path)
        {
            return (new Uri(new Uri(StorageDistributionUrl), path)).ToString();
        }

        public string GetBucketAdminImagePath(string path)
        {
            return "media/admin/" + path;
        }
    }
}
