public class Stats
{
    public int Strength { get; set; }
    public int Armour { get; set; }
    public int MagicResist { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int AttackRange { get; set; }
    public float Speed { get; set; }

    public Stats(int strength, int armour, int magicResist, int health, int mana, int attackRange, float speed) {
        this.Strength = strength;
        this.Armour = armour;
        this.MagicResist = magicResist;
        this.Health = health;
        this.Mana = mana;
        this.AttackRange = attackRange;
        this.Speed = speed;
    }
}
