using System;
using System.Collections.Generic;
using System.Linq;

namespace Guandan.Game
{
    public enum PlayKind
    {
        Invalid,
        Single,
        Pair,
        Triple,
        FullHouse,
        Straight,
        ConsecutivePairs,
        SteelPlate,
        Bomb,
        StraightFlush,
        JokerBomb,
    }

    [Serializable]
    public readonly struct WildcardUse
    {
        public readonly string CardId;
        public readonly int RepresentedRank;
        public readonly CardSuit RepresentedSuit;

        public WildcardUse(string cardId, int representedRank, CardSuit representedSuit = CardSuit.Hearts)
        {
            CardId = cardId;
            RepresentedRank = representedRank;
            RepresentedSuit = representedSuit;
        }
    }

    [Serializable]
    public sealed class PlayPattern
    {
        public PlayKind Kind { get; }
        public IReadOnlyList<Card> Cards { get; }
        public IReadOnlyList<WildcardUse> Wildcards { get; }
        public int MainRank { get; }
        public int SequenceHighRank { get; }

        public bool IsBomb => Kind == PlayKind.Bomb || Kind == PlayKind.StraightFlush || Kind == PlayKind.JokerBomb;

        public PlayPattern(
            PlayKind kind,
            IEnumerable<Card> cards,
            int mainRank,
            int sequenceHighRank = 0,
            IEnumerable<WildcardUse> wildcards = null)
        {
            Kind = kind;
            Cards = cards.OrderBy(card => card.Id, StringComparer.Ordinal).ToArray();
            MainRank = mainRank;
            SequenceHighRank = sequenceHighRank;
            Wildcards = wildcards?.OrderBy(use => use.CardId, StringComparer.Ordinal).ToArray()
                ?? Array.Empty<WildcardUse>();
        }

        public string Identity
        {
            get
            {
                var ids = string.Join(",", Cards.Select(card => card.Id));
                var wilds = string.Join(",", Wildcards.Select(use => $"{use.CardId}:{use.RepresentedRank}:{use.RepresentedSuit}"));
                return $"{Kind}|{MainRank}|{SequenceHighRank}|{ids}|{wilds}";
            }
        }

        public string Label => Kind switch
        {
            PlayKind.Single => "Single",
            PlayKind.Pair => "Pair",
            PlayKind.Triple => "Triple",
            PlayKind.FullHouse => "Full House",
            PlayKind.Straight => "Straight",
            PlayKind.ConsecutivePairs => "Consecutive Pairs",
            PlayKind.SteelPlate => "Consecutive Triples",
            PlayKind.Bomb => $"{Cards.Count}-card Bomb",
            PlayKind.StraightFlush => "Straight Flush",
            PlayKind.JokerBomb => "Joker Bomb",
            _ => "Invalid",
        };
    }
}
