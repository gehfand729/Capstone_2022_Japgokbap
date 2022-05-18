using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandom : MonoBehaviour
{
    int total = 0;
    List<Test>_List= new List<Test>();
    void Start()
    {
        for(int i = 0; i < _List.Count; i++){
            total += _List[i].weight;
        }
    }
    public Test randomTest(){
        int selectNum = 0;
        int weight = 0;
        selectNum = Mathf.RoundToInt(total* Random.Range(0.0f,1.0f));

        for(int i =0; i < _List.Count; i++){
            weight += _List[i].weight;
            if(selectNum <= weight){
                Debug.Log(_List[i].name);
                return _List[i];
            }
        }
        return null;
    }
}

public class Test{
    public int weight;
    public string name;
}
