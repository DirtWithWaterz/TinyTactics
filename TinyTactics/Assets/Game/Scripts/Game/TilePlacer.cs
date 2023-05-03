using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
public class TilePlacer : MonoBehaviourPun
{

    public TileBase[] TileBases;
    public Transform[] TileDataObjects;
    [SerializeField] private string selectedTile = null;
    [SerializeField] private byte width;
    [SerializeField] private Tilemap buildingsMap = null;
    [SerializeField] private Tilemap groundMap = null;
    [SerializeField] private GameObject previewTile = null;

    public void Start()
    {
        buildingsMap = GameObject.Find("Buildings").GetComponent<Tilemap>();
        groundMap = GameObject.Find("Ground").GetComponent<Tilemap>();
    }
    private void Update()
    {
        if (selectedTile != null && UserInput.instance.Interact)
        {
            photonView.RPC(nameof(SetTileRPC), RpcTarget.AllBufferedViaServer, UserInput.instance.MousePosition, selectedTile, width);
            Debug.Log(selectedTile);
            Debug.Log(width);
        }
    }
    public void SelectTile(string tileName)
    {
        char lastChar = tileName[tileName.Length - 1];
        if (char.IsDigit(lastChar))
        {
            string str = lastChar.ToString();
            width = byte.Parse(str);
            selectedTile = tileName.Substring(0, tileName.Length - 1);
        }

    }
    [PunRPC]
    public void SetTileRPC(Vector3 pos, string _string, byte width)
    {
        foreach (TileBase tile in TileBases)
        {
            if (tile.name != _string) { continue; }
            Vector3Int position = new Vector3Int(Mathf.FloorToInt(pos.x * 2), Mathf.FloorToInt(pos.y * 2), 0);
            if (groundMap.HasTile(position)) { return; }
            if (!groundMap.HasTile(new Vector3Int(position.x, position.y - 1, 0))) { return; }
            bool colliderFound = false;
            for (int i = 1; i <= width; i++)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector3 worldPosition = buildingsMap.CellToWorld(new Vector3Int(position.x + (i * j), position.y, 0));
                    Collider2D collider = Physics2D.OverlapPoint(worldPosition);
                    if (collider != null)
                    {
                        colliderFound = true;
                        break;
                    }
                }
                if (colliderFound) { break; }
            }

            if (!colliderFound)
            {
                buildingsMap.SetTile(position, tile);
            }
        }
    }
}
