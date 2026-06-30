using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestRPG.Client.Api;
using TestRPG.Client.Api.Settings;

namespace TestRPG.Client
{
    public sealed class HealthCheckBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ApiSettings apiSettings;

        private void Start()
        {
            if (apiSettings == null)
            {
                Debug.LogError("ApiSettings is not assigned.");
                return;
            }

            // キャンセルトークンを取得して、サーバーの状態を確認する非同期処理を開始
            var token = this.GetCancellationTokenOnDestroy();
            CheckHealthAsync(token).Forget();
        }

        /// <summary>
        /// api/health エンドポイントにリクエストを送ってサーバーの状態を確認
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async UniTaskVoid CheckHealthAsync(CancellationToken cancellationToken)
        {
            try
            {
                // HealthApiClient を作成して api/health エンドポイントにリクエストを送る
                var client = new HealthApiClient(apiSettings.BaseUrl);
                var status = await client.GetHealthAsync(cancellationToken);
                Debug.Log($"<color=green>Health API status: {status}</color>");
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Health API request was canceled.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Health API error: {ex.Message}");
            }
        }
    }
}
