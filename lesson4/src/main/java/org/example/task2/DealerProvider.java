package org.example.task2;

public class DealerProvider {

    private final FactoryProvider factoryProvider;

    public DealerProvider(FactoryProvider factoryProvider) {
        this.factoryProvider = factoryProvider;
    }

    public ComponentInfo getComponent(int id) {
//        if (id < 0 || String.valueOf(id).length() < 6) {
//            throw new RuntimeException("Некорректный номер детали.");
//        }
//        return factoryProvider.getComponentInfo(id);


        ComponentInfo componentInfo = factoryProvider.getComponentInfo(id);
        if (componentInfo == null) {
            throw new RuntimeException("not found");
        }
        return componentInfo;


    }
}
