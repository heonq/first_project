using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAxis {x=0,z}

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] cubeSpawnPoints; // 큐브가 생성되는 포인트를 만들어준다 x와z 축을 움직이는 두개의 포인트를 만든다 transform으로 생
    [SerializeField]
    private Transform movingCubePrefab; // 움직이는 큐브의 프리팹을 선언 해준다 

    [field:SerializeField]
    public Transform LastCube { set; get; } // 
    public MovingCube currentCube { set; get; } = null;

    [SerializeField]
    private float colorWeight = 15.0f; // 컬러에 변화를 주는데 쓰는 변수 수가 작을 수록 컬러 변화가 적다 
    private int currentColorNumberOfTime = 5; // 이 변수가 0이 되면 컬러가 변화한다 
    private int maxColorNumberOfTime = 5; // 컬러 변화 카운트의 최대값 
    private MoveAxis moveAxis = MoveAxis.x; // 

    public void SpawnCube()
    {
        
        Transform clone = Instantiate(movingCubePrefab); //큐브프리팹을 만들고 clone으로 선언한다 

        if ( LastCube ==null||LastCube.name.Equals("StartCubeTop")) //라스트큐브가 비어있거나 이름이 스타트큐브탑일때,
        {
            clone.position = cubeSpawnPoints[(int)moveAxis].position; //큐브프리팹의 위치를 정한다
            

        }
        else //처음 생성하는게 아닐 경우 
        {
            float x = cubeSpawnPoints[(int)moveAxis].position.x; //x 값은 스폰포인트의 x값을 받는다 
            float z = cubeSpawnPoints[(int)moveAxis].position.z; //z 값은 스폰포인트의 z값을 받는다

            float y = LastCube.position.y + movingCubePrefab.localScale.y;//마지막 큐브의 y값에 큐브프리팹의 y값을 더해준다 

            clone.position = new Vector3(x, y, z);

        }

        clone.GetComponent<MeshRenderer>().material.color = GetRandomColor();

        clone.GetComponent<MovingCube>().Setup(this, moveAxis);

        moveAxis = (MoveAxis)(((int)moveAxis + 1) % cubeSpawnPoints.Length); // moveAxis를 반대편 포인트로 할당한

        //LastCube = clone; // 방금 생성한 큐브를 마지막 큐브로 할당한다 
        currentCube = clone.GetComponent<MovingCube>();
    }

    public void OnDrawGizmos()
    {
        for ( int i=0; i < cubeSpawnPoints.Length; ++ i )
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cubeSpawnPoints[i].transform.position, movingCubePrefab.localScale);
        }
    }

    private Color GetRandomColor()
    {
        Color color = Color.white; 

        if (currentColorNumberOfTime > 0 ) //카운트가 1 이상일 때   
        {
            float colorAmount = (1.0f / 255.0f) * colorWeight; // 코드에서 컬러값은 0에서 1이기 때문에 1/255에 컬러변화값을 곱해준다 
            color = LastCube.GetComponent<MeshRenderer>().material.color; // 컬러에 마지막 큐브의 컬러를 지정해준다 
            color = new Color(color.r - colorAmount, color.g - colorAmount, color.b - colorAmount); // 다음에 오는 큐브의 컬러를 컬러 변화값을 뺀 새 컬러로 지정해준다

            currentColorNumberOfTime--;

        }

        else
        {
            color = new Color(Random.value, Random.value, Random.value); //컬러 카운트가 0일 때 새로운 컬러를 지정해준다 
            currentColorNumberOfTime = maxColorNumberOfTime; // 새로운 컬러 지정 후 카운트를 초기화 시킨다 
        }
        return color;

    }



}
