//package org.example.ocp;
//
//import java.util.ArrayList;
//import java.util.List;
//
//public class Main {
//
//    public static void main(String[] args) {
//        List<Shape> shapes = new ArrayList<>();
//        shapes.add(new EquilateralTriangle(2, 3));
//        shapes.add(new Square(4));
//        shapes.add(new Circle(3));
//
//        double sumArea = 0;
//        for (Shape shapes : shapes) {
//            if (shape instanceof EquilateralTriangle) {
//                EquilateralTriangle triangle = (EquilateralTriangle) shape;
//                sumArea += triangle.getLeg1() triangle.getLeg2() / 2.0;
//            }
//
//            if (shape instanceof Square) {
//                Square square = (Square) shape;
//                sumArea += Math.pow(square.getSide(), 2);
//            }
//            if (shape instanceof Circle) {
//                Circle circle = (Circle) shape;
//                sumArea += Math.PI * circle.getRadius() * circle.getRadius();
//            }
//        }
//
//        System.out.println("Sum is... whatever" + sumArea);
//
//        List<ShapeV2> shapesV2 = new ArrayList<>();
//        shapes.add(new EquilateralTriangleV2(2, 3));
//        shapes.add(new SquareV2(4));
//        shapes.add(new CircleV2(3));
//
//        double sumAreaV2 = 0;
//
//        for (ShapeV2 shape : shapesV2) {
//            sumAreaV2 += shape.getArea();
//        }
//
//        System.out.println("Sum is... whatever" + sumAreaV2);
//    }
//}
