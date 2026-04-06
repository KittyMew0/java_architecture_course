package org.example.factorymethod;

public class Main {

    public static void main(String[] args) {

        AbstractClassCreator creator;

        creator = new ConcreteCreator1();
        creator.anOperation();

        creator = new ConcreteCreator2();
        creator.anOperation();

        creator = new ConcreteCreator3();
        creator.anOperation();
    }
}