using DG.Tweening;
using System;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AnimationHandler))]
public class HO_PlayerAI : HO_EntityAI
{
    [SerializeField] private AnimationHandler animationHandler;

    [Space(10)] [Header("Decision Weights Properties")]
    [SerializeField] private int hpPotionWeight;
    [SerializeField] private int defPotionWeight;
    [SerializeField] private int spellPotionWeight;

    [Space(10)] [Header("Spell and Effects Properties")]
    [SerializeField] private ElementalSpellSO currentSpellSO;
    [SerializeField] private ParticleSystem healingEffect;
    [SerializeField] private GameObject defenseForceField;


    protected override void Awake()
    {
        base.Awake();
        animationHandler = GetComponent<AnimationHandler>();
        healthBar = GetComponent<WorldSpaceHealthBar>();
    }

    private void Start()
    {
        currentSpellSO = null;
        defenseForceField.SetActive(false);
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

    public override void OnEntityTurn(EElementalAttackType weakToElement, EElementalAttackType resistantToElement)
    {
        // Reset DEF stat and attack valuues before each turn
        characterStats.ResetDEFToBase();
        currentAttackDMG = 0;
        attackElementalType = EElementalAttackType.Unknown;

        // Get the number of each potions
        int numHealthPotions = InventorySystem.Instance.GetConsumableAmount(EConsumableType.Health_Potion);
        int numDefensePotions = InventorySystem.Instance.GetConsumableAmount(EConsumableType.Defense_Potion);
        int numSpellPotions = InventorySystem.Instance.GetConsumableAmount(EConsumableType.Fire_Potion) +
            InventorySystem.Instance.GetConsumableAmount(EConsumableType.Water_Potion) +
            InventorySystem.Instance.GetConsumableAmount(EConsumableType.Earth_Potion);

        // Decide what item to use,if there are any available consumables
        Consumable item = null;
        float hpInPercent = characterStats.GetCurrentHPInPercent();

        if (hpInPercent < 0.3 && numHealthPotions > 0)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Health_Potion);
        }
        else if (hpInPercent < 0.3 && numDefensePotions > 0)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Defense_Potion);
        }
        else if (hpInPercent > 0.7 && numSpellPotions > 0)
        {
            item = PickSpellPotion(weakToElement, resistantToElement);
        }
        else
        {
            float hpPotionFinalWeight = numHealthPotions > 0 ? hpPotionWeight : 0;
            float defPotionFinalWeight = numDefensePotions > 0 ? defPotionWeight : 0;
            float spellPotionFinalWeight = numSpellPotions > 0 ? spellPotionWeight : 0;
            float totalWeight = hpPotionFinalWeight + defPotionFinalWeight + spellPotionFinalWeight;

            if (totalWeight != 0)
            {
                float randItem = UnityEngine.Random.Range(0, totalWeight);

                if (randItem < hpPotionFinalWeight)
                {
                    item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Health_Potion);
                }
                else if (randItem < hpPotionFinalWeight + defPotionFinalWeight)
                {
                    item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Defense_Potion);
                }
                else
                {
                    item = PickSpellPotion(weakToElement, resistantToElement);
                }
            }
        }

        // Consume item and decide how player attacks
        if (item != null)
        {
            InventorySystem.Instance.ConsumeConsumable(item.ItemID);

            if (item.ConsumableData.ConsumableType  == EConsumableType.Health_Potion)
            {
                characterStats.Heal(item.ConsumableData.NumberValue);
                healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
                healingEffect.Play();
            }
            else if (item.ConsumableData.ConsumableType == EConsumableType.Defense_Potion)
            {
                characterStats.ModifyDEF(item.ConsumableData.NumberValue);
                defenseForceField.SetActive(true);
            }

            currentSpellSO = item.ConsumableData as ElementalSpellSO;
        }

        currentAttackDMG = (currentSpellSO != null) ? item.ConsumableData.NumberValue : characterStats.GetTotalATK();
        attackElementalType = (currentSpellSO != null) ? currentSpellSO.ElementalType : EElementalAttackType.Unknown;
    }

    private Consumable PickSpellPotion(EElementalAttackType weakToElement, EElementalAttackType resistantToElement)
    {
        // Determine the third element that will have no damage modifiers
        EElementalAttackType normalElement = EElementalAttackType.Unknown;
        foreach (EElementalAttackType elememnt in Enum.GetValues(typeof(EElementalAttackType)))
        {
            if (elememnt != EElementalAttackType.Unknown && elememnt != weakToElement && elememnt != resistantToElement)
            {
                normalElement = elememnt;
                break;
            }
        }

        // Return one spell based on priority
        Consumable returnSpell = InventorySystem.Instance.GetOneSpellOfType(weakToElement);
        if (returnSpell != null) return returnSpell;

        returnSpell = InventorySystem.Instance.GetOneSpellOfType(normalElement);
        if (returnSpell != null) return returnSpell;

        returnSpell = InventorySystem.Instance.GetOneSpellOfType(resistantToElement);
        return returnSpell;
    }

    public override void EntityTakeDamage(int damage, EElementalAttackType attackElementalType)
    {
        base.EntityTakeDamage(damage, attackElementalType);
        healthBar.UpdateHP(characterStats.GetCurrentHPInPercent());
        defenseForceField.SetActive(false);
    }
}