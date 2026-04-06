package org.example.factorymethod;

public class ConcreteProduct1 implements Product {

    @Override
    public void operation() {
        System.out.println("ConcreteProduct1 created");
    }
}