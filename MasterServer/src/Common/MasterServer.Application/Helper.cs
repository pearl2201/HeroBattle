using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace MasterServer.Application
{
    public class Helper
    {
        public static Dictionary<string, object> MakeFirebaseJsonDiff(Object oldObj, Object newObj)
        {
            var oldNormalize = NormalizeObjectData(oldObj);
            var newNormalize = NormalizeObjectData(newObj);
            var addKeys = newNormalize.Select(x => x.Key).Except(oldNormalize.Select(y => y.Key));
            var removeKeys = newNormalize.Select(x => x.Key).Except(oldNormalize.Select(y => y.Key));
            var maybeChangeKeys = newNormalize.Where(x => !addKeys.Contains(x.Key) && !removeKeys.Contains(x.Key));
            Dictionary<string, object> diffDict = new Dictionary<string, object>();
            foreach (var key in addKeys)
            {
                diffDict[key] = newNormalize[key];
            }
            foreach (var key in removeKeys)
            {
                diffDict[key] = null;
            }
            foreach (var key in maybeChangeKeys)
            {
                if (oldNormalize[key.Key] != newNormalize[key.Key])
                {
                    diffDict[key.Key] = newNormalize[key.Key];
                }
            }
            return diffDict;
        }
        public static Dictionary<string, object> NormalizeObjectData(Object data)
        {
            Dictionary<string, object> path = new Dictionary<string, object>();
            var left = JToken.Parse(JsonConvert.SerializeObject(data));
            ConstructNormalizeData(left, "", path);
            return path;
        }

        public static void ConstructNormalizeData(JToken token, string currentpath, Dictionary<string, object> snapshots)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    {
                        var current = (JObject)token;
                        var keys = current.Properties();
                        foreach (var key in keys)
                        {
                            ConstructNormalizeData(key, key.Path, snapshots);
                        }
                        break;
                    }
                case JTokenType.Array:
                    {
                        foreach (var elem in (JArray)token)
                        {
                            ConstructNormalizeData(elem, elem.Path, snapshots);
                        }
                        break;
                    }
                case JTokenType.Property:
                    {
                        if (((JProperty)token).Value is JToken jo)
                        {
                            ConstructNormalizeData(jo, jo.Path, snapshots);
                        }
                        else
                        {
                            snapshots[token.Path] = ((JProperty)token).Value;
                        }


                        break;
                    }
                case JTokenType.Integer:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<int>();
                        break;
                    }
                case JTokenType.String:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<string>();
                        break;
                    }
                case JTokenType.Comment:
                    {


                        break;
                    }
                case JTokenType.TimeSpan:
                    {
                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<TimeSpan>();

                        break;
                    }
                case JTokenType.Boolean:
                    {
                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<Boolean>();

                        break;
                    }
                case JTokenType.Null:
                    {
                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = null;

                        break;
                    }
                case JTokenType.Bytes:
                    {
                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<byte[]>();

                        break;
                    }
                case JTokenType.Constructor:
                    {


                        break;
                    }
                case JTokenType.Date:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<DateTime>();
                        break;
                    }
                case JTokenType.Float:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<float>();
                        break;
                    }
                case JTokenType.Guid:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<Guid>();
                        break;
                    }
                case JTokenType.None:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = null;
                        break;
                    }
                case JTokenType.Raw:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<string>();
                        break;
                    }
                case JTokenType.Undefined:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = null;
                        break;
                    }
                case JTokenType.Uri:
                    {

                        snapshots[Regex.Replace(token.Path.Replace(".", "/"), @"\[(\d+)\]", @"/$1")] = token.ToObject<string>();
                        break;
                    }
                default:

                    break;
            }
        }
    }

}

