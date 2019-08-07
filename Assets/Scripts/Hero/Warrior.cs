using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : BaseHero, IFighter {

    public override Stats InitialiseStats() {
        return new Stats(
        50  /*Strength*/,
        25  /*Armour*/,
        20  /*Magic Resist*/,
        600 /*Health*/,
        100 /*Mana*/,
        1   /*Range*/,
        0.5f/*Attack Speed*/);
    }
}
