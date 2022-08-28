using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AloneSpace
{
    public class MapPanel : MonoBehaviour
    {
        public enum MapPanelViewMode
        {
            All,
            Mini,
        }

        [SerializeField] MapPanelCell cellTemplate;
        [SerializeField] Transform cellParent;
        [SerializeField] Camera mapCamera;
        [SerializeField] Camera stackTargetCamera;
        [SerializeField] Transform cameraOffset;
        [SerializeField] Transform cameraAnchor;
        [SerializeField] RenderTexture renderTextureTarget;

        QuestData questData;

        MapPanelCell[] mapPanelCells;
        
        public MapPanelViewMode ViewMode { get; private set; }

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandGlobalMapFocusCell.AddListener(SetGlobalMapFocusCell);
            MessageBus.Instance.UserCommandSetCameraAngle.AddListener(SetCameraAngle);
            
            RefreshLayout();

            ApplyViewMode(MapPanelViewMode.Mini);
        }

        public void ApplyViewMode(MapPanelViewMode viewMode)
        {
            ViewMode = viewMode;

            UpdateView();

            UpdateCamera();
        }

        void SetGlobalMapFocusCell(int index, bool immediate)
        {
            UpdateView();
        }

        void UpdateCamera()
        {
            if (ViewMode == MapPanelViewMode.All)
            {
                var mapCameraData = mapCamera.GetUniversalAdditionalCameraData();
                mapCameraData.renderType = CameraRenderType.Overlay;
                mapCamera.targetTexture = null;
                
                var stackTargetCameraData = stackTargetCamera.GetUniversalAdditionalCameraData();
                if (!stackTargetCameraData.cameraStack.Contains(mapCamera))
                {
                    stackTargetCameraData.cameraStack.Insert(0, mapCamera);
                }

                cameraOffset.position = mapPanelCells[questData.ObserveActor.CurrentAreaIndex].transform.position;
                mapCamera.orthographicSize = 1.2f;
                cameraAnchor.transform.Rotate(new Vector3(15.0f, 10.0f, 0.0f));
            }
            else
            {
                var bounds = new Bounds();
                var routeIndexes = questData.ObserveActor.GetRouteAreaData().Select(x => x.AreaIndex);
                foreach (var routeIndex in routeIndexes)
                {
                    bounds.Encapsulate(mapPanelCells[routeIndex].transform.position);
                }
                
                var mapCameraData = mapCamera.GetUniversalAdditionalCameraData();
                mapCameraData.renderType = CameraRenderType.Base;
                mapCamera.targetTexture = renderTextureTarget;
                
                var stackTargetCameraData = stackTargetCamera.GetUniversalAdditionalCameraData();
                if (stackTargetCameraData.cameraStack.Contains(mapCamera))
                {
                    stackTargetCameraData.cameraStack.Remove(mapCamera);
                }
                
                cameraOffset.position = bounds.center;
                mapCamera.orthographicSize = (bounds.max * 0.5f).magnitude;
            }
        }

        void UpdateView()
        {
            var routeIndexes = questData.ObserveActor.GetRouteAreaData().Select(x => x.AreaIndex).ToArray();
            var (_, currentY, currentZ) = GetPosition(questData.ObserveActor.CurrentAreaIndex);
            var isCurrentOdd = currentZ % 2 == 1;
            
            for (var i = 0; i < mapPanelCells.Length; i++)
            {
                if (ViewMode == MapPanelViewMode.All)
                {
                    var (__, y, z) = GetPosition(i);
                    var isOdd = currentZ % 2 == 1;
                    if (currentY < (y + ((isCurrentOdd && isOdd) ? -1 : 0)))
                    {
                        mapPanelCells[i].gameObject.SetActive(false);
                        continue;
                    }
                    
                    mapPanelCells[i].gameObject.SetActive(true);
                }
                else
                {
                    if (!routeIndexes.Contains(i))
                    {
                        mapPanelCells[i].gameObject.SetActive(false);
                        continue;
                    }
                    
                    mapPanelCells[i].gameObject.SetActive(true);
                }

                mapPanelCells[i].UpdateView(GetColor(i, routeIndexes));
            }
        }

        void RefreshLayout()
        {
            if (mapPanelCells != null)
            {
                foreach (var mapPanelCell in mapPanelCells)
                {
                    Destroy(mapPanelCell);                
                }
            }
            
            mapPanelCells = new MapPanelCell[questData.MapData.MapSize];

            for (var i = 0; i < mapPanelCells.Length; i++)
            {
                var index = i;
                mapPanelCells[index] = Instantiate(cellTemplate, cellParent);
                mapPanelCells[index].Initialize();
                
                var (x, y, z) = GetPosition(i);
                var isEvenNumberZ = z % 2 == 0;
                var evenNumberOffset = isEvenNumberZ ? 0.0f : 0.5f;
                mapPanelCells[i].transform.localPosition = new Vector3(x + evenNumberOffset, y + evenNumberOffset, z / 2.0f);
            }
        }

        void OnClickCell(int index)
        {
            MessageBus.Instance.PlayerCommandSetDestinateAreaIndex.Broadcast(questData.ObservePlayer.InstanceId, index);
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(index, false);
        }

        static Color GetColor(int index, int[] routeIndexes)
        {
            if (routeIndexes.First() == index)
            {
                return new Color(0.2f, 0.4f, 0.2f);
            }
                
            if (routeIndexes.Last() == index)
            {
                return new Color(0.2f, 0.4f, 0.5f);
            }
                
            if (routeIndexes.Contains(index))
            {
                return new Color(0.5f, 0.6f, 0.2f);
            }

            return new Color(0.5f, 0.5f, 0.5f, 0.1f);
        }

        (int x, int y, int z) GetPosition(int index)
        {
            var z = index / (questData.MapData.MapSizeX * questData.MapData.MapSizeY);
            var zi = index % (questData.MapData.MapSizeX * questData.MapData.MapSizeY);
            var y = zi / questData.MapData.MapSizeX;
            var x = zi % questData.MapData.MapSizeX;
            
            return (x, y, z);
        }

        void SetCameraAngle(Quaternion quaternion)
        {
            cameraAnchor.rotation = quaternion;
        }
    }
}
