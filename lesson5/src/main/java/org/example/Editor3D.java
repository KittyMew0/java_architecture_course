package org.example;

import java.util.ArrayList;

public class Editor3D implements UILayer {

    private ProjectFile projectFile;
    private BusinessLogicalLayer businessLogicalLayer;
    private DatabaseAccess databaseAccess;
    private Database database;

    private Editor3D(BusinessLogicalLayer businessLogicalLayer) {
        this.businessLogicalLayer = businessLogicalLayer;
    }

    private void initialize() {
        database = new EditorDatabase(projectFile);
        databaseAccess = new EditorDatabaseAccess(database);
        businessLogicalLayer = new EditorBusinessLogicalLayer(databaseAccess);
    }

    @Override
    public void openProject(String fileName) {
        projectFile = new ProjectFile(fileName);
        initialize();
    }

    @Override
    public void showProjectSettings() {
        // предусловие
        checkProjectFile();

        System.out.println("project v1");
        System.out.println("***");
        System.out.printf("filename: %s\n", projectFile.getFileName());
        System.out.printf("setting1: %d\n", projectFile.getSetting1());
        System.out.printf("setting2: %s\n", projectFile.getSetting2());
        System.out.printf("setting3: %s\n", projectFile.getSetting3());
        System.out.println("***");

    }

    private void checkProjectFile() {
        if (projectFile == null) {
            throw new RuntimeException("File not opredelen");
        }
    }

    @Override
    public void saveProject() {
        checkProjectFile();
        database.save();
        System.out.println("file was saved. probably. or project");
    }

    @Override
    public void printAllModels() {
        checkProjectFile();

        ArrayList<Model3D> models = (ArrayList<Model3D>)businessLogicalLayer.getAllModels();
        for (int i = 0; i < models.size(); i++) {
            System.out.printf("=%d=\n", i);
            System.out.println(models.get(i));
            for (Texture texture : models.get(i).getTextures()) {
                System.out.println("\t%s\n", texture);
            }
        }
    }

    @Override
    public void printAllTextures() {
        checkProjectFile();

        ArrayList<Texture> texture = (ArrayList<Texture>)businessLogicalLayer.getAllTextures();
        for (int i = 0; i < texture.size(); i++) {
            System.out.printf("=%d=\n", i);
            System.out.println(texture.get(i));
        }
    }

    @Override
    public void renderAll() {
        checkProjectFile();
        System.out.println("Подождите...");
        long startTime = System.currentTimeMillis();
        businessLogicalLayer.renderAllModels();
        long endTime (System.currentTimeMillis() - startTime)
        System.out.printf("Oneрация выполнена за %d ис.\n", endTime);
    }

    @Override
    public void renderModel(int i) {
// Предусловие
        checkProjectFile();
        ArrayList<Model3D> models = (ArrayList<Model3D>)businesslogicalLayer.getAllModels();
        if (i < 0 || i > models.size()-1) {
            throw new RuntimeException("Номер модели указан некорректною.");
        }
        System.out.println("Подождите...");
        Long startTime = System.currentTimeMillis();
        businessLogicalLayer.renderModel(models.get(1));
        Long endTime (System.currentTimeMillis() - startTime);
        System.out.printf("Операция выполнена за %d mc.\n", endTime);
    }
}
