using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

//형식 매개변수 TGridObject
public class GridXZ<TGridObject> {


    //가로
    private int width;
    //세로
    private int height;
    //정사각형 cell 한개의 변길이
    private float cellSize;
    //cell이 만들어지기 시작하는 지점
    private Vector3 originPosition;
    //객체를 생성할 때 입력받은 형식으로 치환  
    //cell의 2차원 배열
    private TGridObject[,] gridArray;

    //클래스 생성자
    //Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject 의 형식 매개변수중 마지막은 반환 형식
    public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> createGridObject) 
    {
        //매개변수와 이름이 같은것은 this를 통해 모호성 제거
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        //cell 2차원 배열 초기화
        gridArray = new TGridObject[width, height];
        //cell 2차원 배열 할당
        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int z = 0; z < gridArray.GetLength(1); z++) {
                //Func를 통해 GridObject세팅
                gridArray[x, z] = createGridObject();
            }                                             
        }

    }
    //가로 길이 Get
    public int GetWidth() {
        return width;
    }
    //세로 길이 Get
    public int GetHeight() {
        return height;
    }
    //cell 한변 길이 Get
    public float GetCellSize() {
        return cellSize;
    }
    //2차원 좌표를 3차원 좌표로 반환
    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y) * cellSize + originPosition;
    }
    //3차원좌표를 버림을 통해 그리드 좌측 하단의 꼭지점 좌표로
    public void GetVertext(Vector3 worldPosition, out int x, out int z) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }
    public void GetVertext(Vector3 worldPosition, out Vector2Int gridPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        gridPosition = new Vector2Int(x, z);
    }
    //그리드 칸 반환
    public TGridObject GetGridObject(int x, int z) {
        if (x >= 0 && z >= 0 && x < width && z < height) {
            return gridArray[x, z];
        } else {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, z;
        GetVertext(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    //좌표가 범위 안에 있는지 확인
    public bool IsValidGridPosition(Vector2Int gridPosition) {
        int x = gridPosition.x;
        int z = gridPosition.y;

        if (x >= 0 && z >= 0 && x < width && z < height) {
            return true;
        } else {
            return false;
        }
    }


}
