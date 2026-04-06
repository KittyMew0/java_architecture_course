package org.example.automobilefunctions;

public class CarWipingStation implements Wiping {

    @Override
    public void wipMirrors() {
        System.out.println("wiping mirrors");
    }

    @Override
    public void wipWindshield() {
        System.out.println("wiping windshield");
    }

    @Override
    public void wipHeadlights() {
        System.out.println("wiping headlights");
    }
}
