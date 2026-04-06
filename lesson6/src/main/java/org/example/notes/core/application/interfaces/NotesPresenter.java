package org.example.notes.core.application.interfaces;

import org.example.notes.core.domain.Note;

import java.util.Collection;

public interface NotesPresenter {
    void printAll(Collection<Note> note);
}
