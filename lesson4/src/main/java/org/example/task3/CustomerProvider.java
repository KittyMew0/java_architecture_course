package org.example.task3;

public class CustomerProvider {
    private Database database;

    public CustomerProvider(Database database) {
        this.database = database;
    }

    public CustomerProvider getCustomer(String login, String password) {
        return new Customer();
    }
}
