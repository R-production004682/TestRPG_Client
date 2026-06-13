using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace TestRPG.Cliant.Api
{
    public sealed class HealthApiClient
    {
        private readonly string baseUrl;

        public HealthApiClient(string baseUrl)
        {
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        /// <summary>
        /// 【環境疎通】
        /// api/health エンドポイントに GET リクエストを送信し、
        /// サーバーのヘルスステータスを取得
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        public IEnumerator GetHealth (
            Action<HealthResponseDto> onSuccess,
            Action<string> onFailure)
        {
            using var request = UnityWebRequest.Get($"{baseUrl}/health");
            
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                onFailure?.Invoke(
                    $"Health API request failed: {request.responseCode} {request.error}");
                yield break;
            }

            var response = JsonUtility.FromJson<HealthResponseDto>(
                request.downloadHandler.text);

            onSuccess?.Invoke(response);
        }
    }

    [Serializable]
    public sealed class HealthResponseDto
    {
        public string status;
    }
}