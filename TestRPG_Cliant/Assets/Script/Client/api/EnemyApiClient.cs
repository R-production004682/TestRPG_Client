using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace TestRPG.Client.Api
{
    public sealed class EnemyApiClient
    {
        private readonly string _baseUrl;

        public EnemyApiClient(string baseUrl)
        {
            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Base URL must be an absolute HTTP or HTTPS URL.", nameof(baseUrl));
            }

            _baseUrl = baseUrl.TrimEnd('/');
        }

        /// <summary>
        /// 敵マスター一覧を非同期で取得
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async UniTask<EnemyResponse[]> GetEnemiesAsync(CancellationToken cancellationToken)
        {
            var url = $"{_baseUrl}{ApiRoutes.MasterEnemies}";

            using var request = UnityWebRequest.Get(url);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(
                    $"Enemy API のリクエストに失敗しました。 StatusCode: {request.responseCode}, Error: {request.error}");
            }

            var json = request.downloadHandler.text;

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new Exception("Enemy API response is empty.");
            }

            return JsonHelper.FromJsonArray<EnemyResponse>(json);
        }
    }

    internal static class JsonHelper
    {
        /// <summary>
        /// 配列 JSON を指定の型の配列に変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T[] FromJsonArray<T>(string json)
        {
            var wrappedJson = $"{{\"items\":{json}}}";
            var wrapper = JsonUtility.FromJson<ArrayWrapper<T>>(wrappedJson);

            if (wrapper == null || wrapper.items == null)
            {
                throw new FormatException("API response is not a valid JSON array.");
            }

            return wrapper.items;
        }

        /// <summary>
        /// 配列をラップする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        private sealed class ArrayWrapper<T> 
        {
            public T[] items;
        }
    }

}
