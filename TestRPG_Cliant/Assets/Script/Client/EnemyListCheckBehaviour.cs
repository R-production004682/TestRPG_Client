using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TestRPG.Client.Api;
using TestRPG.Client.Api.Settings;
using UnityEngine;

namespace TestRPG.Client
{
    public sealed class EnemyListCheckBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ApiSettings apiSettings;

        private EnemyApiClient _enemyApiClient;

        private void Awake()
        {
            if (apiSettings == null)
            {
                Debug.LogError("ApiSettings is not assigned.");
                return;
            }

            _enemyApiClient = new EnemyApiClient(apiSettings.BaseUrl);
        }
        
        private void Start()
        {
            if (_enemyApiClient == null)
            {
                return;
            }

            ShowEnemiesAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// API から敵一覧を取得して、各敵の情報をログに出力させる
        /// </summary>
        private async UniTaskVoid ShowEnemiesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var enemies = await _enemyApiClient.GetEnemiesAsync(cancellationToken);

                if (enemies.Length == 0)
                {
                    Debug.Log("<color=yellow>Enemy master is empty or null.</color>");
                    return;
                }

                foreach (var enemy in enemies)
                {
                    Debug.Log(
                        $"<color=green>Enemy: {enemy.name} Lv:{enemy.lv} HP:{enemy.maxHp} ATK:{enemy.atk} DEF:{enemy.def}</color>");
                }
            }
            catch (OperationCanceledException)
            {
                // GameObject破棄時のキャンセルは正常終了として扱う。
            }
            catch (Exception exception)
            {
                Debug.LogError($"Enemy API error: {exception.Message}");
            }
        }
    }
}
