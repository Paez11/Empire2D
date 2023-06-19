using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] BuildingTypeSO activeBuildingType;
    UnitRTS unit;
    GatheringManager gatheringManager;
    private GameObject buildingPf; // Vista previa del edificio
    private bool isBuilding = false; // Indica si se está construyendo un edificio

    void Awake() 
    {
        unit = GetComponent<UnitRTS>();
        gatheringManager = GameObject.Find("GatheringManager").GetComponent<GatheringManager>();    
    }
    void Update()
    {
        if (isBuilding)
        {
            // Actualizar la posición de la vista previa del edificio según la posición del cursor
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            buildingPf.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);

            // Si se hace clic izquierdo, construir el edificio
            if (Input.GetMouseButtonDown(0))
            {
                //unit.MoveTo(buildingPf.transform.position);
                ConstructBuilding();
            }
        }
    }

    public void StartBuilding(GameObject buttonPrefab)
    {
        // Crear la vista previa del edificio
        buildingPf = Instantiate(buttonPrefab);
        isBuilding = true;
    }

    private void ConstructBuilding()
    {
        // Obtener los costos del edificio seleccionado
        BuildingCost buildingCost = buildingPf.GetComponent<BuildingCost>();
        // Verificar si hay suficientes recursos
        if (gatheringManager.HasEnoughResources(buildingCost.woodCost, buildingCost.foodCost, buildingCost.stoneCost, buildingCost.goldCost))
        {
            // Gastar los recursos necesarios
            gatheringManager.SpendResources(buildingCost.woodCost, buildingCost.foodCost, buildingCost.stoneCost, buildingCost.goldCost);

            // Crear el edificio
            Instantiate(buildingPf, buildingPf.transform.position, Quaternion.identity);

            // Destruir la vista previa del edificio
            Destroy(buildingPf);
        }
        else
        {
            // No hay suficientes recursos, destruir la vista previa del edificio
            Destroy(buildingPf);
        }

        // Reiniciar el estado del BuildingManager
        isBuilding = false;
    }
}
