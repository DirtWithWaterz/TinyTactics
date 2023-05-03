using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
public class TilePlacer : MonoBehaviourPun
{

    public Tile[] Tiles;
    public Transform[] TileDataObjects;
    [SerializeField] private string selectedTile = null;
    [SerializeField] private byte width;

    [SerializeField] private Tilemap backgroundMap = null;
    [SerializeField] private Tilemap groundMap = null;
    [SerializeField] private Tilemap buildingsMap = null;
    [SerializeField] private Tilemap miscMap = null;
    [SerializeField] private Tilemap foregroundMap = null;

    [SerializeField] private GameObject previewTile = null;
    private SpriteRenderer previewRenderer;

    public void Start()
    {
        buildingsMap = GameObject.Find("Buildings").GetComponent<Tilemap>();
        groundMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        previewRenderer = previewTile.GetComponent<SpriteRenderer>();
        previewRenderer.color = new Color(255, 255, 255, 50);
    }
    private void Update()
    {
        if (selectedTile != null && UserInput.instance.Interact)
        {
            photonView.RPC(nameof(SetTileRPC), RpcTarget.AllBufferedViaServer, UserInput.instance.MousePosition, selectedTile, width);
            Debug.Log(selectedTile);
            Debug.Log(width);
        }
        Vector3Int pos = new Vector3Int(
            Mathf.FloorToInt(UserInput.instance.MousePosition.x * 2 + 1), 
            Mathf.FloorToInt(UserInput.instance.MousePosition.y * 2 + 1), 
            0);
        Vector3 worldPosition = buildingsMap.CellToWorld(pos);
        previewTile.transform.position = worldPosition;
    }
    public void SelectTile(string tileName)
    {
        char lastChar = tileName[tileName.Length - 1];
        if (char.IsDigit(lastChar))
        {
            string str = lastChar.ToString();
            width = byte.Parse(str);
            selectedTile = tileName.Substring(0, tileName.Length - 1);
            foreach(Tile tile in Tiles){
                if(tile.name == selectedTile){
                    previewRenderer.sprite = tile.sprite;
                }
            }
        }

    }
    [PunRPC]
    public void SetTileRPC(Vector3 pos, string _string, byte width)
    {
        foreach (Tile tile in Tiles)
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
