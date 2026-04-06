package org.example.lsp.sample1;

public class Bird {
    private boolean canFly = true;

    public Bird(boolean canFly) {
        this.canFly = canFly;
    }

    public Bird() {

    }

    public boolean isCanFly() {
        return canFly;
    }

    public void fly(){
        System.out.println("flying");
    }
}
