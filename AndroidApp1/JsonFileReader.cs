using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Android.App;

namespace AndroidApp1
{
    /// <summary>
    /// 静态帮助类：用于从 Assets 读取 JSON 文件，并根据给定键查找其中的对象数组或值。
    /// </summary>
    public static class JsonFileReader
    {
        /// <summary>
        /// Enumerate all top-level property names (keys) in a JSON object.
        /// Used for auto-discovery of characters in studentdata.json.
        /// </summary>
        public static List<string> GetAllKeys(string assetFileName)
        {
            var keys = new List<string>();
            try
            {
                var json = ReadJsonFromAssets(assetFileName);
                if (string.IsNullOrEmpty(json)) return keys;

                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        keys.Add(prop.Name);
                    }
                }
            }
            catch { }
            return keys;
        }

        /// <summary>
        /// 从应用的 Assets 中读取指定文件的内容为字符串。
        /// </summary>
        public static string? ReadJsonFromAssets(string assetFileName)
        {
            try
            {
                var context = Application.Context;
                using var stream = context.Assets.Open(assetFileName);
                using var reader = new StreamReader(stream,Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 在指定 Assets 中的 json 文件中查找第一个键名为 key 的属性，并返回该属性的值字符串（去掉 JSON 引号）。
        /// 支持字符串、数组（返回第一个元素）、数值、布尔、对象（返回对象的原始 JSON）。
        /// 解析失败或未找到时返回 "fail"。
        /// </summary>
        public static string FindArrayByKey(string assetFileName, string key)
        {
            if (string.IsNullOrEmpty(assetFileName) || string.IsNullOrEmpty(key))
                return "fail";

            try
            {
                var json = ReadJsonFromAssets(assetFileName);
                if (string.IsNullOrEmpty(json))
                    return "fail";

                using var doc = JsonDocument.Parse(json);
                if (TryFindPropertyRecursive(doc.RootElement, key, out var found))
                {
                    switch (found.ValueKind)
                    {
                        case JsonValueKind.String:
                            return found.GetString() ?? "fail";
                        case JsonValueKind.Number:
                        case JsonValueKind.True:
                        case JsonValueKind.False:
                            return found.GetRawText();
                        case JsonValueKind.Array:
                            {
                                foreach (var item in found.EnumerateArray())
                                {
                                    if (item.ValueKind == JsonValueKind.String)
                                        return item.GetString() ?? "fail";
                                    else
                                        return item.GetRawText();
                                }
                                return "fail"; // 空数组
                            }
                        case JsonValueKind.Object:
                            return found.GetRawText();
                        case JsonValueKind.Null:
                        case JsonValueKind.Undefined:
                        default:
                            return "fail";
                    }
                }
            }
            catch (Exception)
            {
                // 解析失败或其他问题，返回 "fail"
            }

            return "fail";
        }

        /// <summary>
        /// 从指定的 Assets JSON 文件中读取给定键的值，并返回其 JSON 字符串形式。
        /// 支持字符串、数组、对象、数值、布尔等所有 JSON 类型。
        /// 解析失败或未找到时返回 null。
        /// </summary>
        /// <param name="assetFileName">Assets 中的 JSON 文件名</param>
        /// <param name="key">要查找的键</param>
        /// <returns>键对应的值的 JSON 字符串；若未找到或出错则返回 null</returns>
        public static string? GetValueByKey(string assetFileName, string key)
        {
            if (string.IsNullOrEmpty(assetFileName) || string.IsNullOrEmpty(key))
                return null;

            try
            {
                var json = ReadJsonFromAssets(assetFileName);
                if (string.IsNullOrEmpty(json))
                    return null;

                using var doc = JsonDocument.Parse(json);
                if (TryFindPropertyRecursive(doc.RootElement, key, out var found))
                {
                    return found.GetRawText();
                }
            }
            catch (Exception)
            {
                // 解析失败或其他问题，返回 null
            }

            return null;
        }

        static bool TryFindPropertyRecursive(JsonElement element, string key, out JsonElement propertyElement)
        {
            propertyElement = default;

            if (element.ValueKind == JsonValueKind.Object)
            {
                if (element.TryGetProperty(key, out var prop))
                {
                    propertyElement = prop;
                    return true;
                }

                foreach (var child in element.EnumerateObject())
                {
                    if (child.Value.ValueKind == JsonValueKind.Object || child.Value.ValueKind == JsonValueKind.Array)
                    {
                        if (TryFindPropertyRecursive(child.Value, key, out propertyElement))
                            return true;
                    }
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object || item.ValueKind == JsonValueKind.Array)
                    {
                        if (TryFindPropertyRecursive(item, key, out propertyElement))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
