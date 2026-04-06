package ru.geekbrains.lesson1.store3D.models;

//TODO: Доработать в рамках ДР

import java.util.List;

public class Scene {

    public int id;
    public List<PoligonalModel> models;
    public List<Flash> flashes;

    public Scene(int id,
                 List<PoligonalModel> models,
                 List<Flash> flashes) {

        this.id = id;
        this.models = models;
        this.flashes = flashes;
    }

    public Object method1(Object type) {
        return type;
    }

    public Object method2(Object type1, Object type2) {
        return type1;
    }
}