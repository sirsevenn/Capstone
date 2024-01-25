using UnityEngine;

public class EnemyData : MonoBehaviour{

	[SerializeField] NameList.Enemy enemy;

    private int AtkData = 0;
    private int DefData = 0;
    private int SkillData = 0;

    public int GetTotalData(){
        return AtkData + DefData + SkillData;
    }

    public void Zero(){
        AtkData = 0;
        DefData = 0;
        SkillData = 0;
    }

	public void UpdateData(ActionType action, int amt){
		switch(action){
			case ActionType.Attack:
				this.AtkData += amt;
				break;
			case ActionType.Defend:
				this.DefData += amt;
				break;
			case ActionType.Skill:
				this.SkillData += amt;
				break;
		}
	}

	public int GetData(ActionType action){

		switch(action){
			case ActionType.Attack:
				return this.AtkData;
			case ActionType.Defend:
				return this.DefData;
			case ActionType.Skill:
				return this.SkillData;
			default:
				Debug.Log("Unable to retrieve data");
				return 0;
		}
	}
}
