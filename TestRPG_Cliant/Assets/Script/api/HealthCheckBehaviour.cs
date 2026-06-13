using UnityEngine;
using TestRPG.Cliant.Api;
using TestRPG.Client.Api.Settings;

namespace TestRPG.Cliant
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

            var cliant = new HealthApiClient(apiSettings.BaseUrl);

            StartCoroutine(cliant.GetHealth(
                response => Debug.Log($"<color=green>Health API status: {response.status}</color>"),
                error => Debug.LogError($"Health API error: {error}")
            ));
        }
    }
}
