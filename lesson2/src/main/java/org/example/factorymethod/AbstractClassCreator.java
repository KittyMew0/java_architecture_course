package org.example.factorymethod;

public abstract class AbstractClassCreator {

    // фабричный метод
    public abstract Product factoryMethod();

    // бизнес-логика
    public void anOperation() {
        Product product = factoryMethod();
        product.operation();
    }
}