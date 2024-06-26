using UnityEngine;

public class HO_ProgressTracker : MonoBehaviour
{
    [Header("Player Progress Tracker Properties")]
    [SerializeField] private int currentRegionLevel;
    [SerializeField] private int roomsDefeated;
    [SerializeField] private int maxRoomsPerRegion;


    #region Singleton
    public static HO_ProgressTracker Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private void Start()
    {
        currentRegionLevel = 1;
        roomsDefeated = 0;
    }

    public int GetCurrentRegionLevel()
    {
        return currentRegionLevel;
    }

    public void RoomHasBeenDefeated()
    {
        roomsDefeated++;

        if (roomsDefeated == maxRoomsPerRegion)
        {
            roomsDefeated = 0;
            currentRegionLevel++;
        }
    }

    public bool IsBossRoom()
    {
        return (roomsDefeated == maxRoomsPerRegion - 1);
    }
}
