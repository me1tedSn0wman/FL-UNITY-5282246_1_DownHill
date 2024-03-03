using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuSceneManager : Singleton<MainMenuSceneManager>
{
    public GameObject carMeshAnchor;
    public Dictionary<int, GameObject> dictOfCarMeshes;

    public float anchorRatotionSpeed = 0.1f;

    public class CarMeshRef
    {
        public int carId;

        public bool isBlocked;
        public int carPrice;

        public GameObject carMesh;

        public CarMeshRef(int carId, bool isBlocked, int carPrice, GameObject carMesh){
            this.carId = carId;
            this.isBlocked = isBlocked;
            this.carPrice = carPrice;
            this.carMesh = carMesh;
        }

        public void SetActive(bool value) {
            carMesh.SetActive(value);
        }

        public void SetBlocked(bool value) {
            this.isBlocked = value;
        }

    }

    public List<CarMeshRef> listofCarsMesh;

    public int crntCarMeshRefID = 0;
    public int chosenCarId = 0;

    public CinemachineVirtualCamera virtCam;

    public void Awake()
    {
        listofCarsMesh= new List<CarMeshRef>();
        MakeListOfCars();
        ShowCar(0);
    }
    public void Update()
    {
        RotateAnchor();
    }

    public void MakeListOfCars() {

//        Debug.Log(GameManager.Instance.dictOfCars.Count);
        foreach (KeyValuePair<int, CarReference> kvp in GameManager.Instance.dictOfCars)
        {
            CarReference crntCarRef = kvp.Value;
            GameObject crntCarGO = Instantiate(crntCarRef.carMeshPrefab, carMeshAnchor.transform);
            crntCarGO.SetActive(false);

            bool crntCarIsBlocked =crntCarRef.isBlocked;
            if (GameManager.Instance.unlockedCars.Contains(crntCarRef.carId)) {
//                Debug.Log("Unlock: " + crntCarRef.carId);
                crntCarIsBlocked = false; 
            }

            CarMeshRef crntCarMeshRef = new CarMeshRef(crntCarRef.carId, crntCarIsBlocked, crntCarRef.carPrice, crntCarGO);
            listofCarsMesh.Add(crntCarMeshRef);
        }
    }

    public void ShowCar(int index) {
        listofCarsMesh[crntCarMeshRefID].SetActive(false);
        listofCarsMesh[index].SetActive(true);

        CarMeshRef crntCarMeshRef = listofCarsMesh[index];
        int chosenCarId = crntCarMeshRef.carId;
        GameManager.Instance.chosenCarId = chosenCarId;
        MainMenuUIManager.Instance.SetCarLockUI(crntCarMeshRef.isBlocked, crntCarMeshRef.carPrice);

//        virtCam.Follow = crntCarMeshRef.carMesh.transform;
        virtCam.LookAt = crntCarMeshRef.carMesh.transform;

        crntCarMeshRefID = index;

    }

    public void ShowNextCar() {
        ShowCar((crntCarMeshRefID + 1) % listofCarsMesh.Count);
    }

    public void ShowPrevCar()
    {
        int tmp = crntCarMeshRefID - 1;
        if (tmp < 0) tmp = listofCarsMesh.Count-1;

        ShowCar(tmp);
    }

    public bool TryUnlockCar() {
        if (!listofCarsMesh[crntCarMeshRefID].isBlocked) return false;
        if (GameManager.Instance.TrySpendPoints(listofCarsMesh[crntCarMeshRefID].carPrice)) {
            GameManager.Instance.UnlockCar(listofCarsMesh[crntCarMeshRefID].carId);
            listofCarsMesh[crntCarMeshRefID].SetBlocked(false);
            return true;
        }
        return false;
    }

    public void RotateAnchor() {
        carMeshAnchor.transform.Rotate(Vector3.up * anchorRatotionSpeed * Time.deltaTime);
    }

    public void UpdateCarList() {
        foreach (CarMeshRef carMeshRef in listofCarsMesh) {
            if (GameManager.Instance.unlockedCars.Contains(carMeshRef.carId)) {
                carMeshRef.SetBlocked(false);
            }
        }
    }


}
