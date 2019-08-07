using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseHero : MonoBehaviour 
{
    // Constants =========================================================
    private const string PLAYER_TILE_TAG = "PlayerTile";
    private const string PLAYER_BANK_TILE_TAG = "PlayerBankTile";

    // Public variables ==================================================
    public Stats stats;
    public bool inCombat; // are we currently in combat with someone?

    // Private variables =================================================
    private Vector3 originalPosition;
    private bool onTile; // is the hero currently on a tile?
    private GameObject currentTile;
    private NavMeshAgent navMeshAgent;
    private GameObject gameController;

    public abstract Stats InitialiseStats();
    
    public void NavMeshAgentControl(bool enable) {
        if (enable)
            navMeshAgent.enabled = true;
        else
            navMeshAgent.enabled = false;
    }

    public void GetClosestEnemy (object sender) {
        if (inCombat || transform.tag == "EnemyHero" || !navMeshAgent.enabled)
            return;

        float minDistance = Mathf.Infinity;
        GameObject closestHero = null;

        GameController gc = gameController.GetComponent<GameController>();
        foreach (GameObject enemyHero in gc.enemyHeroes) {
            float distance = Vector3.Distance(this.transform.position, enemyHero.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestHero = enemyHero;
            }
        }
        Vector3 movePostition = new Vector3(closestHero.transform.position.x, closestHero.transform.position.y, closestHero.transform.position.z - 2f);
        if (closestHero != null)
            Move(movePostition);
    }

    public void Move(Vector3 position) {
        //NavMeshAgentControl(true);
        navMeshAgent.SetDestination(position);
    }

    private void OnEnable() {
        EventManager.onRoundStart += GetClosestEnemy;
    }

    private void OnDisable() {
        EventManager.onRoundStart -= GetClosestEnemy;
    }

    private void Start() {
        originalPosition = transform.position;
        stats = InitialiseStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameController = GameObject.FindGameObjectWithTag("GameController");

        // Disable NavMeshAgent so the hero doesnt get force teleported onto the NavMesh:
        NavMeshAgentControl(false);

        if (!CheckTilesBelowHero(true))
            transform.position = originalPosition;
    }

    private bool CheckTilesBelowHero(bool start) {
        bool success = false;
        // Create a new vector where a tile is expected (all tiles are at 0 Y):
        Vector3 tilePos = new Vector3(transform.position.x, 0, transform.position.z);
        Collider[] colliders;
        // Check if there are any colliders at the new vector:
        if ((colliders = Physics.OverlapSphere(tilePos, 0.01f /*Radius*/)).Length >= 1) {
            // For each collider check if it is a Tile or a BankTile, and whether the Tile is occupied:
            foreach (Collider col in colliders) {
                GameObject tile = col.gameObject;
                if ((tile.tag == PLAYER_TILE_TAG || tile.tag == PLAYER_BANK_TILE_TAG) && !tile.GetComponent<Tile>().isOccupied) {
                    // Set the hero's position to be centered above the tile:
                    transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.5f, tile.transform.position.z);

                    // Notify the tile that something was placed on it:
                    if (start) {
                        // If the hero was just initialized only notify of tile placement:
                        EventManager.RaiseOnTilePlacement(this, new EventManager.OnTileEventArgs(tile));
                    }
                    else {
                        // If the hero was succesfully moved using a mouse - notify of tile placement 
                        // and notify the previous tile that it has been freed:
                        EventManager.RaiseOnTilePlacement(this, new EventManager.OnTileEventArgs(tile, currentTile));
                        EventManager.RaiseOnTileRemoval(this, new EventManager.OnTileEventArgs(currentTile));
                    }
                    // Set the new tile as the current tile:
                    currentTile = tile;
                    // Enable NavMeshAgent so the hero starts following the NavMesh:
                    if (!start && tile.tag != PLAYER_BANK_TILE_TAG)
                        NavMeshAgentControl(true);
                    // Only set success to true if the hero was moved and placed succesfully:
                    success = true;
                }
            }
        }
        return success;
    }

    private void OnMouseDown() {
        // Disable NavMeshAgent on mouse down so the hero isnt stuck to the NavMesh:
        NavMeshAgentControl(false);

        originalPosition = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + 0.5f, currentTile.transform.position.z);
        EventManager.RaiseOnHeroDrag(this, new EventManager.OnTileEventArgs(currentTile), false);
    }

    private void OnMouseDrag() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // create a logical plane at this object's position
        // and perpendicular to world Y:
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;     // this will return the distance from the camera
        if (plane.Raycast(ray, out distance)) {  // if plane hit...
            Vector3 pos = ray.GetPoint(distance);   // get the point
            transform.position = pos;   // pos has the position in the plane you've touched
        }
    }

    private void OnMouseUp() {
        EventManager.RaiseOnHeroDrag(this, new EventManager.OnTileEventArgs(currentTile), false);

        if (!CheckTilesBelowHero(false))
            transform.position = originalPosition;
    }
}
