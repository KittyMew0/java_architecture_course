package org.example;

import org.example.database.NotesDatabase;
import org.example.notes.core.application.ConcreteNoteEditor;
import org.example.notes.infrastructure.persistence.NotesDbContext;
import org.example.notes.presentation.queries.controllers. NotesController;
import org.example.notes.presentation.queries.views.NotesConsolePresenter;

public class Main {
    public static void main(String[] args) {
        NotesController controller = new NotesController(
                new ConcreteNoteEditor(new NotesDbContext(new NotesDatabase()), new NotesConsolePresenter()));
        controller.routeGetAll();
    }
}
