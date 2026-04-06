package org.example.automobilefunctions;

public class RefuelingStation implements Refueling {

    @Override
    public void fuel(FuelType fuelType) {
        switch (fuelType) {
            case Diesel -> System.out.println("the car is being fueled with diesel");
            case Gasoline -> System.out.println("the car is being fueled with gasoline");
        }
    }
}