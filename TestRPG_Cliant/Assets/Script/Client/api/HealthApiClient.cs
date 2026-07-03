using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TestRPG.Client.Api
{
    public sealed class HealthApiClient
    {
        private readonly string _baseUrl;

        public HealthApiClient(string baseUrl)
        {
            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Base URL must be an absolute HTTP or HTTPS URL.", nameof(baseUrl));
            }

            _baseUrl = baseUrl.TrimEnd('/');
        }

        /// <summary>
        /// 【環境疎通】
        /// api/health エンドポイントに GET リクエストを送信し、
        /// サーバーのヘルスステータスを取得
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async UniTask<string> GetHealthAsync(CancellationToken cancellationToken)
        {
            using var request = UnityWebRequest.Get($"{_baseUrl}{ApiRoutes.Health}");

            // キャンセル処理のサポート
            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Health API request failed: {request.responseCode} {request.error}");
            }

            // レスポンスの内容を検証
            var jsonText = request.downloadHandler.text;
            if (string.IsNullOrEmpty(jsonText))
            {
                throw new Exception("Health API response is empty.");
            }

            var response = JsonUtility.FromJson<HealthResponseDto>(jsonText);

            // 正常レスポンスの検証
            if (response == null || string.IsNullOrEmpty(response.status))
            {
                throw new Exception("Health API response format is invalid or status is missing.");
            }

            return response.status;
        }
    }

    [Serializable]
    internal sealed class HealthResponseDto
    {
        public string status;
    }
}
