package org.example.automobilefunctions;

import java.awt.*;

public abstract class Car {
    public Car(String make, String model, Color color) {
        this.make = make;
        this.model = model;
        this.color = color;
    }

    private String make;

    private String model;

    private Color color;

    protected CarType type;

    private int wheelsCount;

    protected FuelType fuelType;

    private GearboxType gearboxType;

    private double engineCapacity;

    private boolean fogLights = false;

    public abstract void movement();

    public abstract void maintenance();

    public abstract boolean gearShifting();

    public abstract boolean switchHeadLights();

    public abstract boolean SwitchWipers();

    public abstract void sweeping();

    public boolean switchForLights() {
        fogLights = !fogLights;
        return fogLights;
    }

    protected void setWheelsCount(int wheelsCount) {
        this.wheelsCount = wheelsCount;
    }

    public int getWheelsCount() {
        return wheelsCount;
    }
}
