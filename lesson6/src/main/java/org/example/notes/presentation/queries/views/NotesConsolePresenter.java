package org.example.notes.presentation.queries.views;

import org.example.notes.core.application.interfaces.NotesPresenter;
import org.example.notes.core.domain.Note;

import java.util.Collection;

public class NotesConsolePresenter implements NotesPresenter {
    @Override
    public void printAll(Collection<Note> note) {
        for (Note note : notes) {
            System.out.println(note);
        }
    }
}
