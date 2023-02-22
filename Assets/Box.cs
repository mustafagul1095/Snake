using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    private Vector2Int coordiinates = new Vector2Int();
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        FindCoordinates();
        UpdateName();
        _meshRenderer = GetComponent<MeshRenderer>();
        

    }

    private void Start()
    {
        _grid = FindObjectOfType<Grid>();
        _grid.OnGridUpdate += GridUpdated;
    }
    
    private void FindCoordinates()
    {
        coordiinates.x = Mathf.RoundToInt(transform.position.x / 10);
        coordiinates.y = Mathf.RoundToInt(transform.position.z / 10);
    }

    private void UpdateName()
    {
        transform.name = coordiinates.ToString();
    }

    private void GridUpdated()
    {
        _meshRenderer.enabled = _grid.GridArray[coordiinates.x, coordiinates.y] > 0;
    }
}
