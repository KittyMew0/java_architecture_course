package org.example.ocp;

public class EquilateralTriangle implements Shape {
    private int leg1;
    private int leg2;
    private double hypotenuse;

    public int getLeg1() {
        return leg1;
    }

    public int getLeg2() {
        return leg2;
    }

    public double getHypotenuse() {
        return hypotenuse;
    }

    public EquilateralTriangle(int leg1, int leg2) {
        this.leg1 = leg1;
        this.leg2 = leg2;
        this.hypotenuse = Math.sqrt(Math.pow(leg1, 2) + Math.pow(leg2, 2));
    }
}
