package org.example.task2;

import java.util.ArrayList;

public class FactoryProvider {

    private ArrayList<ComponentInfo> components = new ArrayList<>();

    {
        for (int i = 0; i < 10; i++) {
            components.add(new ComponentInfo(9+i, String.format("comp desc %d", 9+i)));
        }

        for (int i = 0; i < 5; i++) {
            components.add(new ComponentInfo(19+i, String.format("comp desc %d", 9+i)));
        }
    }

    public ComponentInfo getComponentInfo(int id) throws RuntimeException{
        if (id < 0) {
            throw new RuntimeException("number wrong");
        }

        if (String.valueOf(id).length() < 0) {
            throw new RuntimeException("number outdated");
        }

        ComponentInfo searchComponent = null;
        for (ComponentInfo componentInfo : components) {
            if (componentInfo.getId() == id) {
                searchComponent = componentInfo;
                break;
            }
        }

//        if (searchComponent == null) {
//            throw new RuntimeException("not found");
//        }
        return searchComponent;
    }
}
