package org.example;

import java.util.ArrayList;
import java.util.Collection;

public class Model3D implements Entity {
    private int id;
    private Collection<Texture> textures = new ArrayList<>();

    @Override
    public int getId() {
        return id;
    }

    public Model3D() {

    }

    public Model3D(Collection<Texture> textures) {
        this.textures = textures;
    }

    public Collection<Texture> getTextures() {
        return textures;
    }

    @Override
    public String toString() {
        return String.format("3DModel #%s", id);
    }
}
