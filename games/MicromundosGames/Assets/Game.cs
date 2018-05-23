using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public Car car_to_instantiate;
	public static Game mInstance;

	public Color defaultColor;
	public Color color_car_id_1;
	public Color color_car_id_2;

	public List<Car> cars;
	public Transform carsContainer;
	public int maxCars;
	public static Game Instance
	{
		get
		{
			return mInstance;
		}
	}

	void Awake () {
		mInstance = this;
	}
	public Color GetColor(int id)
	{
		switch (id) {
		case 1:
			return color_car_id_1;
			break;
		case 2:
			return color_car_id_2;
			break;
		}
		return defaultColor;
	}
	public bool AddCar(Transform parent, int carId) {
		if (cars.Count > maxCars)
			return false;
		Car car = Instantiate (car_to_instantiate);
		car.transform.SetParent (carsContainer);
		car.transform.localEulerAngles = parent.localEulerAngles;
		car.transform.localPosition = parent.position;
		car.SetID( carId );
		cars.Add (car);
		return true;
	}
	public void RemoveCar(Car car)
	{
		cars.Remove (car);
		Destroy (car.gameObject);
	}
}
