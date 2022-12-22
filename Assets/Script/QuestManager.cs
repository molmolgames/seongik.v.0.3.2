using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    public void GenerateData()
    {
        //questList.Add(questid , int 형 -> quest npc id)
        questList.Add(10, new QuestData("고양이한테 먹을것 주기", new int[] { 14 }));
        questList.Add(20, new QuestData("열쇠 모으기", new int[] { 15 }));
    }
    
    public int GetQusetTalkIndex(int npcId)
    {
        return questId+ questActionIndex;
    }
    public string CheckQuset(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;           
        }
        //if(questActionIndex == questList[questId].npcId.Length)
        //{
        //    NextQuest();
        //}
        return questList[questId].questName;
    }
    public void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
}
