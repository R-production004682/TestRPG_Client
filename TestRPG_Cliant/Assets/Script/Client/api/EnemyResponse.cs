using System;

namespace TestRPG.Client.Api
{
    /// <summary>
    /// サーバーから帰ってくる敵データを受け取る
    /// </summary>
    [Serializable]
    public sealed class EnemyResponse
    {
        public int id;
        public string name;
        public int lv;
        public int maxHp;
        public int atk;
        public int def;
        public int agi;
        public int evasionRate;
        public int criticalRate;
        public int expReward;
        public int goldReward;
        public int areaNo;
        public int enemyType;
        public int escapeType;
    }

    /*
     * 【 NOTE 】
        サーバー側では、MaxHp のように書いていたが、
        APIレスポンスのJSONでは通常 maxHp のような camelCase になるので、
        JSON の名前と、フィールド名が一致するように camelCase で定義
     */
}
