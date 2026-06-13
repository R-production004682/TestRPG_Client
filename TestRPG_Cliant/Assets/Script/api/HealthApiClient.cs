using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace TestRPG.Cliant.Api
{
    public sealed class HealthApiClient
    {
        private readonly string baseUrl;

        public HealthApiClient(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));
            }

            this.baseUrl = baseUrl.TrimEnd('/');
        }

        /// <summary>
        /// 【環境疎通】
        /// api/health エンドポイントに GET リクエストを送信し、
        /// サーバーのヘルスステータスを取得
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        public async Task<string> GetHealthAsync (CancellationToken cancellationToken)
        {
            using var request = UnityWebRequest.Get($"{baseUrl}/health");

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