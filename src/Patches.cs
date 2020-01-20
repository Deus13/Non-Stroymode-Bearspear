using Harmony;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace nonStroyBearspear
{


    [HarmonyPatch(typeof(Inventory), "AddGear", new Type[] { typeof(GameObject) })]
    internal class Inventory_AddGear
    {
        private static void Prefix(Inventory __instance, ref GameObject go)
        {
            var settings = nonStroyBearspearSettings.Instance;

            GearItem gearItem = go.GetComponent<GearItem>();
            Implementation.Log("uppre" + gearItem.name);
            if (gearItem.name == "GEAR_BearSpearStory")
            {

                GameObject gearItemtmp = Resources.Load("GEAR_BearSpear") as GameObject;

                go.GetComponent<GearItem>().m_NarrativeCollectibleItem = gearItemtmp.GetComponent<GearItem>().m_NarrativeCollectibleItem;
                go.GetComponent<GearItem>().m_DegradeOnUse = gearItemtmp.GetComponent<GearItem>().m_DegradeOnUse;
                go.GetComponent<GearItem>().m_DegradeOnUse.m_DegradeHP = settings.Degrade;
                go.GetComponent<GearItem>().m_Harvest = gearItemtmp.GetComponent<GearItem>().m_Harvest;
                go.GetComponent<GearItem>().m_Harvest.m_YieldGearUnits[0] = 1;                      //head
                go.GetComponent<GearItem>().m_Harvest.m_YieldGearUnits[1] = settings.SpearHarvestSaplings;                      //sapling
                go.GetComponent<GearItem>().m_AutoEquipOnInteract = false;

                go.GetComponent<GearItem>().m_WeightKG = settings.Weight;
               

                // go.GetComponent<GearItem>().name = "GEAR_BearSpearStory";
                // go.GetComponent<GearItem>().m_CurrentHP = gearItem.m_CurrentHP;
            }
            if (gearItem.name == "GEAR_BearSpearBroken")
            {

                
               
                go.GetComponent<GearItem>().m_Harvest.m_YieldGearUnits[0] = settings.BrokenSpearHarvestSaplings;                     //sapling
                go.GetComponent<GearItem>().m_Harvest.m_YieldGearUnits[1] = settings.BrokenSpearHarvestScrapmetal;                     //scrap
                
                // go.GetComponent<GearItem>().name = "GEAR_BearSpearStory";
                // go.GetComponent<GearItem>().m_CurrentHP = gearItem.m_CurrentHP;
            }

        }
    }






    [HarmonyPatch(typeof(PlayerStruggle), "RetrieveBearHuntReduxSettings")]

    internal static class PlayerStruggle_RetrieveBearHuntReduxSettings
    {
        private static void Postfix(PlayerStruggle __instance, ref BearHuntReduxSettings __result)
        {
            if (__result == null)
            {
                BearHuntAiRedux bearHuntAiRedux = __instance.m_PartnerBaseAi.m_BearHuntAiRedux;
                if (!bearHuntAiRedux)
                {
                    bearHuntAiRedux = new BearHuntAiRedux();
                }
                BearEncounter currentBearEncounterOwner = bearHuntAiRedux.GetCurrentBearEncounterOwner();
                if (!currentBearEncounterOwner)
                {
                    currentBearEncounterOwner = new BearEncounter();
                }

                var settings = nonStroyBearspearSettings.Instance;

                currentBearEncounterOwner.m_BearHuntReduxSettings = (BearHuntReduxSettings)BearHuntReduxSettings.CreateInstance("BearHuntReduxSettings");
                currentBearEncounterOwner.m_BearHuntReduxSettings.m_StruggleTapStrengthScale = GameManager.GetCustomMode().m_StruggleTapStrengthScale * settings.TapstrengthScale;
                currentBearEncounterOwner.m_BearHuntReduxSettings.m_StrugglePlayerDamageReceivedScale = GameManager.GetCustomMode().m_StrugglePlayerDamageReceivedScale * settings.DamageReceivedScale;
                currentBearEncounterOwner.m_BearHuntReduxSettings.m_StrugglePlayerDamageReceivedIntervalScale = GameManager.GetCustomMode().m_StrugglePlayerDamageReceivedIntervalScale * settings.DamageReceivedIntervalScale;
                currentBearEncounterOwner.m_BearHuntReduxSettings.m_StrugglePlayerClothingDamageScale = GameManager.GetCustomMode().m_StrugglePlayerClothingDamageScale * settings.ClothingDamageScale;

                
                __result = currentBearEncounterOwner.m_BearHuntReduxSettings;
            }
        }
    }

    [HarmonyPatch(typeof(BearSpearItem), "OnStruggleHitEnd")]
    internal static class BearSpearItem_OnStruggleHitEnd
    {
        private static bool Prefix(BearSpearItem __instance, BaseAi ___m_HitAi, ref float ___m_HitDamage, ref float ___m_HitBleedOutMinutes, LocalizedDamage ___m_LocalizedDamage, Vector3 ___m_HitPosition, Vector3 ___m_HitSourcePosition, GearItem ___m_GearItem)
        {
            var settings = nonStroyBearspearSettings.Instance;

            if (!___m_HitAi)
            {
                return false;
            }
            if (GameManager.GetPlayerStruggleComponent().GetBearSpearStruggleOutcome() == BearSpearStruggleOutcome.Failed)
            {
                ___m_HitDamage = ___m_HitDamage*settings.Damagefailed;
                ___m_HitBleedOutMinutes = ___m_HitDamage * settings.Bleedfailed;
            }
            else
            {
                ___m_HitDamage = ___m_HitDamage * settings.DamageSuccess;
                ___m_HitBleedOutMinutes = ___m_HitDamage * settings.BleedSuccess;
            }
            if (___m_LocalizedDamage)
            {
                ___m_HitAi.SetupDamageForAnim(___m_HitPosition, ___m_HitSourcePosition, ___m_LocalizedDamage);
            }
            ___m_HitAi.ApplyDamage(___m_HitDamage, ___m_HitBleedOutMinutes, DamageSource.Player, string.Empty);
            if (___m_GearItem)
            {
                ___m_GearItem.DegradeOnUse();
            }
            if (___m_GearItem.IsWornOut())
            {
               
                AccessTools.Method(typeof(BearSpearItem), "Break").Invoke(__instance, null);
            }
            return false;
        }
    }


    [HarmonyPatch(typeof(BaseAi), "MaybeCollideWithSpear")]
    internal static class BaseAi_MaybeCollideWithSpear
    {
        private static void Postfix(BaseAi __instance)
        {
            if (!(bool)__instance.m_BearHuntAiRedux)
            {
                PlayerManager playerManagerComponent = GameManager.GetPlayerManagerComponent();
                if ((bool)playerManagerComponent.m_ItemInHands)
                {
                    BearSpearItem bearSpearItem = playerManagerComponent.m_ItemInHands.m_BearSpearItem;
                    if ((bool)bearSpearItem && bearSpearItem.IsRaised())
                    {
                        bearSpearItem.UpdateCollision(__instance);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Panel_Log), "RemoveBearSpearInSandbox", null)]
    internal static class Panel_Log_RemoveBearSpearInSandbox
    {
        
        private static bool Prefix(List<BlueprintItem> ___m_BlueprintItemList)
        {
            var settings = nonStroyBearspearSettings.Instance;
            for (int num = ___m_BlueprintItemList.Count - 1; num >= 0; num--)
            {
                if (___m_BlueprintItemList[num].m_CraftedResult.name == "GEAR_BearSpear")
                {
                    GameObject gearItemtmp = Resources.Load("GEAR_BearSpearStory") as GameObject;
                    ___m_BlueprintItemList[num].m_CraftedResult = gearItemtmp.GetComponent<GearItem>();
                    ___m_BlueprintItemList[num].m_RequiredGearUnits[0] = settings.CostSaplings;
                    ___m_BlueprintItemList[num].m_DurationMinutes = settings.DurationSpear;
                }
                if (___m_BlueprintItemList[num].m_CraftedResult.name == "GEAR_SpearHead")
                {
                    ___m_BlueprintItemList[num].m_RequiredGearUnits[0] = settings.CostHead;
                    ___m_BlueprintItemList[num].m_DurationMinutes = settings.DurationSpearhead;
                }
            }
            return false;
        }  
    }
}