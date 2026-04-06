package ru.geekbrains.lesson1.store;

import java.util.ArrayList;
import java.util.List;

public class Store {

    private List<Order> orders = new ArrayList<>();

    public boolean addOrder(Order order){
        orders.add(order);
        return true;
    }

    public boolean cancelOrder(int id){
        //TODO: Отмена заказа
        return true;
    }

    public boolean paymentOrder(int id)
    {
        //TODO: Оплата заказа
        return true;
    }

}
