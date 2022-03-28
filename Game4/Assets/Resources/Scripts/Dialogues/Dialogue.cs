using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnswerTypes
{
    next_L = 0,
    exit = 1,
    shop = 2,
    healHp_F = 3,
    healMp_F = 4,
    healAll_F = 5,
    giveItem_L_F = 6,
    finish = 7
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(2, 5)]
    public string dialogueDescription;
    public string npcName;
    public Sprite npcImage;

    public Replicas[] replicas;

}

[System.Serializable]
public struct Replicas
{
    [TextArea(2, 5)]
    public string[] replicaText;
    public Answers[] answers;

    [System.Serializable]
    public struct Answers
    {
        public string answerText;
        public AnswerTypes answerType;
        public int link;

    }
}