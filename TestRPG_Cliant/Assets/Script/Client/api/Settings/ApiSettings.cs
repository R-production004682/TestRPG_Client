using UnityEngine;

namespace TestRPG.Client.Api.Settings
{
    [CreateAssetMenu(
        fileName = "ApiSettings",
        menuName = "TestRPG/Settings/API Settings")]
    public sealed class ApiSettings : ScriptableObject
    {
        [SerializeField]
        private string baseUrl = "http://192.168.11.7:5000";

        public string BaseUrl => baseUrl.TrimEnd('/');
    }
}
