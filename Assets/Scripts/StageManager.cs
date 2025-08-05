using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    private bool _isScrolling = false;

    [SerializeField] private float scrollXSpeed = 5f;
    [SerializeField] private float scrollXMaxSpeed = 5f;
    [SerializeField] private float scrollXAcceleration = 0.1f;
    [SerializeField] private float scrollXAccelerationDelay = 0.5f;
    [SerializeField] private float gapCoefficient = 0.6f;

    [SerializeField] private FloorController floorPrefab;
    [SerializeField] private float floorGeneratePosY = -4f;
    [SerializeField] private float floorGenerateThresholdX = 0f;
    [SerializeField] private float floorDestroyThresholdX = 0f;
    private List<FloorController> _floorList = new();

    [SerializeField] private ObstacleController obstaclePrefab;
    [SerializeField] private float obstacleGeneratePosY = -2.5f;
    [SerializeField] private float obstacleGenerateThresholdX = 0f;
    [SerializeField] private float obstacleDestroyThresholdX = 0f;
    [SerializeField] private float obstacleStartPosX = 15f;
    List<ObstacleController> _obstacleList = new();

    private void Awake()
    {
        var initialFloor = FindObjectOfType<FloorController>();

        if (initialFloor != null)
        {
            _floorList.Add(initialFloor);
        }
        else
        {
            _floorList.Add(Instantiate(floorPrefab, new Vector2(0, floorGeneratePosY), Quaternion.identity));
        }
    }

    public void StartScrollStage()
    {
        _obstacleList.Add(GenerateObstacle(obstacleStartPosX));
        StartCoroutine(AccelerationTimer());

        _isScrolling = true;
    }

    private void FixedUpdate()
    {
        if (!_isScrolling) return;

        var lastFloor = _floorList.LastOrDefault();
        if (lastFloor != null && lastFloor.PosX <= floorGenerateThresholdX)
        {
            var newFloor = Instantiate(floorPrefab,
                new Vector2(lastFloor.PosX + lastFloor.ScaleX / 2, floorGeneratePosY), Quaternion.identity);
            _floorList.Add(newFloor);
        }

        var firstFloor = _floorList.FirstOrDefault();
        if (firstFloor != null && firstFloor.PosX <= floorDestroyThresholdX)
        {
            _floorList.Remove(firstFloor);
            Destroy(firstFloor.gameObject);
        }

        var lastObstacle = _obstacleList.LastOrDefault();
        if (lastObstacle != null && lastObstacle.PosX + lastObstacle.ScaleX / 2 <= obstacleGenerateThresholdX)
        {
            _obstacleList.Add(GenerateObstacle(lastObstacle.PosX));
        }

        var firstObstacle = _obstacleList.FirstOrDefault();
        if (firstObstacle != null && firstObstacle.PosX <= obstacleDestroyThresholdX)
        {
            _obstacleList.Remove(firstObstacle);
            Destroy(firstObstacle.gameObject);
        }

        var scrollXVelocity = -scrollXSpeed * Time.fixedDeltaTime;
        foreach (var floor in _floorList)
        {
            floor.Move(new Vector2(scrollXVelocity, 0));
        }

        foreach (var obstacle in _obstacleList)
        {
            obstacle.Move(new Vector2(scrollXVelocity, 0));
        }

        MainSceneDataStore.Instance.score =
            Mathf.Min(MainSceneDataStore.Instance.score + -scrollXVelocity, 99999);
    }

    IEnumerator AccelerationTimer()
    {
        while (Mathf.Abs(scrollXSpeed - scrollXMaxSpeed) > scrollXAcceleration / 2)
        {
            yield return new WaitForSeconds(scrollXAccelerationDelay);
            scrollXSpeed = Mathf.Min(scrollXSpeed + scrollXAcceleration, scrollXMaxSpeed);
        }
    }

    private ObstacleController GenerateObstacle(float prevObstaclePosX)
    {
        var obstacle = Instantiate(obstaclePrefab, Vector2.zero, Quaternion.identity);

        var minGap = obstacle.ScaleX * scrollXSpeed + gapCoefficient * scrollXSpeed;

        var generatePos = new Vector2(prevObstaclePosX + Random.Range(minGap, minGap + obstacle.ScaleX * 2),
            obstacleGeneratePosY);

        obstacle.transform.position = generatePos;

        obstacle.SetTargetDinoType((DinoType)Enum.ToObject(typeof(DinoType),
            Random.Range(0, Enum.GetNames(typeof(DinoType)).Length)));
        return obstacle;
    }
}