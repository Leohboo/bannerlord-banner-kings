﻿using BannerKings.Managers.Court;
using BannerKings.Populations;
using System;
using TaleWorlds.CampaignSystem;

namespace BannerKings.Models.Populations
{
    public class BKCaravanAttractionModel : IBannerKingsModel
    {
        public ExplainedNumber CalculateEffect(Settlement settlement)
        {
            ExplainedNumber result = new ExplainedNumber(1f);

            PopulationData data = BannerKingsConfig.Instance.PopulationManager.GetPopData(settlement);
            result.Add(data.EconomicData.Mercantilism.ResultNumber * 10f);
            result.Add(data.MilitaryData.Militarism.ResultNumber * -100f);

            BannerKingsConfig.Instance.CourtManager.ApplyCouncilEffect(ref result, settlement.OwnerClan.Leader, CouncilPosition.Steward, 0.15f, true);
            return result;
        }
    }
}