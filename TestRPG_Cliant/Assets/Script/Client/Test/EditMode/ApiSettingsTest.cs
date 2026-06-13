using UnityEngine;
using NUnit.Framework;
using TestRPG.Client.Api.Settings;

namespace TestRPG.Client.Tests.EditMode
{
    public sealed class ApiSettingsTest
    {
        /// <summary>
        /// 内容 : 末尾のスラッシュは削除されることを確認する
        /// 期待挙動 : "http://localhost5000/api/" -> "http://localhost5000/api"
        /// </summary>
        [Test]
        public void BaseUrl_EnsureThatTrailingSlashIsRemoved()
        {
            // Arrange
            var settings = ScriptableObject.CreateInstance<ApiSettings>();

            JsonUtility.FromJsonOverwrite("{\"baseUrl\" : \"http://localhost5000/api/\"}", settings);

            // Act & Assert
            Assert.AreEqual("http://localhost5000/api", settings.BaseUrl);
        }

        /// <summary>
        /// 内容 : BaseUrlが空文字の場合、例外が発生せずに空文字を返すことことを確認する
        /// 期待挙動 : "" -> ""
        /// </summary>
        [Test]
        public void BaseUrl_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            var settings = ScriptableObject.CreateInstance<ApiSettings>();

            JsonUtility.FromJsonOverwrite("{\"baseUrl\" : \"\"}", settings);
            // Act & Assert
            Assert.AreEqual(string.Empty, settings.BaseUrl);
        }
    }
}
