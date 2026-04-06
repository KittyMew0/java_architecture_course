package org.example;

import org.example.database.NotesDatabase;
import org.example.notes.core.application.ConcreteNoteEditor;
import org.example.notes.infrastructure.persistence.NotesDbContext;
import org.example.notes.presentation.queries.controllers. NotesController;
import org.example.notes.presentation.queries.views.NotesConsolePresenter;

public class Main {

    // в дз хочет видеть какую то схему...
    // или добавить че то, он не решил (ну там в мейн показать что
    // заметка добавляется, удаляется, бла бла бла...
    public static void main(String[] args) {
        NotesController controller = new NotesController(
                new ConcreteNoteEditor(new NotesDbContext(new NotesDatabase()), new NotesConsolePresenter()));
        controller.routeGetAll();
    }
}