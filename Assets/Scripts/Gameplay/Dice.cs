using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    [System.Serializable]
    public struct Die
    {
        // This struct is used to generate a random number between 0 and the max variable.
        public int max;

        public Die(int _max)
        {
            max = _max;
        }

        public int Roll(int rollAmount = 1)
        {
            int total = 0;

            for (int i = 0; i < rollAmount; i++)
            {
                total += Random.Range(1, max + 1);
            }

            return total;
        }
    }

    [System.Serializable]
    public struct Dice
    {
        // This struct is essentially just a wrapper for the Die struct to make serialization a little nicer.
        public Die die;
        public int amount;

        public Dice(Die _die, int _amount)
        {
            die = _die;
            amount = _amount;
        }

        public int Roll()
        {
            return die.Roll(amount);
        }
    }

    [System.Serializable]
    public struct DicePool
    {
        // This struct uses the dice class to allow for a serialized list of different amount of different dice to all be rolled at once.
        public List<Dice> dicePool;

        public int Roll()
        {
            int total = 0;

            foreach (Dice dice in dicePool)
            {
                total += dice.Roll();
            }

            return total;
        }
    }
}
