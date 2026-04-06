package org.example.lsp.sample1;

public class Main {
    public static void main(String[] args) {
        Bird bird1 = new Bird();
        Bird bird2 = new Bird();
        Bird bird3 = new Bird();

        Ostrich ostrich1 = new Ostrich();

        Bird[] birds = new Bird[3];
        birds[0] = bird1;
        birds[1] = bird2;
        birds[2] = ostrich1;

        flyBirds(birds);
    }

    public static void flyBirds(Bird[] birds) {
        for (Bird bird : birds) {
            if (bird.isCanFly()) {
                bird.fly();
            }

        }
    }
}
