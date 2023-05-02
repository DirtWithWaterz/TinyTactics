using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
public class TilePlacer : MonoBehaviourPun
{

    public TileBase[] TileBases;
    public Transform[] TileDataObjects;
    [SerializeField] private string selectedTile = null;
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private GameObject previewTile = null;

    public void Start()
    {
        tilemap = GameObject.Find("Buildings").GetComponent<Tilemap>();
    }
    private void Update()
    {
        if (selectedTile != null && UserInput.instance.Interact)
        {
            photonView.RPC(nameof(SetTileRPC), RpcTarget.AllBufferedViaServer, UserInput.instance.MousePosition, selectedTile);
        }
    }
    public void SelectTile(string tileName)
    {
        selectedTile = tileName;
    }
    [PunRPC]
    public void SetTileRPC(Vector3Int pos, string _string)
    {
        foreach (TileBase tile in TileBases)
        {
            if (tile.name != _string) { continue; }
            tilemap.SetTile(pos, tile);
        }
    }
}
