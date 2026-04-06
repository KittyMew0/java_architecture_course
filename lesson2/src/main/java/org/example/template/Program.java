package org.example.template;

public class Program {

    public static String data = """
            em some data
            some
            data
            something
            something again
            well...
            """;

    public static void main(String[] args) {
        LogReader logReader = new PoemReader(data);
        logReader.setCurrentPosition(3);

        for (LogEntry log : logReader.readLogObject()) {
            System.out.println(log.getText());
        }
    }
}
