using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//임시 빌딩 오브젝트 생성 담당
public class BuildingGhost : MonoBehaviour {

    public GridBuildingSystem3D buildingSystem;
    private Transform visual;

    private void Start() {
        RefreshVisual();

        buildingSystem.OnSelectedChanged += Instance_OnSelectedChanged;
    }
    private void OnDestroy()
    {
        buildingSystem.OnSelectedChanged -= Instance_OnSelectedChanged;
    }
    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        RefreshVisual();
    }

    private void LateUpdate() {
        //빌딩시스템의 메소드(선택한 건물에 따라 위치 계산)를 통해 위치 GET
        Vector3 targetPosition = buildingSystem.GetMouseWorldSnappedPosition();
        //그리드에 붙도록 Y=0
        targetPosition.y = 0f;
        //해당 위치로 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        //건물 회전값을 받아서 해당 로테이션값으로 부드럽게 회전
        transform.rotation = Quaternion.Lerp(transform.rotation, buildingSystem.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }
    //임시 건물를 생성하는 메소드
    private void RefreshVisual() {
        //먼저 visual 오브젝트가 있다면 파괴
        if (visual != null) {
            Destroy(visual.gameObject);
            visual = null;
        }
        //빌딩시스템에서 선택한 빌딩 종류를 받아라
        PlacedObjectTypeSO placedObjectTypeSO = buildingSystem.GetPlacedObjectTypeSO();
        //선택한 빌딩이 없다면 return
        if (placedObjectTypeSO == null) return;
        //선택한 빌딩이 있다면
        //해당 빌딩의 임시 오브젝트 생성
        visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity, this.transform);
        visual.localPosition = Vector3.zero;
        visual.localEulerAngles = Vector3.zero;
        //오브젝트레이어 설정
        SetLayerRecursive(visual.gameObject, 12);
        
    }
    //12번 레이어를 파랑색으로 렌더링하도록 URP rendering 세팅을 했기 때문에
    private void SetLayerRecursive(GameObject targetGameObject, int layer) {
        //전체 오브젝트의 레이어를 변경후
        targetGameObject.layer = layer;
        //자식 오브젝트로 
        foreach (Transform child in targetGameObject.transform) {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

}

