using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/skel")]
public class TempSpecialSkillDataSO : ScriptableObject
{
    [SerializeField] private ETempSkillTypes skillType;
    [SerializeField] private string skillName;
    [TextArea(4, 10)] [SerializeField] private string skillDescription;

    public ETempSkillTypes GetSkillType() => skillType;

    public string GetSkillName() => skillName;

    public string GetSkillDescription() => skillDescription;
}
