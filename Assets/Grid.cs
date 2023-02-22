using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Grid : MonoBehaviour
{
    private readonly float _movePeriod = 0.5f;
    private float _time = 0f;
    private readonly int[,] _gridArray = new int[18, 18];
    private int _direction = 1;
    private int _nextDirection = 1;
    private readonly Queue<Vector2Int> _snake = new Queue<Vector2Int>();
    private Vector2Int _snakePos = new Vector2Int(3, 3);
    private bool _pointCollected = false;

    public int[,] GridArray => _gridArray;

    public Action OnGridUpdate;

    private void Start()
    {
        FillWalls();
        InitiateSnake();
        GeneratePoint();
        UpdateGrid();
    }

    private void Update()
    {
        SetDirection();
        _time += Time.deltaTime;
        if (_time >= _movePeriod)
        {
            _time = 0;
            UpdateGrid();
        }
    }

    private void InitiateSnake()
    {
        _snake.Enqueue(new Vector2Int(1,3));
        _snake.Enqueue(new Vector2Int(2,3));
        _snake.Enqueue(new Vector2Int(3,3));
    }
    
    private void FillWalls()
    {
        for (int i = 17; i >= 0; i--)
        {
            for (int j = 17; j >= 0; j--)
            {
                if (i is 0 or 17 || j is 0 or 17)
                {
                    _gridArray[j, i] = 2;
                }
            }
        }
    }

    private void GeneratePoint()
    {
        Random rnd = new Random();
        _gridArray[rnd.Next(1, 17), rnd.Next(1, 17)] = 3;
    }
    private void UpdateGrid()
    {
        CheckDirection();
        _snake.Enqueue(_snakePos);
        if (!_pointCollected)
        {
            Vector2Int last = _snake.Dequeue();
            _gridArray[last.x, last.y] = 0;
        }
        else
        {
            GeneratePoint();
            _pointCollected = false;
        }
        foreach (var snk in _snake)
        {
            _gridArray[snk.x, snk.y] = 1;
        }
        
        OnGridUpdate?.Invoke();
    }

    private void CheckDirection()
    {
        _direction = _nextDirection;
        if (_direction == 1)
        {
            _snakePos.x += 1;
        }
        else if (_direction == 2)
        {
            _snakePos.y += 1;
        }
        else if (_direction == 3)
        {
            _snakePos.x -= 1;
        }
        else if (_direction == 4)
        {
            _snakePos.y -= 1;
        }

        if (_gridArray[_snakePos.x, _snakePos.y] == 1 || _gridArray[_snakePos.x, _snakePos.y] == 2)
        {
            GameOver();
        }
        else if (_gridArray[_snakePos.x, _snakePos.y] == 3)
        {
            _gridArray[_snakePos.x, _snakePos.y] = 0;
            _pointCollected = true;
        }
    }
    
    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (_direction != 4)
            {
                _nextDirection = 2;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (_direction != 2)
            {
                _nextDirection = 4;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (_direction != 1)
            {
                _nextDirection = 3;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_direction != 3)
            {
                _nextDirection = 1;
            }
        }
    }
}
