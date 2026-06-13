using UnityEngine;

namespace TestRPG.Client.Api.Settings
{
    [CreateAssetMenu(
        fileName = "ApiSettings",
        menuName = "TestRPG/Settings/API Settings")]
    public sealed class ApiSettings : ScriptableObject
    {
        [SerializeField]
        private string baseUrl = "http://localhost:5000/api";

        public string BaseUrl => baseUrl.TrimEnd('/');
    }
}