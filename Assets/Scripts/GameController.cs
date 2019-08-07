using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject BuyHeroButton1, BuyHeroButton2, BuyHeroButton3, BuyHeroButton4, BuyHeroButton5;
    public GameObject DirectionWall;
    public int goldAmount = 100;
    public List<GameObject> enemyHeroes = new List<GameObject>();
    public List<GameObject> playerHeroes = new List<GameObject>();

    private GameObject[] playerBankTiles;

    public enum Hero {
        Dwarf,
        Elf,
        Warrior,
        Witch,
        Wizard,
        Ninja,
        Samurai,
        Mermaid,
        Ent,
        Dragon
    }

    public void RerollButtonOnClick() {
        if (goldAmount >= 2) {
            LoadHeroBuyButton(GetRandomHero(), BuyHeroButton1);
            LoadHeroBuyButton(GetRandomHero(), BuyHeroButton2);
            LoadHeroBuyButton(GetRandomHero(), BuyHeroButton3);
            LoadHeroBuyButton(GetRandomHero(), BuyHeroButton4);
            LoadHeroBuyButton(GetRandomHero(), BuyHeroButton5);

            goldAmount -= 2;
        }
    }

    public void BuyHeroButtonOnClick(GameObject button) {
        foreach (GameObject bankTile in playerBankTiles) {
            if (!bankTile.GetComponent<BankTile>().isOccupied) {
                // Place bought hero on this tile:
                Instantiate(Resources.Load<GameObject>("Prefabs/Default_Hero"), new Vector3(bankTile.transform.position.x, bankTile.transform.position.y + 0.5f, bankTile.transform.position.z), Quaternion.identity);
                return;
            }
        }
    }

    public void LevelUpButtonOnClick() {
        EventManager.RaiseOnRoundStart(this);
    }

    private Hero GetRandomHero() {
        Array values = Enum.GetValues(typeof(Hero));
        return (Hero)values.GetValue(Useful.RandomHero(values.Length));
    }

    private void LoadHeroBuyButton(Hero hero, GameObject button) {
        switch (hero) {
            case Hero.Dwarf:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Dwarf");
                break;
            case Hero.Elf:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Elf");
                break;
            case Hero.Warrior:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Warrior");
                break;
            case Hero.Witch:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Witch");
                break;
            case Hero.Wizard:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Wizard");
                break;
            case Hero.Ninja:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Ninja");
                break;
            case Hero.Samurai:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Samurai");
                break;
            case Hero.Mermaid:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Mermaid");
                break;
            case Hero.Ent:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Ent");
                break;
            case Hero.Dragon:
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/Dragon");
                break;
            default:
                return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get all player bank tiles:
        playerBankTiles = GameObject.FindGameObjectsWithTag("PlayerBankTile");

        // Initialise a few enemy heroes for testing:
        GameObject enemyHero = Instantiate(Resources.Load<GameObject>("Prefabs/Default_Hero"), new Vector3(6.5f, 0.5f, 6.5f), Quaternion.LookRotation(-Vector3.forward));
        enemyHero.tag = "EnemyHero";
        enemyHeroes.Add(enemyHero);

        enemyHero = Instantiate(Resources.Load<GameObject>("Prefabs/Default_Hero"), new Vector3(8.5f, 0.5f, 8.5f), Quaternion.LookRotation(-Vector3.forward));
        enemyHero.tag = "EnemyHero";
        enemyHeroes.Add(enemyHero);

        enemyHero = Instantiate(Resources.Load<GameObject>("Prefabs/Default_Hero"), new Vector3(8.5f, 0.5f, 10.5f), Quaternion.LookRotation(-Vector3.forward));
        enemyHero.tag = "EnemyHero";
        enemyHeroes.Add(enemyHero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
