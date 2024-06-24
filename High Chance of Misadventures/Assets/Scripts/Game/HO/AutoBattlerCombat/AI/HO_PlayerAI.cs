using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AnimationHandler))]
public class HO_PlayerAI : HO_EntityAI
{
    [SerializeField] private AnimationHandler animationHandler;

    [Space(10)] [Header("Decision Weights Properties")]
    [SerializeField] private int hpPotionWeight;
    [SerializeField] private int defPotionWeight;
    [SerializeField] private int fireScrollWeight;
    [SerializeField] private int waterScrollWeight;
    [SerializeField] private int earthScrollWeight;

    [Space(10)] [Header("Spell Properties")]
    [SerializeField] private ElementalSpellSO currentSpellSO;


    protected override void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        healthBar = GetComponent<WorldSpaceHealthBar>();
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

    public override void TriggerAttackAnimation(Vector3 opponentPos, float meleeDistanceOffset, float animDuration)
    {
        if (currentSpellSO == null)
        {
            Vector3 offsetDir = transform.position - opponentPos;
            offsetDir.Normalize();

            animationHandler.PlayAnimation(ActionType.Heavy);
            transform.DOJump(opponentPos + offsetDir * meleeDistanceOffset, 1, 1, animDuration).SetEase(Ease.Linear);
        }
        else
        {
            GameObject spellObj = GameObject.Instantiate(currentSpellSO.ProjectilePrefab, transform);
            spellObj.transform.position += new Vector3(0, 1f, 0);

            ElementalSpell spell = spellObj.GetComponent<ElementalSpell>();
            StartCoroutine(spell.FireProjectile(opponentPos + new Vector3(0, 1f, 0), animDuration));
        }
    }

    public override void TriggerEndAttackAnimation(Vector3 originalPos, float animDuration)
    {
        if (currentSpellSO == null)
        {
            transform.DOJump(originalPos, 1, 1, animDuration).SetEase(Ease.Linear);
        }
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
        attackElementalType = EElementalAttackType.Unknown;

        // Get final weights of the potions and scrolls
        int numHPPotion = InventorySystem.Instance.GetConsumableAmount(EConsumableType.Health_Potion);

        float hpPotionFinalWeight = CalculateFinalWeight(hpPotionWeight, numHPPotion, true);
        float defPotionFinalWeight = CalculateFinalWeight(defPotionWeight, InventorySystem.Instance.GetConsumableAmount(EConsumableType.Defense_Potion));
        float fireScrollFinalWeight = CalculateFinalWeight(fireScrollWeight, InventorySystem.Instance.GetConsumableAmount(EConsumableType.Fire_Potion));
        float waterScrollFinalWeight = CalculateFinalWeight(waterScrollWeight, InventorySystem.Instance.GetConsumableAmount(EConsumableType.Water_Potion));
        float earthScrollFinalWeight = CalculateFinalWeight(earthScrollWeight, InventorySystem.Instance.GetConsumableAmount(EConsumableType.Earth_Potion));

        // Decide what item to use
        Consumable item = null;
        float totalSum = hpPotionFinalWeight + defPotionFinalWeight + fireScrollFinalWeight + waterScrollFinalWeight + earthScrollFinalWeight;
        float randItem = Random.Range(0, totalSum);

        if (randItem < hpPotionFinalWeight || (characterStats.GetCurrentHPInPercent() < 0.25 && numHPPotion > 0))
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Health_Potion);
        }
        else if (randItem < hpPotionFinalWeight + defPotionFinalWeight)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Defense_Potion);
        }
        else if (randItem < totalSum - waterScrollFinalWeight - earthScrollFinalWeight)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Fire_Potion);
        }
        else if (randItem < totalSum - earthScrollFinalWeight)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Water_Potion);
        }
        else if (randItem < totalSum)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Earth_Potion);
        }


        // Consume item and decide how player attacks
        if (item != null)
        {
            InventorySystem.Instance.ConsumeConsumable(item.ItemID);

            if (item.ConsumableData.ConsumableType  == EConsumableType.Health_Potion)
            {
                characterStats.Heal(item.ConsumableData.NumberValue);
                healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
            }
            else if (item.ConsumableData.ConsumableType == EConsumableType.Defense_Potion)
            {
                characterStats.ModifyDEF(item.ConsumableData.NumberValue);
            }

            currentSpellSO = item.ConsumableData as ElementalSpellSO;
        }

        currentAttackDMG = (currentSpellSO != null) ? item.ConsumableData.NumberValue : characterStats.GetTotalATK();
        attackElementalType = (currentSpellSO != null) ? currentSpellSO.ElementalType : EElementalAttackType.Unknown;
    }

    private float CalculateFinalWeight(int baseWeight, int numItem, bool isHPPotion = false)
    {
        float finalWeight = baseWeight * ((Mathf.Pow(2, numItem - 1) - 1) / Mathf.Pow(2, numItem - 1) + 1);
        return (!isHPPotion || isHPPotion && characterStats.GetCurrentHPInPercent() < 0.75) ? finalWeight : 0f;
    }

    public override void EntityTakeDamage(int damage, EElementalAttackType attackElementalType)
    {
        base.EntityTakeDamage(damage, attackElementalType);
        healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
    }
}