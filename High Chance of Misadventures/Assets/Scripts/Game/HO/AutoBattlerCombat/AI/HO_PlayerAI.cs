using DG.Tweening;
using System;
using System.Collections.Generic;
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

    [Space(10)] [Header("Animation Properties")]
    [SerializeField] private float attackAnimDuration;


    protected override void Awake()
    {
        base.Awake();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        currentSpellSO = null;
        defenseForceField.SetActive(false);
    }

    public void PlayerMove()
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
            animator.SetFloat("Speed", attackAnimDuration / animDuration);
            transform.DOJump(opponentPos + offsetDir * meleeDistanceOffset, 1, 1, animDuration).SetEase(Ease.Linear).OnComplete(() => {
                int rand = UnityEngine.Random.Range(0, attackSoundsList.Count);
                SoundEffectManager.Instance.PlaySoundEffect(attackSoundsList[rand]);
            });
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

    public override void OnEntityTurn(HO_ElementalEffects elementalEffects)
    {
        // Reset DEF stat and attack valuues before each turn
        characterStats.ResetDEFToBase();
        currentAttackDMG = 0;
        attackElementalType = EElementalAttackType.Unknown;

        // Get the number of each potions
        int numHealthPotions = InventorySystem.Instance.GetConsumableAmountOfType(EConsumableType.Health_Potion);
        int numDefensePotions = InventorySystem.Instance.GetConsumableAmountOfType(EConsumableType.Defense_Potion);
        int numSpellPotions = InventorySystem.Instance.GetConsumableAmountOfType(EConsumableType.Fire_Potion) +
            InventorySystem.Instance.GetConsumableAmountOfType(EConsumableType.Water_Potion) +
            InventorySystem.Instance.GetConsumableAmountOfType(EConsumableType.Earth_Potion);

        // Decide what item to use,if there are any available consumables
        Consumable item = null;
        float hpInPercent = characterStats.GetCurrentHPInPercent();

        if (hpInPercent < 0.3 && numHealthPotions > 0)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Health_Potion);
        }
        else if (hpInPercent < 0.7 && numDefensePotions > 0)
        {
            item = InventorySystem.Instance.GetOneConsumableOfType(EConsumableType.Defense_Potion);
        }
        else if (hpInPercent > 0.7 && numSpellPotions > 0)
        {
            item = PickSpellPotion(elementalEffects.WeakToElementsList, elementalEffects.ResistantToElementsList);
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
                    item = PickSpellPotion(elementalEffects.WeakToElementsList, elementalEffects.ResistantToElementsList);
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
                healthNumber.OnChangeHP(item.ConsumableData.NumberValue);
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

    private Consumable PickSpellPotion(List<EElementalAttackType> weakToElementsList, List<EElementalAttackType> resistantToElementsList)
    {
        // Determine the third element that will have no damage modifiers
        List<EElementalAttackType> normalElementsList = new();
        foreach (EElementalAttackType element in Enum.GetValues(typeof(EElementalAttackType)))
        {
            if (element != EElementalAttackType.Unknown && !weakToElementsList.Contains(element) && !resistantToElementsList.Contains(element))
            {
                normalElementsList.Add(element);
                break;
            }
        }

        // Return one spell based on priority
        Consumable returnSpell = null;
        
        foreach (var weakElement in weakToElementsList)
        {
            returnSpell = InventorySystem.Instance.GetOneSpellOfType(weakElement);
            if (returnSpell != null) return returnSpell;
        }
        
        foreach (var normalElement in normalElementsList)
        {
            returnSpell = InventorySystem.Instance.GetOneSpellOfType(normalElement);
            if (returnSpell != null) return returnSpell;
        }

        foreach (var resistantElement in resistantToElementsList)
        {
            returnSpell = InventorySystem.Instance.GetOneSpellOfType(resistantElement);
            if (returnSpell != null) return returnSpell;
        }

        return returnSpell;
    }

    public override void EntityTakeDamage(int damage, EElementalAttackType attackElementalType = EElementalAttackType.Unknown, bool hasArmorPierce = false)
    {
        base.EntityTakeDamage(damage, attackElementalType, hasArmorPierce);
        defenseForceField.SetActive(false);
    }
}