package org.example.automobilefunctions;

import java.awt.*;

public class Harvester extends Car implements Fueling, Wiping {

    private Refueling refueling;
    private Wiping wiping;

    public Harvester(String make, String model, Color color) {
        super(make, model, color);
        setWheelsCount(6);
        this.fuelType = FuelType.Diesel;
    }

    public void setRefuelingStation(Refueling refueling) {
        this.refueling = refueling;
    }

    public void setCarWipingStation(Wiping wiping) {
        this.wiping = wiping;
    }

    @Override
    public void movement() {

    }

    @Override
    public void maintenance() {

    }

    @Override
    public boolean gearShifting() {
        return false;
    }

    @Override
    public boolean switchHeadLights() {
        return false;
    }

    @Override
    public boolean SwitchWipers() {
        return false;
    }

    @Override
    public void sweeping() {
        System.out.println("automobile is sweeping something");
    }

    @Override
    public void wipMirrors() {
        if (wiping != null) {
            wiping.wipMirrors();
        }
    }

    @Override
    public void wipWindshield() {
        if (wiping != null) {
            wiping.wipWindshield();
        }
    }

    @Override
    public void wipHeadlights() {
        if (wiping != null) {
            wiping.wipHeadlights();
        }
    }

    @Override
    public void fuel() {
        if (refueling != null) {
            refueling.fuel(fuelType);
        }
    }

}
