package org.example.factorymethod;

public class ConcreteCreator2 extends AbstractClassCreator {

    @Override
    public Product factoryMethod() {
        return new ConcreteProduct2();
    }
}