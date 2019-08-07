using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool isOccupied = false;

    public List<GameObject> neighbourTiles = new List<GameObject>();

    public float DiagonalDistance(Transform currentTile, Transform goalTile) {
        float normalCost = 1f;
        float diagonalCost = 1f;
        float dx = Mathf.Abs(currentTile.position.x - goalTile.position.x);
        float dz = Mathf.Abs(currentTile.position.z - goalTile.position.z);
        return normalCost * (dx + dz) + (diagonalCost - 2f * normalCost) * Mathf.Min(dx, dz);
    }

    public virtual void FindNeighbourTiles() {
        neighbourTiles.Clear();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1.5f, Vector3.up);

        foreach (RaycastHit hit in hits) {
            if (hit.transform != this.transform && (hit.transform.tag == "PlayerTile" || hit.transform.tag == "EnemyTile") 
                && !neighbourTiles.Contains(hit.transform.gameObject)) {
                neighbourTiles.Add(hit.transform.gameObject);
            }
        }
    }

    private void Update() {
        //FindNeighbourTiles();
    }

    private void Start() {
        FindNeighbourTiles();
    }

    private void SetTileColor(object sender, EventManager.OnTileEventArgs e) {
        if (e.OldTile != null)
            e.OldTile.GetComponent<Renderer>().material.color = Color.white;

        e.Tile.GetComponent<Tile>().isOccupied = true;
        e.Tile.GetComponent<Renderer>().material.color = Color.green;
    }

    private void RemoveTileColor(object sender, EventManager.OnTileEventArgs e) {
        e.Tile.GetComponent<Tile>().isOccupied = false;
        //e.Tile.GetComponent<Renderer>().material.color = Color.white;
        SetDragColor(sender, e, true);
    }

    private void SetDragColor(object sender, EventManager.OnTileEventArgs e, bool afterRemoveTile) {
        Renderer renderer = GetComponent<Renderer>();
        if (afterRemoveTile && !isOccupied) {
            renderer.material.color = Color.white;
            return;
        }

        if (transform.parent.tag == "PlayerHeroRow" && !GetComponent<Tile>().isOccupied)
            renderer.material.color = Color.white;

        if (transform.parent.tag == "PlayerPlayRow" && !GetComponent<Tile>().isOccupied) {
            renderer.material.color = Color.magenta;
            //e.Tile.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    private void OnEnable() {
        EventManager.onTilePlacement += SetTileColor;
        EventManager.onTileRemoval += RemoveTileColor;
        EventManager.onHeroDrag += SetDragColor;
    }

    private void OnDisable() {
        EventManager.onTilePlacement -= SetTileColor;
        EventManager.onTileRemoval -= RemoveTileColor;
        EventManager.onHeroDrag -= SetDragColor;
    }
}
