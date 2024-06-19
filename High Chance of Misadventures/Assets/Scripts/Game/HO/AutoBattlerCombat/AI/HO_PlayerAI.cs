using UnityEngine;

[RequireComponent(typeof(AnimationHandler))]
public class HO_PlayerAI : HO_EntityAI
{
    [SerializeField] private AnimationHandler animationHandler;

    [Space(10)] [Header("Decision Weights Properties")]
    [SerializeField] private int hpPotionWeight;
    [SerializeField] private int atkPotionWeight;
    [SerializeField] private int defPotionWeight;
    [SerializeField] private int fireScrollWeight;
    [SerializeField] private int waterScrollWeight;
    [SerializeField] private int earthScrollWeight;


    protected override void Awake()
    {
        characterStats.InitializeCharacterStat();

        animationHandler = GetComponent<AnimationHandler>();
    }

    public void EnterRoom()
    {
        if (animationHandler == null) animationHandler = GetComponent<AnimationHandler>();

        animationHandler.PlayerMove();
    }

    public void StopMove()
    {
        animationHandler.PlayerStopMove();
    }

    public override void TriggerAttackAnimation()
    {
        animationHandler.PlayAnimation(ActionType.Heavy);
    }

    public override void TriggerHurtAnimation()
    {
        
    }

    public override void TriggerDeathAnimation()
    {
        animationHandler.PlayDeathAnimation();
    }

    public override void OnEntityTurn()
    {
        // Reset DEF stat and attack valuues before each turn
        characterStats.ResetDEFToBase();
        currentAttackDMG = 0;
        isAttackElemental = false;

        // Get final weights of the potions and scrolls
        int numHPPotion = InventorySystem.Instance.GetPotionAmount(EPotionType.Health_Potion);

        float hpPotionFinalWeight = CalculateFinalWeight(hpPotionWeight, numHPPotion, true);
        float atkPotionFinalWeight = CalculateFinalWeight(atkPotionWeight, InventorySystem.Instance.GetPotionAmount(EPotionType.Attack_Potion));
        float defPotionFinalWeight = CalculateFinalWeight(defPotionWeight, InventorySystem.Instance.GetPotionAmount(EPotionType.Defense_Potion));
        float fireScrollFinalWeight = CalculateFinalWeight(fireScrollWeight, InventorySystem.Instance.GetScrollAmount(EElementalAttackType.Fire));
        float waterScrollFinalWeight = CalculateFinalWeight(waterScrollWeight, InventorySystem.Instance.GetScrollAmount(EElementalAttackType.Water));
        float earthScrollFinalWeight = CalculateFinalWeight(earthScrollWeight, InventorySystem.Instance.GetScrollAmount(EElementalAttackType.Earth));

        // Decide what item to use
        ConsumableItem item = null;
        bool isItemScroll = false;
        float totalSum = hpPotionFinalWeight + atkPotionFinalWeight + defPotionFinalWeight + fireScrollFinalWeight + waterScrollFinalWeight + earthScrollFinalWeight;
        float randItem = Random.Range(0, totalSum);

        if (randItem < hpPotionFinalWeight || (characterStats.GetCurrentHPInPercent() < 0.25 && numHPPotion > 0))
        {
            item = InventorySystem.Instance.GetOnePotionOfType(EPotionType.Health_Potion);
        }
        else if (randItem < hpPotionFinalWeight + atkPotionFinalWeight)
        {
            item = InventorySystem.Instance.GetOnePotionOfType(EPotionType.Attack_Potion);
        }
        else if (randItem < hpPotionFinalWeight + atkPotionFinalWeight + defPotionFinalWeight)
        {
            item = InventorySystem.Instance.GetOnePotionOfType(EPotionType.Defense_Potion);
        }
        else if (randItem < totalSum - waterScrollFinalWeight - earthScrollFinalWeight)
        {
            item = InventorySystem.Instance.GetOneScrollOfType(EElementalAttackType.Fire);
            isItemScroll = true;
        }
        else if (randItem < totalSum - earthScrollFinalWeight)
        {
            item = InventorySystem.Instance.GetOneScrollOfType(EElementalAttackType.Water);
            isItemScroll = true;
        }
        else if (randItem < totalSum)
        {
            item = InventorySystem.Instance.GetOneScrollOfType(EElementalAttackType.Earth);
            isItemScroll = true;
        }

        // Consume item and decide how player attacks
        if (item != null && item is Potion)
        {
            Potion potion = (Potion)item;
            InventorySystem.Instance.UsePotion(potion.ItemID, characterStats);

            if (potion.PotionData.PotionType == EPotionType.Health_Potion)
            {
                HO_AutoBattleUI.Instance.UpdatePlayerHP(characterStats.GetCurrentHPInPercent());
            }
        } 
        else if (item != null && item is ScrollSpell)
        {
            ScrollSpell scroll = (ScrollSpell)item;
            InventorySystem.Instance.UseScroll(scroll.ItemID);

            currentAttackDMG = scroll.FinalValue;
            isAttackElemental = true;
        }

        if (!isItemScroll)
        {
            currentAttackDMG = characterStats.GetTotalATK();
            isAttackElemental = false;
        }

        // Reset ATK stat after each turn
        characterStats.ResetATKToBase();
    }

    private float CalculateFinalWeight(int baseWeight, int numItem, bool isHPPotion = false)
    {
        float finalWeight = baseWeight * ((Mathf.Pow(2, numItem - 1) - 1) / Mathf.Pow(2, numItem - 1) + 1);
        return (!isHPPotion || isHPPotion && characterStats.GetCurrentHPInPercent() < 0.65) ? finalWeight : 0f;
    }

    public override void EntityTakeDamage(int damage, bool isElemental)
    {
        base.EntityTakeDamage(damage, isElemental);
        HO_AutoBattleUI.Instance.UpdatePlayerHP(characterStats.GetCurrentHPInPercent());
    }
}