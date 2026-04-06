package ru.geekbrains.lesson1.store;

public class OrderItem {

    private static int couner = 5000;

    private  int id;
    private Product product;
    private int quantity;

    {
        id = ++couner;
    }

    public OrderItem(Product product){
        this.product = product;
        quantity = 1;

    }

    public OrderItem(Product product, int quantity){
        this.product = product;
        this.quantity = quantity;
    }

}
