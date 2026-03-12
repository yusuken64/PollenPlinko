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
    public House HousePrefab;

    public Camera GameCamera => HiveCamera;

    public LayerMask HiveItemLayerMask;
    public LayerMask HexLayerMask;

	void Start()
    {
        HexGrid.GenerateHexRing(1);
    }

    public void HandleUpdate(Vector3 worldPoint)
    {
        //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //if (hit.collider != null)
        //{
        //    hit.collider.GetComponent<Egg>()?.Hatch();
        //}
    }

    HiveItem _selectedItem;
    Vector3 _mouseDownPos;
    bool _dragging;

    float dragThreshold = 0.2f;

    public void HandleMouseDown(Vector3 worldPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, HiveItemLayerMask);

        if (hit.collider != null)
        {
            _selectedItem = hit.collider.GetComponent<HiveItem>();
            _mouseDownPos = worldPoint;
            _dragging = false;
        }
    }

    private  Hex _lastValidHex;

    public void HandleMouseDrag(Vector3 worldPoint)
    {
        if (_selectedItem == null) return;

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
            _selectedItem.transform.position = worldPoint;

            // Check hex under mouse
            Hex hoveredHex = GetHexAtWorldPoint(worldPoint);

            if (hoveredHex != null)
            {
                _lastValidHex = hoveredHex;
            }
        }
    }

    public void HandleMouseUp(Vector3 worldPoint)
    {
        if (_selectedItem == null) return;

        if (!_dragging)
        {
            _selectedItem.HandleClick();
        }
        else if (_lastValidHex != null)
        {
            _selectedItem.transform.position = _lastValidHex.transform.position;
            _lastValidHex.SetEgg(_selectedItem, this);
        }

        _selectedItem = null;
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

    public void SpawnHouse()
    {
        var newHouse = Instantiate(HousePrefab, this.transform);
        var hex = HexGrid.GetEmptyHex();

        if (hex != null)
        {
            newHouse.Setup(hex, Game);
        }
    }
}
