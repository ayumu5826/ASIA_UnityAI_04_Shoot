using UnityEngine;

public class GFood : MonoBehaviour
{
    /// <summary>
    /// 機器人是否吃到食物
    /// </summary>
    public static bool complete;
    /// <summary>
    /// 觸發開始事件：碰到勾選　Is Trigger 物件會執行一次
    /// </summary>
    /// <param name="other">碰到的物件碰撞資訊</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "機器人")
        {
            //吃到食物
            complete = true;
        }
    }

}
