using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestRPG.Client.Tests.PlayMode
{
    public sealed class HealthCheckBehaviourTest
    {
        /// <summary>
        /// 内容 : ApiSettingsが未設定の状態でStart()を呼び出してもクラッシュしないことを確認する
        /// 期待挙動 : エラーログが出力されるが、例外でクラッシュせずに正常に終了すること
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Start_WithoutApiSettings_DoesNotCrash()
        {
            // Arrange
            var go = new GameObject("HealthCheckTarget");
            var behaviour = go.AddComponent<HealthCheckBehaviour>();

            // エラーログが出力されることを期待値として登録
            LogAssert.Expect(LogType.Error, "ApiSettings is not assigned.");

            // Act
            yield return null;

            // Assert
            Assert.Pass("クラッシュせずに安全に終了しました。");

            Object.Destroy(go);
        }
    }
}