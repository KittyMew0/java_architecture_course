package org.example.factorymethod;

public class ConcreteCreator3 extends AbstractClassCreator {

    @Override
    public Product factoryMethod() {
        return new ConcreteProduct3();
    }
}
