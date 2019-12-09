using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalApplication
{
    class Cards
    {

        public enum Type
        {
            clubs,
            hearts,
            spades,
            diamonds
        }
        public enum Value
        {
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9,
            ten = 10,
            eleven = 11

        }
        public enum CardName
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }


        private CardName _cardName;
        private Value _value;
        private Type _suit;
        public CardName Name
        {
            get { return _cardName; }
            set { _cardName = value; }
        }
        public Value value
        {
            get { return _value; }
            set { _value = value; }
        }
        public Type Suit
        {
            get { return _suit; }
            set { _suit = value; }
        }

        public Cards(CardName Name, Value value, Type Suit)
        {
            _cardName = Name;
            _value = value;
            _suit = Suit;
        }
    }
}
