package org.example.automobilefunctions;

import java.awt.*;

public class Main {
    // нужно было добавить... мойку и что то еще,
    // вроде сделано, ндао чекнуть
    public static void main(String[] args) {
        Harvester harvester = new Harvester("A", "b", Color.BLACK);

        RefuelingStation refuelingStation = new RefuelingStation();
        CarWipingStation carWashStation = new CarWipingStation();

        harvester.setRefuelingStation(refuelingStation);
        harvester.setCarWipingStation(carWashStation);

        harvester.fuel();
        harvester.wipHeadlights();
        harvester.wipMirrors();
        harvester.wipWindshield();
    }

    public static double calculateMaintenance(Car car) {
        if (car.getWheelsCount() == 6) {
            return 1500 * 6;
        }
        else {
            return 1000 * 4;
        }
    }
}