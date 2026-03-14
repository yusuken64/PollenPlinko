using System.Linq;
using UnityEngine;

public class Hive : MonoBehaviour, ISubGame
{
    public Game Game;
    public Camera HiveCamera;
    public HexGrid HexGrid;

    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 20f;

    public Nurse NursePrefab;
    public HiveItem HousePrefab;
    public HiveItem StoragePrefab;
    public HiveItem FlowerPrefab;

    public Camera GameCamera => HiveCamera;

    public LayerMask HiveItemLayerMask;
    public LayerMask HexLayerMask;

    public MergeManager MergeManager;

    public Vector3 _mouseDownPos;
    public bool _dragging;
    public float dragThreshold = 0.2f;

    void Start()
    {
        HexGrid.GenerateHexRing(1);
    }

    public void HandleMouseDown(Vector3 worldPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, HiveItemLayerMask);

        if (hit.collider != null)
        {
            HexGrid.SelectedItem = hit.collider.GetComponent<HiveItem>();
            _mouseDownPos = worldPoint;
            _dragging = false;
        }
    }

    public void HandleMouseDrag(Vector3 worldPoint)
    {
        if (HexGrid.SelectedItem == null) return;

        if (!_dragging)
        {
            float dist = Vector3.Distance(worldPoint, _mouseDownPos);
            if (dist > dragThreshold)
            {
                _dragging = true;
            }
        }

        if (_dragging)
        {
            // Move visually with mouse
            HexGrid.SelectedItem.transform.position = worldPoint;

            // Check hex under mouse
            Hex hoveredHex = GetHexAtWorldPoint(worldPoint);

            HexGrid.PreviewHover(hoveredHex, HexGrid.SelectedItem);

            if (hoveredHex != null)
            {
                HexGrid.LastValidHex = hoveredHex;
            }
        }
    }

    public void HandleMouseUp(Vector3 worldPoint)
    {
        if (HexGrid.SelectedItem == null) return;

        if (!_dragging)
        {
            HexGrid.SelectedItem.HandleClick();
        }
        else if (HexGrid.LastValidHex != null)
        {
            HexGrid.SelectedItem.transform.position = HexGrid.LastValidHex.transform.position;
            HexGrid.LastValidHex.SetItem(HexGrid.SelectedItem, this);

            var cluster = HexGrid.GetMergeCluster(HexGrid.LastValidHex, HexGrid.SelectedItem);
            if (cluster.Count() >= 3)
            {
                StartCoroutine(MergeManager.MergeRoutine(cluster, HexGrid.LastValidHex, HexGrid.SelectedItem));
            }
        }

        HexGrid.ClearHighlight();
        HexGrid.SelectedItem = null;
        _dragging = false;
    }

    Hex GetHexAtWorldPoint(Vector3 worldPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, HexLayerMask);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Hex>();
        }

        return null;
    }

    public void SpawnNurse()
	{
        var newNurse = Instantiate(NursePrefab, this.transform);
        newNurse.Hive = this;
    }

	public void SpawnItem(HiveItem itemPrefab)
	{
		var newHouse = Instantiate(itemPrefab, this.transform);
		var hex = HexGrid.GetEmptyHex();

		if (hex != null)
		{
			newHouse.Setup(hex, Game, 1);
		}
	}
}
