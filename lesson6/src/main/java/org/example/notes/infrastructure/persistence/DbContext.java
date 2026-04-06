package org.example.notes.infrastructure.persistence;

public class DbContext {
    protected Database database;

    public DbContext(Database database) {
        this.database = database;
    }

    protected abstract void onModelCreating(ModelBuilder builder);

    public boolean saveChanges() {
    // TODO: Сохранение изменений в БД
        return true;
    }
}
