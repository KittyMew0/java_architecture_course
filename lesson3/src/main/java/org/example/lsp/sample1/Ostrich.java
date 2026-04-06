package org.example.lsp.sample1;

public class Ostrich extends Bird{

    public Ostrich() {
        super(false);
    }

    @Override
    public void fly() {
        throw new UnsupportedOperationException("An ostrich can't fly");
    }

    public void run() {
        System.out.println("Ostrich is running");
    }
}
