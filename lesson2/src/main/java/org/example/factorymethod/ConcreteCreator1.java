package org.example.factorymethod;

public class ConcreteCreator1 extends AbstractClassCreator {

    @Override
    public Product factoryMethod() {
        return new ConcreteProduct1();
    }
}