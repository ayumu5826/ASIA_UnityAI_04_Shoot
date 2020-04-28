using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Robot : Agent
{
    [Header("速度"), Range(1, 50)]
    public float speed = 10;

    /// <summary>
    /// 機器人剛體
    /// </summary>
    private Rigidbody rigRobot;
    /// <summary>
    /// 食物鋼體
    /// </summary>
    private Rigidbody rigGFood;
    private Rigidbody rigBFood;

    private void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigGFood = GameObject.Find("香蕉").GetComponent<Rigidbody>();
        rigBFood = GameObject.Find("漢堡").GetComponent<Rigidbody>();
    }

    
    /// <summary>
    /// 事件開始時：重新設定機器人與食物位置
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //重設機器人的加速度與角速度
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        
        //隨機機器人初始位置
        Vector3 posRobot = new Vector3(Random.Range(-3.0f, 3.0f), 0.2f, Random.Range(-3.0f, 3.0f));
        transform.position = posRobot;

        //隨機食物初始位置
        Vector3 posGFood = new Vector3(Random.Range(-3.5f, 3.5f), 0.6f, Random.Range(-3.5f, 3.5f));
        Vector3 posBFood = new Vector3(Random.Range(-3.5f, 3.5f), 0.6f, Random.Range(-3.5f, 3.5f));
        rigBFood.position = posBFood;
        rigGFood.position = posGFood;

        //尚未吃到食物
        GFood.complete = false;
        BFood.complete = false;
    }

    /// <summary>
    /// 收集觀測資料
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        //加入觀測資料：機器人、足球座標、機器人加速度  X、Z
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigGFood.position);
        sensor.AddObservation(rigBFood.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);
    }

    /// <summary>
    /// 動作：控制機器人與回饋
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        //使用參數控制機器人
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0];
        control.z = vectorAction[1];
        rigRobot.AddForce(control * speed);

        //球進球門:成功:加一分並結束
        if (GFood.complete)
        {
            SetReward(1);
            EndEpisode();
        }

        //機器人或球掉到地板下:失敗:扣一分並結束
        if (transform.position.y < 0 || BFood.complete )
        {
            SetReward(-1);
            EndEpisode();
        }

    }

    /// <summary>
    /// 探索：讓開發者測試環境
    /// </summary>
    /// <returns></returns>
    public override float[] Heuristic()
    {
        //提供開發者控制的方式
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
