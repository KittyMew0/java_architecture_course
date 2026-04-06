package org.example.task3;

//Разработать контракты и компоненты системы "Покупка онлайн билетов на автобус в час пик
//* 1. Предусловия.
//* 2. Постусловия.
//+ 3. Инвариант.
//* 4. Определить обстрактные и конкретные классы.
//* 5. Определить интерфейсы.
//* 6. Реализовать наследование.
//* 7. Выявить компоненты.
//* 8. Разработать Диаграмму компонент используя нотацию UML 2.0. Общая без деталей.

import java.util.Collection;
import java.util.Date;

public class Program {
    public static void main(String[] args) {

        Core core = new Core();
        MobileApp mobileApp = new MobileApp(core.getTicketProvider(), core.getCustomerProvider());
        BusStation busStation = new BusStation(core.getTicketProvider());

        if (mobileApp.buyTicket("11000060221")) {
            System.out.println("Клиент успешно купил билет..");
            mobileApp.searchTicket(new Date());
            Collection<Ticket> tickets = mobileApp.getTickets();
            if (busStation.checkTicket(tickets.stream().findFirst().get().getQrcode())) {
                System.out.println("Клиент успешно прошел в автобус.");
            }
        }
    }
}