using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public class OnTileEventArgs {
        public OnTileEventArgs(GameObject tile, GameObject oldTile) { Tile = tile; OldTile = oldTile; }
        public OnTileEventArgs(GameObject tile) { Tile = tile; }
        public GameObject Tile { get; }
        public GameObject OldTile { get; }
    }

    public delegate void OnTilePlacement(object sender, OnTileEventArgs e);
    public static event OnTilePlacement onTilePlacement;

    public delegate void OnTileRemoval(object sender, OnTileEventArgs e);
    public static event OnTileRemoval onTileRemoval;

    public delegate void OnHeroDrag(object sender, OnTileEventArgs e, bool afterTileRemoval);
    public static event OnHeroDrag onHeroDrag;

    public delegate void OnRoundStart(object sender);
    public static event OnRoundStart onRoundStart;

    public static void RaiseOnTilePlacement(object sender, OnTileEventArgs e) => onTilePlacement?.Invoke(sender, e);
    public static void RaiseOnTileRemoval(object sender, OnTileEventArgs e) => onTileRemoval?.Invoke(sender, e);
    public static void RaiseOnHeroDrag(object sender, OnTileEventArgs e, bool afterTileRemoval) => onHeroDrag?.Invoke(sender, e, afterTileRemoval);
    public static void RaiseOnRoundStart(object sender) => onRoundStart?.Invoke(sender);

}
