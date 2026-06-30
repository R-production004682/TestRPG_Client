using System;
using NUnit.Framework;
using TestRPG.Client.Api;

namespace TestRPG.Client.Tests.EditMode
{
    public sealed class HealthApiClientTest
    {
        /// <summary>
        /// 内容 : BaseUrlがnullまたは空文字の場合、ArgumentExceptionがスローされることを確認する
        /// 期待挙動 : null または "" -> ArgumentException
        /// </summary>
        /// <param name="invalidUrl"></param>
        [TestCase(null)]
        [TestCase("")]
        [TestCase("localhost:5000")]
        [TestCase("ftp://localhost:5000")]
        public void Constructor_InvalidBaseUrl_ThrowsArgumentException(string invalidUrl)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new HealthApiClient(invalidUrl));
            Assert.That(ex.Message, Does.Contain("Base URL must be an absolute HTTP or HTTPS URL"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("localhost:5000")]
        [TestCase("ftp://localhost:5000")]
        public void EnemyApiClientConstructor_InvalidBaseUrl_ThrowsArgumentException(string invalidUrl)
        {
            var ex = Assert.Throws<ArgumentException>(() => new EnemyApiClient(invalidUrl));
            Assert.That(ex.Message, Does.Contain("Base URL must be an absolute HTTP or HTTPS URL"));
        }
    }
}
