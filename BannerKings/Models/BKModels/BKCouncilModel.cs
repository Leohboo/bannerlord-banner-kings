﻿using BannerKings.Managers.Court;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace BannerKings.Models.BKModels
{
    public class BKCouncilModel : IBannerKingsModel
    {
        public ExplainedNumber CalculateEffect(Settlement settlement)
        {
            return new ExplainedNumber();
        }


        public CouncilAction GetAction(CouncilActionType type, CouncilData council, Hero requester, CouncilMember targetPosition, CouncilMember currentPosition = null)
        {
            if (type == CouncilActionType.REQUEST)
                return GetRequest(type, council, requester, targetPosition, currentPosition);
            else if (type == CouncilActionType.RELINQUISH)
                return GetRelinquish(type, requester, currentPosition, targetPosition);
            else return GetSwap(type, council, requester, targetPosition, currentPosition);
        }



        private CouncilAction GetSwap(CouncilActionType type, CouncilData council, Hero requester, CouncilMember targetPosition, CouncilMember currentPosition = null)
        {
            CouncilAction action = new CouncilAction(type, requester, targetPosition, currentPosition);
            action.Influence = GetInfluenceCost(type, targetPosition);

            if (currentPosition == null || currentPosition.Member != requester)
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Not part of the council.");
                return action;
            }

            if (!targetPosition.IsValidCandidate(requester))
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Not a valid candidate.");
                return action;
            }

            if (requester.Clan != null && requester.Clan.Influence < action.Influence)
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Not enough influence.");
                return action;
            }

            if (targetPosition.IsCorePosition(targetPosition.Position))
            {
                if (requester.Clan != null && !requester.Clan.Kingdom.Leader.IsFriend(requester))
                {
                    action.Possible = false;
                    action.Reason = new TextObject("{=!}Not trustworthy enough for this position.");
                    return action;
                }

                if (council.GetCompetence(requester, targetPosition.Position) < 0.5f)
                {
                    action.Possible = false;
                    action.Reason = new TextObject("{=!}Not competent enough for this position.");
                    return action;
                }
            }

            action.Possible = true;
            action.Reason = new TextObject("{=!}Action can be taken.");
            return action;
        }

        private CouncilAction GetRelinquish(CouncilActionType type, Hero requester, CouncilMember currentPosition, CouncilMember targetPosition = null)
        {
            CouncilAction action = new CouncilAction(type, requester, targetPosition, currentPosition);
            action.Influence = GetInfluenceCost(type, targetPosition);
            if (currentPosition == null)
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}No position to be relinquished.");
                return action;
            }

            if (currentPosition.Member != requester)
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Not current councilman of the position.");
                return action;
            }

            action.Possible = true;
            action.Reason = new TextObject("{=!}Action can be taken.");
            return action;
        }

        private CouncilAction GetRequest(CouncilActionType type, CouncilData council, Hero requester, CouncilMember targetPosition, CouncilMember currentPosition = null)
        {
            CouncilAction action = new CouncilAction(type, requester, targetPosition, currentPosition);
            action.Influence = GetInfluenceCost(type, targetPosition);

            if (currentPosition != null && currentPosition.Member == requester)
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Already part of the council.");
                return action;
            }

            if (!targetPosition.IsValidCandidate(requester))
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Not a valid candidate.");
                return action;
            }

            if (requester.Clan != null && requester.Clan.Influence < action.Influence)
            {
                action.Possible = false;
                action.Reason = new TextObject("{=!}Not enough influence.");
                return action;
            }

            if (targetPosition.IsCorePosition(targetPosition.Position))
            {
                if (requester.Clan != null && !requester.Clan.Kingdom.Leader.IsFriend(requester))
                {
                    action.Possible = false;
                    action.Reason = new TextObject("{=!}Not trustworthy enough for this position.");
                    return action;
                }

                if (council.GetCompetence(requester, targetPosition.Position) < 0.5f)
                {
                    action.Possible = false;
                    action.Reason = new TextObject("{=!}Not competent enough for this position.");
                    return action;
                }
            }

            action.Possible = true;
            action.Reason = new TextObject("{=!}Action can be taken.");
            return action;
        }

        public int GetInfluenceCost(CouncilActionType type, CouncilMember targetPosition)
        {
            if (type == CouncilActionType.REQUEST)
            {
                if (targetPosition.Member != null)
                    return 100;
                else return 50;
            }
            else if (type == CouncilActionType.RELINQUISH)
                return 0;
            else
            {
                if (targetPosition.Member != null)
                    return 50;
                return 10;
            }
        }
    }
}
